using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PushPullReciever : Interactable
{
    Arm arm;
    public Rigidbody2D rb2d;

    public LayerMask ignoreMask;

    public bool stuck;
    [Space]
    public float raycastDistance;

    Bounds spriteBounds;

    bool playerOnSide;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteBounds = GetComponent<SpriteRenderer>().bounds;
    }

    public override void OnInteract(int playerNumber)
    {
        if (stuck)
        {
            stuck = false;
            arm.states.attatched = false;
            arm.attatched = null;
        }
        else
        {
            arm.states.attatched = true;
            arm.attatched = this;
            stuck = true;
            rb2d.isKinematic = false;
            arm.direction = GetDirection();
            rb2d.constraints = RigidbodyConstraints2D.None;
        }

        Player1Interactions.Instance.UpdateInteractables();
        Player2Interactions.Instance.UpdateInteractables();
    }

    public override bool Conditions(GameObject from)
    {
        Arm temp = from.GetComponent<Arm>();
        if (temp == null || temp.states.HasState() || !temp.attatch || !playerOnSide)
            return false;

        arm = temp;

        return true;
    }

    private float GetDirection()
    {
        float t = arm.transform.position.x - transform.position.x;
        if (t < 0)
            return 1;
        else
            return -1;
    }

    private void FixedUpdate()
    {
        if (stuck && arm != null)
        {
            rb2d.MovePosition((arm.transform.position - transform.position).normalized * Vector3.Distance(transform.position, arm.transform.position) + transform.position);
        }
    }

    private void Update()
    {

        if (!stuck)
        {
            Debug.Log("t");

            RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, spriteBounds.extents, 0, Vector2.down, Mathf.Abs(raycastDistance * transform.lossyScale.y), ignoreMask);

            foreach (RaycastHit2D hit in hits)
                if (hit.collider == null || hit.collider.isTrigger)
                    continue;
                else
                {
                    Debug.Log("2");
                    rb2d.isKinematic = true;
                    rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
                    goto finish;
                }

            rb2d.isKinematic = false;
            rb2d.constraints = RigidbodyConstraints2D.None;
        }

    finish:
        Collider2D[] r = Physics2D.OverlapBoxAll(transform.position + (Vector3)Vector2.right, Vector2.one * 0.5f * transform.lossyScale, 0);
        Collider2D[] l = Physics2D.OverlapBoxAll(transform.position + (Vector3)Vector2.left, Vector2.one * 0.5f * transform.lossyScale, 0);

        foreach (Collider2D c in r)
        {
            if (c != null && c.CompareTag("Player2"))
            {
                playerOnSide = true;
                break;
            }
            else
                playerOnSide = false;
        }

        if (playerOnSide)
            return;

        foreach (Collider2D c in l)
        {
            if (c != null && c.CompareTag("Player2"))
            {
                playerOnSide = true;
                break;
            }
            else
                playerOnSide = false;
        }
    }
}