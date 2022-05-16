using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PushPullReciever : Interactable
{
    Arm arm;
    public Rigidbody2D rb2d;

    public bool stuck;

    bool playerOnSide;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public override void OnInteract(int playerNumber)
    {
        if (stuck)
        {
            arm.states.attatched = false;
            arm.attatched = this;
            stuck = false;
            rb2d.isKinematic = true;
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
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
        if (stuck)
        {
            rb2d.MovePosition((arm.transform.position - transform.position).normalized * Vector3.Distance(transform.position, arm.transform.position) + transform.position);
        }
    }

    private void Update()
    {
        Collider2D r = Physics2D.OverlapBox(transform.position + transform.right, Vector2.one * 0.5f, 0);
        Collider2D l = Physics2D.OverlapBox(transform.position + transform.right * -1, Vector2.one * 0.5f, 0);

        if ((r != null && r.CompareTag("Player2")) || (l != null && l.CompareTag("Player2")))
            playerOnSide = true;
        else
            playerOnSide = false;
    }
}