using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class OldPlayerController : MonoBehaviour
{
    public Vector2 RespawnPosition { get; set; }

    public Vector2 outsideForce { get { return incommingForce; } set { incommingForce = value; } }
    public Vector2 additiveForce { get { return addForce; } set { addForce = value; } }

    public Rigidbody2D rb2d;

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

    protected abstract void Awake();

    protected abstract void OnEnable();

    protected abstract void Start();

    protected abstract void OnSide(InputAction.CallbackContext cb);

    protected abstract void CancelSide(InputAction.CallbackContext cb);
    protected abstract void OnJump(InputAction.CallbackContext cb);
    protected abstract void OnDrop(InputAction.CallbackContext cb);

    protected abstract void Update();

    protected abstract void FixedUpdate();
    protected abstract void OnDestroy();

    public void Kill()
    {
        Destroy(gameObject);
        new Checkpoint.Respawner(this);
    }
}