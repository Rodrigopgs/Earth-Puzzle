using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public Vector2 OutsideForce { get { return incommingForce; } set { incommingForce = value; } }
    public Vector2 AdditiveForce { get { return addForce; } set { addForce = value; } }

    protected Rigidbody2D rb2d;

    [Header("Movement")]
    public float moveSpeed = 16f;
    [Tooltip("This should be much lower than the move speed")]
    public float maxSpeed = 16f;
    [Tooltip("These are the object layers the player can jump off of")]
    public LayerMask jumpMask;

    protected float moveDirection;

    [Header("Jumping")]
    public float jumpStrength = 8f;
    [Tooltip("How far the player needs to be from the ground to jump")]
    public float raycastDistance = 0.515f;
    [Min(1f)]
    [Tooltip("How much the movement speed should be divided by while in the air")]
    public float airMovementDivisor = 6f;
    [Tooltip("The rigid body drag while in the air")]
    public float airDrag = 0.6f;
    [Tooltip("The gravity scale of the rigid body while in the air")]
    public float airGravity = 1.5f;
    [Tooltip("How long in seconds should a jump be queued for if the jump button is pressed while in the air")]
    public float jumpQueueTime = 0.3f;
    [Tooltip("How long in seconds should the jump button work after falling off a ledge")]
    public float cyoteTime = 0.12f;

    protected bool onGround;
    protected bool jumpThisFrame;
    protected bool cyote;
    protected bool groundLastUpdate;
    protected bool jumping;

    protected float drag;
    protected float jumpQueueWaitTime;
    protected float cyoteWaitTime;
    protected float jumpWaitTime;
    protected float startingGravityScale;

    protected Vector3 startingScale;
    protected Vector2 incommingForce = Vector2.zero;
    protected Vector2 addForce = Vector2.zero;



    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        startingGravityScale = rb2d.gravityScale;
        drag = rb2d.drag;

        startingScale = transform.localScale;
    }

    public void OnMovement(InputAction.CallbackContext cb)
    {
        if (cb.performed)
        {
            moveDirection = cb.ReadValue<float>();

            if (moveDirection > 0)
                transform.localScale = new Vector3(startingScale.x, startingScale.y, startingScale.z);
            else
                transform.localScale = new Vector3(-startingScale.x, startingScale.y, startingScale.z);
        }
        else if (cb.canceled)
        {
            moveDirection = 0f;
        }
    }

    public void OnJump(InputAction.CallbackContext cb)
    {
        if (cb.ReadValue<float>() >= 0.5f)
        {
            jumpThisFrame = true;
            jumpQueueWaitTime = 0f;
        }
    }
    public void OnDown(InputAction.CallbackContext cb)
    {

    }

    void Update()
    {
        if (jumpWaitTime >= 0.2f)
        {
            jumping = false;
            jumpWaitTime = 0;
        }
        if (jumping)
            jumpWaitTime += Time.deltaTime;

        if (!jumping)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, jumpMask);
            if (hit.collider != null && !hit.collider.isTrigger)
            {
                onGround = true;
                cyote = false;
                jumping = false;

                cyoteWaitTime = 0f;
                rb2d.gravityScale = startingGravityScale;

                rb2d.drag = drag;
            }
            else
            {
                onGround = false;

                rb2d.drag = airDrag;
                rb2d.gravityScale = airGravity;
            }
        }

        //queue jump for short period if in air
        if (jumpQueueWaitTime >= jumpQueueTime)
        {
            jumpThisFrame = false;
            jumpQueueWaitTime = 0f;
        }
        if (jumpThisFrame)
            jumpQueueWaitTime += Time.deltaTime;

        //allow jumping if just fell off ground
        if (groundLastUpdate && !onGround && !jumping)
            cyote = true;
        if (cyoteWaitTime >= cyoteTime)
        {
            cyote = false;
            cyoteWaitTime = 0f;
        }
        if (cyote)
            cyoteWaitTime += Time.deltaTime;

        groundLastUpdate = onGround;
    }

    void FixedUpdate()
    {
        Vector2 forceVector = Vector2.zero;
        Vector2 absoluteVector = rb2d.velocity;

        if (onGround)
            forceVector.x += moveDirection * moveSpeed;
        else
            forceVector.x += moveDirection * moveSpeed / airMovementDivisor;

        if (jumpThisFrame && (onGround || cyote))
        {
            absoluteVector.y = jumpStrength;

            onGround = false;
            jumpThisFrame = false;
            cyote = false;
            jumping = true;

            jumpQueueWaitTime = 0f;
            cyoteWaitTime = 0f;

            rb2d.gravityScale = airGravity;
            rb2d.drag = airDrag;
        }

        if ((forceVector != Vector2.zero || absoluteVector != Vector2.zero) && incommingForce == Vector2.zero)
        {
            rb2d.AddForce(forceVector + addForce, ForceMode2D.Force);
            rb2d.velocity = new Vector2(Mathf.Clamp(rb2d.velocity.x, -maxSpeed, maxSpeed), absoluteVector.y);
            addForce = Vector2.zero;
        }
        else if (incommingForce != Vector2.zero)
        {
            rb2d.AddForce(incommingForce, ForceMode2D.Impulse);
            incommingForce = Vector2.zero;
        }
        else
        {
            rb2d.MovePosition((Vector2)transform.position + AdditiveForce);
            AdditiveForce = Vector2.zero;

        }
    }
}