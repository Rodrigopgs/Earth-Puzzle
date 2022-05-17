using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class TriggerEvent : MonoBehaviour
{
    public UnityEvent triggerEvent;

    public bool requirePlayer;
    public bool aSynchronous;
    public bool requireBothPlayers;

    int players;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger)
            return;

        if (requirePlayer || requireBothPlayers)
        {
            OldPlayerController c = collision.GetComponent<OldPlayerController>();

            if (c != null)
            {
                if (requireBothPlayers)
                {
                    players++;

                    if (players >= 2)
                        triggerEvent.Invoke();
                }
                else
                {
                    triggerEvent.Invoke();
                }
            }
        }
        else
        {
            triggerEvent.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.isTrigger)
            return;

        if (requireBothPlayers)
        {
            OldPlayerController c = collision.GetComponent<OldPlayerController>();
            if (c != null && !aSynchronous)
                players--;
        }
    }
}
