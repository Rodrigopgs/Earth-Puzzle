public class ArmTransfer
{
    public static ArmTransfer Instance { get; private set; }

    public readonly bool attatch;
    public readonly bool pickup;
    public readonly bool throwPlayer;

    public ArmTransfer(bool[] unlocks)
    {
        attatch = unlocks[0];
        pickup = unlocks[1];
        throwPlayer = unlocks[2];

        Instance = this;
    }

    public ArmTransfer(Arm arm)
    {
        attatch = arm.attatch;
        pickup = arm.pickup;
        throwPlayer = arm.throwPlayer;

        Instance = this;
    }

}
