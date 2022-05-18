using System.Collections.Generic;

using UnityEngine;

using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(Collider2D))]
public class ActivatedMovingPlatform : MonoBehaviour
{
    public bool Move
    {
        get => direction;
        set
        {
            float totalDistance = Vector2.Distance(start, end);
            direction = value;
            if (value)
                moveAmount = 1f - Vector2.Distance(transform.position, end) / totalDistance;
            else
                moveAmount = 1f - Vector2.Distance(transform.position, start) / totalDistance;
        }
    }

    public Vector2 start;
    public Vector2 end;

    public float moveSpeed = 2;
    public bool direction;

    float moveAmount;

    List<OldPlayerController> players = new List<OldPlayerController>();

    Vector2 velocity;
    Vector2 lastPos;

    private void Start()
    {
        lastPos = transform.position;

        float totalDistance = Vector2.Distance(start, end);
        if (direction)
            moveAmount = 1f - Vector2.Distance(transform.position, end) / totalDistance;
        else
            moveAmount = 1f - Vector2.Distance(transform.position, start) / totalDistance;
    }

    private void Update()
    {
        moveAmount = Mathf.Clamp01(moveAmount + Time.deltaTime * moveSpeed);

        if (direction)
        {
            transform.position = Vector2.Lerp(start, end, moveAmount);
        }
        else
        {
            transform.position = Vector2.Lerp(end, start, moveAmount);
        }

        velocity += (Vector2)transform.position - lastPos;
        lastPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (velocity == Vector2.zero)
            return;

        if (players.Count > 0)
            for (int i = 0; i < players.Count; i++)
            {
                players[i].additiveForce = velocity * 1.5f;
            }

        velocity = Vector2.zero;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        OldPlayerController controller = collision.GetComponent<OldPlayerController>();

        if (controller != null)
            players.Add(controller);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OldPlayerController controller = collision.GetComponent<OldPlayerController>();

        if (controller != null && players.Contains(controller))
            players.Remove(controller);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(start, end);

        Gizmos.DrawCube(start, new Vector2(0.25f, 0.25f));
        Gizmos.DrawCube(end, new Vector2(0.25f, 0.25f));
    }
#endif
}
