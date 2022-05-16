using System.Collections.Generic;

using UnityEngine;

public abstract class Hazard : MonoBehaviour
{
    public List<OldPlayerController> Killed { get; set; } = new List<OldPlayerController>();

    public virtual void Kill(OldPlayerController player)
    {
        player.Kill(this);
        Killed.Add(player);
    }

}
