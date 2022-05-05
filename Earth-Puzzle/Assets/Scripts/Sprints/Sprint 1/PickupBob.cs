using UnityEngine;
using UnityEngine.Events;

public class PickupBob : MonoBehaviour
{
    [Tooltip("How far up and down the bob will go")]
    public float bobAmount = 0.1f;
    [Tooltip("How fast the scrap will bob")]
    public float bobSpeed = 2f;
    [Space, Tooltip("The method that will be triggered when this object is touched")]
    public UnityEvent pickupEvent;

    Vector3 startPos;

    [Header("Debug")]
    public float radius = 0.12f;
    public Vector2 offset = Vector2.zero;

    void Start()
    {
        startPos = transform.position;
    }

#if DEBUG
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (Application.isPlaying)
        {
            Gizmos.DrawWireSphere(new Vector3(startPos.x + offset.x, startPos.y - bobAmount + offset.y, startPos.z), radius);
            Gizmos.DrawWireSphere(new Vector3(startPos.x + offset.x, startPos.y + bobAmount + offset.y, startPos.z), radius);
        }
        else
        {
            Gizmos.DrawWireSphere(new Vector3(transform.position.x + offset.x, transform.position.y - bobAmount + offset.y, transform.position.z), radius);
            Gizmos.DrawWireSphere(new Vector3(transform.position.x + offset.x, transform.position.y + bobAmount + offset.y, transform.position.z), radius);
        }
    }
#endif

    void Update()
    {
        transform.position = new Vector3(startPos.x, Mathf.Sin(Time.time * bobSpeed) * bobAmount + startPos.y, startPos.z);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            pickupEvent.Invoke();
    }

}