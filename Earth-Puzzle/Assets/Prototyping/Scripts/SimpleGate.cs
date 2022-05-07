using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SimpleGate : MonoBehaviour
{

    public Vector2 openOffset = new Vector2(0f, 1f);

    float lerpAmount;
    float distance;

    Vector2 offset;
    Vector2 start;

    public bool Open
    {
        set
        {
            if (!open && value)
                lerpAmount = Vector2.Distance(transform.position, start) / distance;
            else if (open && !value)
                lerpAmount = Vector2.Distance(transform.position, offset) / distance;
            open = value;
        }
        get => open;
    }

    bool open;

    private void Start()
    {
        offset = openOffset + (Vector2)transform.position;
        start = transform.position;
        distance = Vector2.Distance(start, offset);
    }

    private void Update()
    {
        lerpAmount = Mathf.Clamp01(lerpAmount + Time.deltaTime * 2f);

        if (open)
        {
            transform.position = Vector2.Lerp(start, offset, lerpAmount);
        }
        else
        {
            transform.position = Vector2.Lerp(offset, start, lerpAmount);
        }
    }
}