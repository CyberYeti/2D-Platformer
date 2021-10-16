using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float horizontalSpeed = 3;
    [SerializeField] float runSpeedMultiplier = 2f;
    [SerializeField] float jumpPower = 100f;
    [SerializeField] Transform groundCheckCollider;
    [SerializeField] LayerMask groundLayer;
    
    private Rigidbody2D rb;
    private Animator animator;
    private float horizontalValue;
    private bool isGrounded = false;
    private bool isFacingRight = true;
    private bool isRunning = false;
    private bool jumpFlag = false;

    const float GROUND_CHECK_RADIUS = 0.2f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //Get Horizontal Value
        horizontalValue = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.LeftControl)) //LShift down enable running
        {
            isRunning = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl)) //LShift up disable running
        {
            isRunning = false;
        }

        if (Input.GetKeyDown(KeyCode.Space)) //Space Pressed, Jump
        {
            jumpFlag = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space)) //Space Released, Stop Jump
        {
            jumpFlag = false;
        }
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Move(horizontalValue, jumpFlag);
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

    private void Move(float _horizontalDirection, bool _jumpFlag)
    {
        // if player is grounded and is pressing space jump
        if (isGrounded && _jumpFlag)
        {
            isGrounded = false;
            _jumpFlag = false;
            rb.AddForce(new Vector2(0, jumpPower));
        }

        #region Move And Run
        float newHorizontalVelocity = _horizontalDirection * horizontalSpeed * Time.fixedDeltaTime * 100;
        //if you are running multiply speed by runSpeedMultiplier
        if (isRunning)
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
