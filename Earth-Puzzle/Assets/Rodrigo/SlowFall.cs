using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowFall : MonoBehaviour
{
    public Player1Controller player;
    public Player2Controller player2;
    public MultiplayerCameraBounds camdistance;
    public Camera camsize;
    public SimpleGate floorgate;

   
    public GameObject floor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        player = (Player1Controller)Player1Interactions.Instance.GetComponent<OldPlayerController>();
        player2 = (Player2Controller)Player2Interactions.Instance.GetComponent<OldPlayerController>();

        if (floorgate.Open == true)
        {
            camdistance.minZoom = 8;
            camsize.orthographicSize = 8;
            player.airGravity =0.2f;
            player.airMovementDivisor = 0.1f;
            player.moveSpeed = 400;
            player2.airGravity = 0.2f;
            player2.airMovementDivisor = 0.1f;
            player2.moveSpeed = 400;

        }
    }
}
