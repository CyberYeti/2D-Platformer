using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float horizontalSpeed = 3;

    private Rigidbody2D rb;
    private float horizontalValue;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalValue = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        Move(horizontalValue);
    }

    private void Move(float _horizontalVelocity)
    {
        float newHorizontalVelocity = _horizontalVelocity * horizontalSpeed * Time.deltaTime * 100;
        Vector2 newVelocity = new Vector2(newHorizontalVelocity, rb.velocity.y);
        rb.velocity = newVelocity;
    }
}
