using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMonster : Hazard
{
    Transform self;
    bool walkright;
    public float movespeed;
    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<Transform>();
        walkright = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    private void FixedUpdate()
    {
        if (walkright == true)
        {
            self.transform.position += new Vector3(1, 0, 0) * movespeed * Time.deltaTime;
        }
        if (walkright == false)
        {
            self.transform.position -= new Vector3(1, 0, 0) * movespeed * Time.deltaTime;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        OldPlayerController playercontroller = other.GetComponent<OldPlayerController>();

        if (other.gameObject.tag != "RockMonster")
        {

            if (walkright == true)
            {
                walkright = false;
            }
            else if (walkright == false)
            {
                walkright = true;
            }

        }
        if (playercontroller != null && !Killed.Contains(playercontroller) && !other.isTrigger)
        {
            Kill(playercontroller);
        }
        
    }


}
