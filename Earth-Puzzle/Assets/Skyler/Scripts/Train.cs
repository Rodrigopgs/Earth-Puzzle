using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Train : MonoBehaviour
{

    public float moveSpeed = 0.5f;

    private bool moveRight;
    private bool moveLeft;

    private Rigidbody2D rb2d;

    public bool MoveLeft
    {
        get => moveLeft;
        set
        {
            moveLeft = value;
            moveRight = false;
        }
    }

    public bool MoveRight
    {
        get => moveRight;
        set
        {
            moveLeft = false;
            moveRight = value;
        }
    }

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (moveLeft)
        {
            rb2d.MovePosition(transform.position + (Vector3)Vector2.left * Time.deltaTime * moveSpeed);
        }
        else if (moveRight)
        {
            rb2d.MovePosition(transform.position + (Vector3)Vector2.right * Time.deltaTime * moveSpeed);
        }
    }

}
