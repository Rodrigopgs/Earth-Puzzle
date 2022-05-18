using UnityEngine;
using UnityEngine.InputSystem;

public class Player1Controller : OldPlayerController
{
    TwoPlayerActions movementActions;
    TwoPlayerActions.Player1Actions player1Actions;
    InputAction side;
    InputAction jump;
    InputAction drop;

    public AnimationClip walkAnim;
    public AnimationClip idleAnim;

    public bool held;
    Animator animator;

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

        startingScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));  //transform.localScale;

        animator = GetComponent<Animator>();

        RespawnPosition = transform.position;
    }

    protected override void OnSide(InputAction.CallbackContext cb)
    {
        moveDirection = cb.ReadValue<float>();

        if (held)
            return;

        if (walkAnim != null && idleAnim != null)
        {
            if (moveDirection != 0)
            {
                animator.Play(walkAnim.name);
            }
            else
            {
                animator.Play(idleAnim.name);
            }
        }

        if (moveDirection > 0)
            transform.localScale = new Vector3(startingScale.x, startingScale.y, startingScale.z);
        else
            transform.localScale = new Vector3(-startingScale.x, startingScale.y, startingScale.z);
    }

    protected override void CancelSide(InputAction.CallbackContext cb)
    {
        moveDirection = 0f;

        if (walkAnim != null && idleAnim != null)
        {
            animator.Play(idleAnim.name);
        }
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
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, raycastDistance, jumpMask);

            if (hits.Length <= 0)
            {
                onGround = false;

                rb2d.drag = airDrag;
                rb2d.gravityScale = airGravity;
                return;
            }

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && !hit.collider.isTrigger)
                {
                    onGround = true;
                    cyote = false;
                    jumping = false;

                    cyoteWaitTime = 0f;
                    rb2d.gravityScale = startingGravityScale;

                    rb2d.drag = drag;
                    return;
                }
                else
                {
                    onGround = false;

                    rb2d.drag = airDrag;
                    rb2d.gravityScale = airGravity;
                }
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
            cyote = false;
            jumping = true;

            jumpQueueWaitTime = 0f;
            cyoteWaitTime = 0f;

            rb2d.gravityScale = airGravity;
            rb2d.drag = airDrag;
        }

        if ((forceVector != Vector2.zero || jumpThisFrame || jumping || cyote) && incommingForce == Vector2.zero)
        {
            rb2d.AddForce(forceVector, ForceMode2D.Impulse);
            rb2d.velocity = new Vector2(Mathf.Clamp(rb2d.velocity.x, -maxSpeed, maxSpeed), absoluteVector.y);
        }
        else if (incommingForce != Vector2.zero)
        {
            rb2d.AddForce(incommingForce, ForceMode2D.Impulse);
            incommingForce = Vector2.zero;
        }
        else if (additiveForce != Vector2.zero && (!jumpThisFrame && !jumping && !cyote))
        {
            rb2d.MovePosition((Vector2)transform.position + additiveForce);
            additiveForce = Vector2.zero;
        }
        jumpThisFrame = false;
    }
    protected override void OnDestroy()
    {
        side.performed -= OnSide;
        side.canceled -= CancelSide;
        jump.performed -= OnJump;
        drop.performed -= OnDrop;
    }
}