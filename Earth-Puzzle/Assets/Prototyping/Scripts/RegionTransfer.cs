using UnityEngine;
using UnityEngine.Events;

using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class RegionTransfer : MonoBehaviour
{
    [Header("Simply drag the 'Main Camera' child of 'Camera System' into the first field of the event,\n" +
        "choose 'MultiplayerCameraBounds' from the dropdown, then choose 'int Region'.\n" +
        "Set the region to the camera's corners' element number. Additionally, create two\n" +
        "empty game objects, position them where you want the players to appear, then drag them into the\n" +
        "'transform' fields of the event.")]
    [Space]
    public UnityEvent onEnter;

    private readonly List<GameObject> players = new List<GameObject>();

    private void Update()
    {
        if (players.Count >= 2)
        {
            onEnter.Invoke();
            players.Clear();
        }
    }

    public void TransferPlayer1(Transform pos)
    {
        foreach (GameObject p in players)
            if (p.CompareTag("Player1"))
                p.transform.position = pos.position;
    }
    public void TransferPlayer2(Transform pos)
    {
        foreach (GameObject p in players)
            if (p.CompareTag("Player2"))
                p.transform.position = pos.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1") || collision.CompareTag("Player2"))
        {
            if (!players.Contains(collision.gameObject))
                players.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1") || collision.CompareTag("Player2"))
            players.Remove(collision.gameObject);

    }
}
