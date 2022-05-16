using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{

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
            Instantiate(controller, controller.RespawnPosition, Quaternion.identity);
        }
    }

}
