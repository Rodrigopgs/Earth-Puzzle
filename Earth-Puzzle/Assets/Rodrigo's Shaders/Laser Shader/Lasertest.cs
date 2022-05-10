using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class Lasertest : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public GameObject startVFX;
    public GameObject endVFX;
    private List<ParticleSystem> particles = new List<ParticleSystem>();

    ActivationReciever actRec;

    private void Start()
    {

        FillVFXlist();
        for (int i = 0; i < particles.Count; i++)
            particles[i].Stop();
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
        //particles position
        startVFX.transform.position = new Vector2(transform.position.x, transform.position.y);
        endVFX.transform.position = new Vector2(hitPos.x, hitPos.y);
        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        startVFX.transform.rotation = targetRotation;

    }

    public void OnClick(InputAction.CallbackContext cb)
    {

        Debug.Log("t");

        if (cb.started)
            lineRenderer.enabled = true;
        //particles on
        for (int i = 0; i < particles.Count; i++)
            particles[i].Play();

        if (cb.canceled)
        {
            lineRenderer.enabled = false;
            //particles off
            for (int i = 0; i < particles.Count; i++)
                particles[i].Clear();

            for (int i = 0; i < particles.Count; i++)
                particles[i].Stop();
   
            if (actRec != null)
                actRec.Deactivate();
        }
    }

    void Update()
    {
        if (actRec != null && lineRenderer.enabled)
            actRec.Activate();
    }
    //particles list to activate/deactivat all at once
    void FillVFXlist()
    {
        for (int i=0; i <startVFX.transform.childCount; i++)
        {
            var ps = startVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps != null)
            {
                particles.Add(ps);
            }
            
        }
        for (int i = 0; i < endVFX.transform.childCount; i++)
        {
            var ps = endVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps != null)
            {
                particles.Add(ps);
            }

        }
    }
}
