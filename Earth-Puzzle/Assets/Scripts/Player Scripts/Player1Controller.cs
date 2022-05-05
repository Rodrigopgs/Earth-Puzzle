using UnityEngine;
using UnityEngine.InputSystem;

public class Player1Controller : PlayerController
{
    TwoPlayerActions movementActions;
    TwoPlayerActions.Player1Actions player1Actions;
    InputAction side;
    InputAction jump;
    InputAction drop;

    protected override void Awake()
    {
        movementActions = new TwoPlayerActions();
        player1Actions = movementActions.Player1;
        side = player1Actions.Movement;
        jump = player1Actions.Jump;
        drop = player1Actions.Down;
    }

    protected override void OnEnable()
    {
        movementActions.Enable();
        player1Actions.Enable();
        side.Enable();
        jump.Enable();
        drop.Enable();

        side.performed += OnSide;
        side.canceled += CancelSide;
        jump.performed += OnJump;
        drop.performed += OnDrop;

    }

    protected override void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        startingGravityScale = rb2d.gravityScale;
        drag = rb2d.drag;

        startingScale = transform.localScale;
    }

    protected override void OnSide(InputAction.CallbackContext cb)
    {
        moveDirection = cb.ReadValue<float>();

        if (moveDirection > 0)
            transform.localScale = new Vector3(startingScale.x, startingScale.y, startingScale.z);
        else
            transform.localScale = new Vector3(-startingScale.x, startingScale.y, startingScale.z);
    }

    protected override void CancelSide(InputAction.CallbackContext cb)
    {
        moveDirection = 0f;
    }
    protected override void OnJump(InputAction.CallbackContext cb)
    {
        if (cb.ReadValue<float>() >= 0.5f)
        {
            jumpThisFrame = true;
            jumpQueueWaitTime = 0f;
        }
    }
    protected override void OnDrop(InputAction.CallbackContext cb)
    {

    }

    protected override void Update()
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

    protected override void FixedUpdate()
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
            rb2d.MovePosition((Vector2)transform.position + additiveForce);
            additiveForce = Vector2.zero;

        }
    }
    protected override void OnDestroy()
    {
        side.performed -= OnSide;
        side.canceled -= CancelSide;
        jump.performed -= OnJump;
        drop.performed -= OnDrop;
    }
}