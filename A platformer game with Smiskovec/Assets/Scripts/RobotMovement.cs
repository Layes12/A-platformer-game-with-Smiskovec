using UnityEngine;

public class RobotMovement : MonoBehaviour
{
    // Adjustable parameters
    public float walkSpeed = 3.0f;
    public float runSpeed = 6.0f;
    public float jumpForce = 8.0f;
    public float slideDistance = 5.0f;
    public float dashDistance = 10.0f;

    // Components and flags
    private Rigidbody2D rb;
    private Animator anim;
    public bool isGrounded = true;
    public bool isSliding = false;
    public bool isDashing = false;
    public bool isRunning = false; // Flag to track if the robot is running

    // Movement speed
    private float moveSpeed;

    // Initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // FixedUpdate is called at a fixed interval
    // FixedUpdate is called at a fixed interval
    void FixedUpdate()
    {
        // Move the robot
        if (!isSliding)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            if (horizontalInput != 0f)
            {
                rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
            }
            else
            {
                // If no input, stop the robot's horizontal movement
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        // Check if the robot is grounded
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));

        // Movement input from user (you can modify this according to your input system)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction and speed
        Vector2 movement = new Vector2(horizontalInput, 0.0f).normalized;
        moveSpeed = isDashing ? runSpeed : (isRunning ? runSpeed : walkSpeed); // Set the moveSpeed here

        // Flip the sprite based on movement direction
        if (movement.x < 0) // If moving left
        {
            transform.localScale = new Vector3(-4.314114f, 4.314114f, 1); // Flip the sprite to face left
        }
        else if (movement.x > 0) // If moving right
        {
            transform.localScale = new Vector3(4.314114f, 4.314114f, 1); // Reset the sprite to face right
        }

        // Set animator parameters
        anim.SetFloat("Speed", moveSpeed);
        anim.SetBool("IsGrounded", isGrounded);

        // Jump
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        // Slide
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Slide();
        }

        // Run (Hold Left Shift to run)
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.W))
        {
            Dash();
        }
    }

    // Jump function
    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    // Slide function
    void Slide()
    {
        if (isGrounded)
        {
            isSliding = true;
            rb.AddForce(transform.right * slideDistance, ForceMode2D.Impulse);
            Invoke("StopSliding", 1.0f); // Slide duration, you can adjust this
        }
    }

    // Stop sliding
    void StopSliding()
    {
        isSliding = false;
    }

    // Dash function
    void Dash()
    {
        if (!isDashing)
        {
            isDashing = true;
            rb.AddForce(transform.right * dashDistance, ForceMode2D.Impulse);
            Invoke("StopDashing", 0.7f); // Dash duration, you can adjust this
        }
    }

    // Stop dashing
    void StopDashing()
    {
        isDashing = false;
    }
}
