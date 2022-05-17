using System.Collections.Generic;

using UnityEngine;

public abstract class Hazard : MonoBehaviour
{
    public List<OldPlayerController> Killed { get; set; } = new List<OldPlayerController>();

    public bool killAllPlayers;

    public virtual void Kill(OldPlayerController player)
    {
        if (!killAllPlayers)
        {
            Killed.Add(player);
            player.Kill(this);
            return;
        }

        OldPlayerController p1 = Player1Interactions.Instance.GetComponent<OldPlayerController>();
        OldPlayerController p2 = Player2Interactions.Instance.GetComponent<OldPlayerController>();


        Killed.Add(p1);
        Killed.Add(p2);

        p1.Kill(this);
        p2.Kill(this);
    }

}
