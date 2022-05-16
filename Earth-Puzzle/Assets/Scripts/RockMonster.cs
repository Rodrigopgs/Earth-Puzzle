using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMonster : MonoBehaviour
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
        if (other.gameObject.tag != "Player1" && other.gameObject.tag != "Player2" && other.gameObject.tag != "RockMonster")
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
    }


}
