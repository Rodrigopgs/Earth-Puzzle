using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    [SerializeField]
    private LaserBehavior laserType;

    public Vector2 startOffset;
    public LaserBehavior LaserType
    {
        get => laserType;
        set
        {
            switch (value)
            {
                case LaserBehavior.Activation:
                    lineRenderer.colorGradient = laserColors[0];
                    break;
                case LaserBehavior.Destruction:
                    lineRenderer.colorGradient = laserColors[1];
                    break;
                case LaserBehavior.Stasis:
                    lineRenderer.colorGradient = laserColors[2];
                    break;
            }
            laserType = value;
        }
    }

    public Gradient[] laserColors;
    public LineRenderer lineRenderer;

    private Vector2 mousePos;

    //Activation Laser

    ActivationReciever actRec;

    //Destruction Laser

    DestructionReciever decRec;

    //Stasis Laser

    StasisReciever stsRec;

    //UI
    public LaserUI UI;

    private void Start()
    {
        if (LaserTransfer.Instance == null)
            new LaserTransfer(this);
        else
            LoadValues();

        lineRenderer.enabled = false;

        switch (laserType)
        {
            case LaserBehavior.Activation:
                lineRenderer.colorGradient = laserColors[0];
                break;
            case LaserBehavior.Destruction:
                lineRenderer.colorGradient = laserColors[1];
                break;
            case LaserBehavior.Stasis:
                lineRenderer.colorGradient = laserColors[2];
                break;
        }
    }

    public void OnMouseChange(InputAction.CallbackContext cb) => mousePos = cb.ReadValue<Vector2>();

    private void ActiveLaser(RaycastHit2D rh2d)
    {
        var temp = rh2d.collider.GetComponent<ActivationReciever>();

        if (actRec != null && actRec != temp)
            actRec.Deactivate();

        actRec = temp;
        if (actRec != null && lineRenderer.enabled)
            actRec.Activate();

        if (decRec != null)
            decRec.Heal();
        decRec = null;
        if (stsRec != null)
            stsRec.Resume();
        stsRec = null;
    }

    private void DestroyLaser(RaycastHit2D rh2d)
    {
        var temp = rh2d.collider.GetComponent<DestructionReciever>();

        if (decRec != null && decRec != temp)
            decRec.Heal();

        decRec = temp;
        if (decRec != null && lineRenderer.enabled)
            decRec.Destroy();

        if (actRec != null)
            actRec.Deactivate();
        actRec = null;
        if (stsRec != null)
            stsRec.Resume();
        stsRec = null;
    }

    private void StasisLaser(RaycastHit2D rh2d)
    {
        var temp = rh2d.collider.GetComponent<StasisReciever>();

        if (stsRec != null && stsRec != temp)
            stsRec.Resume();

        stsRec = temp;
        if (stsRec != null && lineRenderer.enabled)
            stsRec.Pause();

        if (actRec != null)
            actRec.Deactivate();
        actRec = null;
        if (decRec != null)
            decRec.Heal();
        decRec = null;
    }

    public void OnClick(InputAction.CallbackContext cb)
    {
        if (cb.started)
        {
            lineRenderer.enabled = true;

            switch (laserType)
            {
                case LaserBehavior.Activation:
                    if (actRec != null)
                        actRec.Activate();
                    break;
                case LaserBehavior.Destruction:
                    if (decRec != null)
                        decRec.Destroy();
                    break;
                case LaserBehavior.Stasis:
                    if (stsRec != null)
                        stsRec.Pause();
                    break;
            }

        }
        if (cb.canceled)
        {
            lineRenderer.enabled = false;

            switch (laserType)
            {
                case LaserBehavior.Activation:
                    if (actRec != null)
                        actRec.Deactivate();
                    break;
                case LaserBehavior.Destruction:
                    if (decRec != null)
                        decRec.Heal();
                    break;
                case LaserBehavior.Stasis:
                    if (stsRec != null)
                        stsRec.Resume();
                    break;
            }

        }
    }

    public void OnTab(InputAction.CallbackContext cb)
    {
        if (cb.started)
            UI.EnableUI();
        else if (cb.canceled)
            UI.DisableUI();
    }

    private void ResetAll()
    {
        if (actRec != null)
            actRec.Deactivate();
        actRec = null;

        if (decRec != null)
            decRec.Heal();
        decRec = null;

        if (stsRec != null)
            stsRec.Resume();
        stsRec = null;
    }

    private void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        Vector2 direction = (Camera.main.ScreenToWorldPoint(mousePos) - transform.position);
        direction.Normalize();
        Vector2 hitPos;

        RaycastHit2D rh2d = Physics2D.Raycast(transform.position + (Vector3)startOffset, direction, 50, ~(1 << LayerMask.NameToLayer("Ignore Laser")));
        if (rh2d.collider != null)
        {
            hitPos = rh2d.point;

            if (rh2d.collider.CompareTag("Laser Reciever"))
            {
                switch (laserType)
                {
                    case LaserBehavior.Activation:
                        ActiveLaser(rh2d);
                        break;
                    case LaserBehavior.Destruction:
                        DestroyLaser(rh2d);
                        break;
                    case LaserBehavior.Stasis:
                        StasisLaser(rh2d);
                        break;
                }
            }
            else
            {
                ResetAll();
            }
        }
        else
        {
            ResetAll();

            hitPos = (Vector2)transform.position + direction * 50;
        }

        Vector3[] linePoss = new Vector3[] { new Vector3(transform.position.x, transform.position.y) + (Vector3)startOffset, new Vector3(hitPos.x, hitPos.y) };

        lineRenderer.SetPositions(linePoss);
    }

    public void Unlock(int laser)
    {
        switch (laser)
        {
            case 0:
                UI.Unlock(UI.UI.transform.Find("Activate").gameObject);
                break;
            case 1:
                UI.Unlock(UI.UI.transform.Find("Heat").gameObject);
                break;
            case 2:
                UI.Unlock(UI.UI.transform.Find("Stasis").gameObject);
                break;
        }
    }

    public void SwitchLaser(int laser) => LaserType = (LaserBehavior)laser;

    public enum LaserBehavior
    {
        Activation,
        Destruction,
        Stasis
    }

    public void LoadValues()
    {
        LaserType = LaserTransfer.Instance.laserType;
        laserColors = LaserTransfer.Instance.laserColors;

        for (int i = 0; i < LaserTransfer.Instance.unlocked.Length; i++)
        {
            if (LaserTransfer.Instance.unlocked[i])
                Unlock(i);
        }
    }

    private void OnDestroy()
    {
        new LaserTransfer(this);
    }

}

[System.Serializable]
public class LaserUI
{
    public GameObject UI;

    public void DisableUI() => UI.SetActive(false);
    public void EnableUI() => UI.SetActive(true);

    public void Unlock(GameObject locked)
    {
        locked.SetActive(true);
    }
}
