using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    public LayerMask groundLayer; //used for raycasting
    private Rigidbody2D rb;
    private BoxCollider2D playerCollider; //used for raycasting

    [Header("Horizontal movement variables")]
    public float maxSpeed;
    public float accelSpeed;
    public float groundDecelSpeed;
    private float horizontalMovement;
    private int lastFacingDirection;

    [Header("Jump variables")]
    public float jumpPower;
    public float totalJumps;
    public float airDecelSpeed;
    public float fallMultiplier;
    public float jumpCutMultiplier;
    public float jumpBufferCooldown;
    public float defaultGravity;
    private float jumpBufferTimer;
    public float lateJumpCooldown;
    private float lateJumpTimer;
    private float jumpCounter;
    private bool airJump;

    [Header("Dash variables")]
    public float doubleClickTime;
    public float dashSpeed;
    public float dashTime;
    public float dashCooldown;
    private float dashTimer;
    private float rLastClickTime;
    private float lLastClickTime;
    private float rTimeBetweenClicks;
    private float lTimeBetweenClicks;
    private bool isDashing;
    private bool dashRight;
    private bool dashLeft;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();

        dashTimer = dashCooldown;
    }


    void Update()
    {
        #region Keyboard Inputs
        //horizontal inputs (left and right arrow keys)
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            horizontalMovement = 0;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            horizontalMovement = 1;
            lastFacingDirection = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalMovement = -1;
            lastFacingDirection = -1;
        }
        else
        {
            horizontalMovement = 0;
        }

        //Jump input
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumpBufferTimer = jumpBufferCooldown;
        }
        else
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        //Checks if should jump in midair based on input and number of jumps remaining
        if (Input.GetKeyDown(KeyCode.UpArrow) && !Grounded() && jumpCounter > 0)
            airJump = true;

        //Dashing mechanic inputs
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            rTimeBetweenClicks = Time.time - rLastClickTime;
            if (rTimeBetweenClicks < doubleClickTime && dashTimer <= 0)
            {
                dashRight = true;
            }
            rLastClickTime = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            lTimeBetweenClicks = Time.time - lLastClickTime;
            if (lTimeBetweenClicks < doubleClickTime && dashTimer <= 0)
            {
                dashLeft = true;
            }
            lLastClickTime = Time.time;
        }
        dashTimer -= Time.deltaTime;
        #endregion

        //horizontal speed cap
        if (Mathf.Abs(rb.velocity.x) >= maxSpeed && !isDashing)
            rb.velocity = new Vector2(maxSpeed * Mathf.Sign(rb.velocity.x), rb.velocity.y);
        else if (Mathf.Abs(rb.velocity.x) >= dashSpeed && !isDashing)
            rb.velocity = new Vector2(dashSpeed * Mathf.Sign(rb.velocity.x), rb.velocity.y);

        if (Grounded()) //on the ground
        {
            //resets jump variables
            lateJumpTimer = lateJumpCooldown;
            jumpCounter = totalJumps;
        }
        else
            lateJumpTimer -= Time.deltaTime;

        //Flips sprite depending on direction moving
        if (Mathf.Sign(rb.velocity.x) == 1 && rb.velocity.x != 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); //face right
        }
        else if (Mathf.Sign(rb.velocity.x) == -1)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); //face left
        }
    }

    void FixedUpdate()
    {
        //Adds movement force when moving left or right
        rb.AddForce(Vector2.right * horizontalMovement * accelSpeed);

        #region Friction
        if (horizontalMovement == 0 || Mathf.Sign(horizontalMovement) != Mathf.Sign(rb.velocity.x)) //No input or opposing movement
        {
            float frictionForce;
            if (Grounded()) //ground
                frictionForce = Mathf.Min(Mathf.Abs(groundDecelSpeed * Time.fixedDeltaTime), Mathf.Abs(rb.velocity.x));
            else //air
                frictionForce = Mathf.Min(Mathf.Abs(airDecelSpeed * Time.fixedDeltaTime), Mathf.Abs(rb.velocity.x));

            frictionForce *= -Mathf.Sign(rb.velocity.x);
            rb.AddForce(Vector2.right * frictionForce, ForceMode2D.Impulse);
        }
        #endregion

        #region Jump
        //removes a jump if you fall off an edge without jumping when you can no longer late jump
        //Basically, replaces ground jump with the midair jump after some fall time
        if (lateJumpTimer < 0 && jumpCounter == totalJumps)
            jumpCounter--;

        //Jumping from ground
        if (jumpBufferTimer > 0 && lateJumpTimer > 0)
        {
            jumpCounter--;
            lateJumpTimer = 0;
            airJump = false;
            Jump();
        }
        else if (airJump) //mid-air jump
        {
            airJump = false;
            jumpCounter--;
            Jump();
        }

        //Falling speed multiplied
        if (rb.velocity.y <= 0 && !Grounded() && !isDashing)
            rb.AddForce(Vector2.down * fallMultiplier);

        //fall faster when jump key is released early
        if (!Input.GetKey(KeyCode.UpArrow) && !Grounded() && rb.velocity.y > 0 && !isDashing)
        {
            rb.AddForce(Vector2.down * jumpCutMultiplier * 0.5f, ForceMode2D.Impulse);
        }
        #endregion

        #region Dash
        if (dashRight)
        {
            Dash(1);
            dashRight = false;
        }
        if (dashLeft)
        {
            Dash(-1);
            dashLeft = false;
        }

        if (isDashing)
            //removes gravity during dash
            rb.gravityScale = 0;
        else
            //resets gravity after dash
            rb.gravityScale = defaultGravity;
        #endregion
    }

    private bool Grounded()
    {
        return Physics2D.Raycast(playerCollider.bounds.min, Vector2.down, 0.05f, groundLayer)
            || Physics2D.Raycast(new Vector2(playerCollider.bounds.max.x, playerCollider.bounds.min.y), Vector2.down, 0.05f, groundLayer);
    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    private void Dash(int direction_)
    {
        isDashing = true;

        if (direction_ == 1)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.right * dashSpeed, ForceMode2D.Impulse);

        }
        else if (direction_ == -1)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.right * -dashSpeed, ForceMode2D.Impulse);
        }

        Invoke("StopDash", dashTime);
        dashTimer = dashCooldown;
    }

    private void StopDash()
    {
        isDashing = false;
    }
}
