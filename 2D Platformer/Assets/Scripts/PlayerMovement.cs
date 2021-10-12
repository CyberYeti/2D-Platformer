using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float horizontalSpeed = 3;
    [SerializeField] float runSpeedMultiplier = 2f;
    
    private Rigidbody2D rb;
    private Animator animator;
    private float horizontalValue;
    private bool isFacingRight = true;
    private bool isRunning = false;

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
        if (Input.GetKeyUp(KeyCode.LeftControl)) //LShift up disable running
        {
            isRunning = false;
        }
    }

    private void FixedUpdate()
    {
        Move(horizontalValue);
    }

    private void Move(float _horizontalDirection)
    {
        float newHorizontalVelocity = _horizontalDirection * horizontalSpeed * Time.deltaTime * 100;
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
    }
}
