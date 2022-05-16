using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BoxHazard : Hazard
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OldPlayerController c = collision.GetComponent<OldPlayerController>();
        if (c != null && !Killed.Contains(c))
            Kill(c);
    }
}
