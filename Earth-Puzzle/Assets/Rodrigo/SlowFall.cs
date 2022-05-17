using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowFall : MonoBehaviour
{
    public Player1Controller player;
    public Player2Controller player2;
    public MultiplayerCameraBounds camdistance;

   
    public GameObject floor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (floor.transform.position.x != 14.5)
        {
            camdistance.minZoom = 8; 
            player.airDrag = 5;
            player2.airDrag = 5;

        }
    }
}
