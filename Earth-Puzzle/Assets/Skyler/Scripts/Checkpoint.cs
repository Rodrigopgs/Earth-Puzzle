using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    [Header("Set this to true if you do not want to define a respawn position.\n" +
        "It will instead use the gameObject's transform position (using the pivot point, not the center).")]
    public bool useTransformPosition;
    [HideInInspector]
    public Vector2 respawnPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OldPlayerController old = collision.GetComponent<OldPlayerController>();
        if (old != null)
            old.RespawnPosition = transform.position;
    }

    public class Respawner
    {
        public Respawner(OldPlayerController controller)
        {
            GameObject toTrack = Instantiate(controller.gameObject, controller.RespawnPosition, Quaternion.identity);
            for (int i = 0; i < MultiplayerCameraBounds.Instance.objectsToTrack.Length; i++)
            {
                if (MultiplayerCameraBounds.Instance.objectsToTrack[i] == controller.transform)
                {
                    MultiplayerCameraBounds.Instance.objectsToTrack[i] = toTrack.transform;
                }
            }
        }
    }

}
