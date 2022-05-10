using UnityEngine;


[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class StasisReciever : MonoBehaviour
{

    Rigidbody2D rb2d;
    RigidbodyConstraints2D constraints;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        constraints = rb2d.constraints;
    }

    public virtual void Pause()
    {
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public virtual void Resume()
    {
        rb2d.constraints = constraints;
    }
}
