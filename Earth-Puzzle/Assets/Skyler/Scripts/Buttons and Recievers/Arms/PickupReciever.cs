using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PickupReciever : Interactable
{

    Arm arm;

    Vector3 startScale;

    public override bool Conditions(GameObject from)
    {
        Arm temp = from.GetComponent<Arm>();
        if (temp == null || !temp.pickup)
            return false;
        else
        {
            arm = temp;
            return true;
        }
    }

    public override void OnInteract(int playerNumber)
    {
        Pickup();
    }

    public bool Place()
    {
        //RaycastHit2D hit = Physics2D.BoxCast(arm.transform.position + arm.transform.right, new Vector2(0.95f, 0.95f), 0, arm.transform.right);

        Vector2 newPos = Lattice2D<Vector2>.RoundToLatticeWorldPosition(arm.transform.position + arm.transform.right * (arm.transform.localScale.x < 0 ? -1 : 1));

        Collider2D[] hits = Physics2D.OverlapBoxAll(newPos, new Vector2(0.865f, 0.865f), 0);
        foreach (Collider2D hit in hits)
            if (!hit.isTrigger)
                return false;

        transform.position = newPos;
        gameObject.SetActive(true);

        arm.states.holding = false;
        Destroy(arm.holdingPreview);
        return true;
    }

    public void Pickup()
    {
        arm.holding = this;
        gameObject.SetActive(false);

        arm.states.holding = true;
        arm.holdingPreview = new GameObject();

        SpriteRenderer rend = arm.holdingPreview.AddComponent<SpriteRenderer>();
        rend.sprite = GetComponent<SpriteRenderer>().sprite;

        arm.holdingPreview.transform.localScale = startScale / 2f;
        arm.holdingPreview.transform.position = arm.transform.position + arm.transform.up / 2;

        arm.holdingPreview.transform.parent = arm.transform;

        Player1Interactions.Instance.UpdateInteractables();
        Player2Interactions.Instance.UpdateInteractables();
    }

    void Start()
    {
        startScale = transform.localScale;
    }
}
