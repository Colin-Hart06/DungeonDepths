using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Setup")]
    public float moveSpeed = 5;
    public float moveForce = 10f;
    public float counterForce = 5f;
    
    [Header("Jump Setup")]
    public float jumpForce = 5;
    public Transform isGroundChecker;
    public float checkGroundRadius = 0.1f;
    public LayerMask groundLayer;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float coyoteTime = 0.1f;
    float lastTimeGrounded;
    bool isGrounded;
    public float maxFallSpeed = -10f;
    
    private Rigidbody2D rig;
    private SpriteRenderer spr;
    public Animator animator;
    private float horizontalInput;
    private Transform currentPlatform;
private Vector3 lastPlatformPosition;
    
    public bool inputDisabled;
    new audioManager audio;
    void Start()
    {
        audio = audioManager.instance;
        rig = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
    }
    private bool landSoundPlayed = true;
    void Update()
    {
        if(Time.timeScale==0f)
        return;
        if (inputDisabled)
        {
            if (rig.linearVelocity.y < 0)
            {
            animator.SetBool("IsJumping", true);
            }
            else
            animator.SetBool("IsJumping",false);
            return;
        }
        // Get input in Update for responsiveness
            horizontalInput = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
        
        Collider2D collider = CheckIfGrounded();
        if (isGrounded && !landSoundPlayed)
        {
            audio.Play("Land");
            landSoundPlayed =true;
        }
        else if (!isGrounded)
        {
            landSoundPlayed = false;
        }

        if(isGrounded&& Mathf.Abs(horizontalInput) > 0&&collider.tag=="Grass"&&!audio.IsPlaying("Grass Walk"))
        {
            audio.Play("Grass Walk");
        }
        else if(horizontalInput==0||!isGrounded||collider.tag!="Grass")
        {
           audio.StopPlaying("Grass Walk");
        }

        if (isGrounded && Mathf.Abs(horizontalInput) > 0 && collider.tag == "Stone" && !audio.IsPlaying("Stone Walk"))
        {
            audio.Play("Stone Walk");
        }
        else if (horizontalInput == 0 || !isGrounded || collider.tag != "Stone")
        {
            audio.StopPlaying("Stone Walk");
        }

        if (isGrounded && Mathf.Abs(horizontalInput) > 0 && collider.tag == "Wood" && !audio.IsPlaying("Wood Walk"))
        {
            audio.Play("Wood Walk");
        }
        else if (horizontalInput == 0 || !isGrounded || collider.tag != "Wood")
        {
            audio.StopPlaying("Wood Walk");
        }
        if (isGrounded && Mathf.Abs(horizontalInput) > 0 && collider.tag == "Brick" && !audio.IsPlaying("Brick Steps"))
        {
            audio.Play("Brick Steps");
        }
        else if (horizontalInput == 0 || !isGrounded || collider.tag != "Brick")
        {
            audio.StopPlaying("Brick Steps");
        }



        if (Input.GetKeyDown(KeyCode.Space) && !inputDisabled && Time.timeScale != 0)
        {
            Grapple grapple = GetComponent<Grapple>();

            // If at grapple endpoint
            if (grapple != null && grapple.IsAtEndpoint())
            {
                grapple.Detatch();
                // Only jump if not holding down
                if (Input.GetAxisRaw("Vertical") >= 0)
                {
                    Jump();
                }
            }
            // If still traveling to endpoint, just cancel
            else if (grapple != null && grapple.IsGrappling())
            {
                grapple.Detatch();
            }
            // Normal ground jump
            else if (isGrounded)
            {
                Jump();
            }
        }
    
    }
    
    void FixedUpdate()
{
    if(!inputDisabled)
    Move(horizontalInput);
    ApplyFallSpeedLimit();
    MoveWithPlatform(); 
}

    
 void Move(float h)
{
    if(!inputDisabled)
    {
    Grapple grapple = GetComponent<Grapple>();
    if (grapple != null && grapple.IsGrappling())
    {
        return;
    }
    
    // Instant acceleration by applying a large force
    if (h != 0)
    {
        float targetSpeed = h * moveSpeed;
        float speedDifference = targetSpeed - rig.linearVelocity.x;
        
        // Different acceleration based on ground/air
        float accelerationMultiplier = isGrounded ? 25f : 10f; // Slower in air
        
        // Only apply force if we're below max speed in that direction
        if (Mathf.Abs(rig.linearVelocity.x) < moveSpeed || Mathf.Sign(speedDifference) == Mathf.Sign(h))
        {
            rig.AddForce(new Vector2(speedDifference * accelerationMultiplier, 0));
        }
    }
    else if (isGrounded && (grapple == null || !grapple.RecentlyDetached()))
    {
        // Stop immediately when on ground with no input
        rig.linearVelocity = new Vector2(0, rig.linearVelocity.y);
    }
    
    // Flip sprite based on movement direction
    if (h < 0)
    {
        spr.flipX = true;
    }
    else if (h > 0)
    {
        spr.flipX = false;
    }
}
}
    
    void Jump()
    {
        rig.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        animator.SetBool("IsJumping", true);
        audio.Play("Jump");
    }
    
    Collider2D CheckIfGrounded()
{
    Collider2D colliders = Physics2D.OverlapCircle(isGroundChecker.position, checkGroundRadius, groundLayer);
    if (colliders != null)
    {
        if (!isGrounded)
        {
            animator.SetBool("IsJumping", false);
            
            // Check if we landed on a moving platform
            if (colliders.GetComponent<Rigidbody2D>() != null || colliders.GetComponent<SawAnimation>() != null)
            {
                currentPlatform = colliders.transform;
                lastPlatformPosition = currentPlatform.position;
            }
        }
        isGrounded = true;
    }
    else
    {
        if (isGrounded)
        {
            lastTimeGrounded = Time.time;
            currentPlatform = null; // Clear platform when leaving ground
        }
        isGrounded = false;
        
        if (rig.linearVelocity.y < 0)
        {
            animator.SetBool("IsJumping", true);
        }
    }
        return colliders;
}
    
    void ApplyFallSpeedLimit()
    {
        if (rig.linearVelocity.y < maxFallSpeed)
        {
            rig.linearVelocity = new Vector2(rig.linearVelocity.x, maxFallSpeed);
        }
    }
    void MoveWithPlatform()
{
    if (currentPlatform != null && isGrounded)
    {
        Vector3 platformMovement = currentPlatform.position - lastPlatformPosition;
        transform.position += platformMovement;
        lastPlatformPosition = currentPlatform.position;
    }
}
}