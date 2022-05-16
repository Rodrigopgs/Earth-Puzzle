using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ActivatedHazard : EventHazard
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OldPlayerController c = collision.GetComponent<OldPlayerController>();
        if (c != null && !Killed.Contains(c))
        {
            Kill(c);
        }
    }

    public enum ActivatedHazardType
    {
        Static,
        Sprite,
        Animation
    }
}
