using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    public LineRenderer lineRenderer;

    ActivationReciever actRec;

    private void Start()
    {
        lineRenderer.enabled = false;
    }

    public void OnMouseChange(InputAction.CallbackContext cb)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(cb.ReadValue<Vector2>());
        Vector2 direction = (mousePos - (Vector2)transform.position).normalized;
        Vector2 hitPos;

        RaycastHit2D rh2d = Physics2D.Raycast(transform.position, direction, 50, ~(1 << LayerMask.NameToLayer("Ignore Laser")));
        if (rh2d.collider != null)
        {
            hitPos = rh2d.point;
            if (rh2d.collider.CompareTag("Activation Reciever"))
                actRec = rh2d.collider.GetComponent<ActivationReciever>();
            else
            {
                if (actRec != null)
                    actRec.Deactivate();
                actRec = null;
            }
        }
        else
        {
            if (actRec != null)
                actRec.Deactivate();
            actRec = null;

            hitPos = (Vector2)transform.position + direction * 50;
        }

        Vector3[] linePoss = new Vector3[] { new Vector3(transform.position.x, transform.position.y), new Vector3(hitPos.x, hitPos.y) };

        lineRenderer.SetPositions(linePoss);
    }

    public void OnClick(InputAction.CallbackContext cb)
    {

        Debug.Log("t");

        if (cb.started)
            lineRenderer.enabled = true;
        if (cb.canceled)
        {
            lineRenderer.enabled = false;
            if (actRec != null)
                actRec.Deactivate();
        }
    }

    void Update()
    {
        if (actRec != null && lineRenderer.enabled)
            actRec.Activate();
    }
}
