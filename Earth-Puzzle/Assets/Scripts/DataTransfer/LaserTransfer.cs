using UnityEngine;

public class LaserTransfer
{
    public static LaserTransfer Instance { get; private set; }

    // Laser
    public readonly Laser.LaserBehavior laserType;
    public readonly Gradient[] laserColors;

    // Laser UI
    public readonly bool[] unlocked;

    public LaserTransfer(Laser.LaserBehavior laserType, Gradient[] laserColors, LaserUI UI)
    {
        this.laserType = laserType;
        this.laserColors = laserColors;
        unlocked = new bool[3] { UI.UI.transform.Find("Activate").gameObject.activeSelf, UI.UI.transform.Find("Heat").gameObject.activeSelf, UI.UI.transform.Find("Stasis").gameObject.activeSelf };

        Instance = this;
    }
    public LaserTransfer(Laser laser)
    {
        laserType = laser.LaserType;
        laserColors = laser.laserColors;
        unlocked = new bool[3] { laser.UI.UI.transform.Find("Activate").gameObject.activeSelf, laser.UI.UI.transform.Find("Heat").gameObject.activeSelf, laser.UI.UI.transform.Find("Stasis").gameObject.activeSelf };

        Instance = this;
    }
}