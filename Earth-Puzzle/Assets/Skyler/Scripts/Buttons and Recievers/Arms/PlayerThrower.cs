using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerThrower : Interactable
{

    public Vector2 throwForce = new Vector2(2, 3);

    Arm arm;
    Player1Controller controller;
    Rigidbody2D rb2d;

    private void Start()
    {
        controller = GetComponent<Player1Controller>();
        rb2d = GetComponent<Rigidbody2D>();

    }

    public override bool Conditions(GameObject from)
    {
        Debug.Log("conditions");

        Arm temp = from.GetComponent<Arm>();
        if (temp == null || !temp.throwPlayer)
            return false;
        else
        {
            arm = temp;
            arm.playerThrower = this;

            Debug.Log("conditions true");
            return true;
        }
    }

    public override void OnInteract(int playerNumber)
    {
        Debug.Log("interact");
        transform.position = arm.transform.position + arm.transform.up;
        transform.parent = arm.transform;
        arm.states.holding = true;
        rb2d.simulated = false;
        rb2d.isKinematic = true;
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        controller.held = true;

    }

    public void Throw()
    {
        Debug.Log("throw");
        transform.parent = null;
        arm.playerThrower = null;
        arm.states.holding = false;
        rb2d.isKinematic = false;
        rb2d.simulated = true;
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        controller.outsideForce = new Vector2(throwForce.x * (arm.transform.localScale.x < 0 ? -1 : 1), throwForce.y);
        controller.held = false;
    }
}
