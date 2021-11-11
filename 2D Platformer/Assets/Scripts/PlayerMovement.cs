using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float horizontalSpeed = 3;
    [SerializeField] float runSpeedMultiplier = 2f;
    [SerializeField] float crouchSpeedMultiplier = 0.5f;
    [SerializeField] float jumpPower = 100f;
    [SerializeField] Transform groundCheckCollider;
    [SerializeField] Transform overheadCheckCollider;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] CapsuleCollider2D standingCollider;
    
    private Rigidbody2D rb;
    private Animator animator;
    private float horizontalValue;
    private bool isGrounded = false;
    private bool isFacingRight = true;
    private bool isRunning = false;
    private bool jump = false;
    private bool isCrouching = false;

    const float GROUND_CHECK_RADIUS = 0.2f;
    const float OVERHEAD_CHECK_RADIUS = 0.2f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //Get Horizontal Value
        horizontalValue = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.LeftControl)) //LControl down enable running
        {
            isRunning = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl)) //LControl up disable running
        {
            isRunning = false;
        }

        if (Input.GetButtonDown("Jump")) //Space Pressed, Jump
        {
            jump = true;
        }
        else if (Input.GetButtonUp("Jump")) //Space Released, Stop Jump
        {
            jump = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) //LShift Pressed, Crouch
        {
            isCrouching = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift)) //LShift Released, Stop Crouch
        {
            isCrouching = false;
        }
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Move(horizontalValue, jump, isCrouching);
    }

    private void GroundCheck()
    {
        isGrounded = false;
        //Check if ground check collider is colliding with other 2D colliders in the ground layer
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, GROUND_CHECK_RADIUS, groundLayer);
        if (colliders.Length > 0)//Grounded
        {
            isGrounded = true;
        }
    }

    private void Move(float _horizontalDirection, bool _jumpFlag, bool _crouchFlag)
    {
        #region Jump And Crouch
        //If we are crouching and uncrouch, Check above player head.
        //If there a ground layer item above us, don't uncrouch
        //Else uncrouch
        if (!_crouchFlag)
        {
            print("checking Overhead");
            print(Physics2D.OverlapCircle(overheadCheckCollider.transform.position, OVERHEAD_CHECK_RADIUS, groundLayer));
            if (Physics2D.OverlapCircle(overheadCheckCollider.transform.position, OVERHEAD_CHECK_RADIUS, groundLayer) != null)
            {
                _crouchFlag = true;
            }
            print(_crouchFlag);
        }

        if (isGrounded)
        {
            //if we press crouch, disable the standing collider, reduce speed and add crouching animation
            //if we disable crouch, enable the standing collider, resume original speed and stop crouching animation
            standingCollider.enabled = !_crouchFlag;
            if (_crouchFlag)
            {

            }
            else
            {

            }

            // if player is grounded and is pressing space jump
            if (_jumpFlag)
            {
                isGrounded = false;
                _jumpFlag = false;
                rb.AddForce(new Vector2(0, jumpPower));
            }
            
        }

        #endregion

        #region Move And Run
        float newHorizontalVelocity = _horizontalDirection * horizontalSpeed * Time.fixedDeltaTime * 100;
        //if you are crouching multiply speed by crouchSpeedMultiplier(slower)
        if (_crouchFlag)
        {
            newHorizontalVelocity *= crouchSpeedMultiplier;
        }
        
        //if you are running multiply speed by runSpeedMultiplier (higher)
        else if (isRunning)
        {
            newHorizontalVelocity *= runSpeedMultiplier;
        }
        
        Vector2 newVelocity = new Vector2(newHorizontalVelocity, rb.velocity.y);
        rb.velocity = newVelocity;

        //0 idle; 3 walking, 6 running
        //Set the float "X Velocity" to the magnitude of the horizontal velocity
        animator.SetFloat("X Velocity", Mathf.Abs(newHorizontalVelocity));


        if (isFacingRight && _horizontalDirection < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            isFacingRight = false;
        }
        else if (!isFacingRight && _horizontalDirection > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            isFacingRight = true;
        }
        #endregion
    }
}
