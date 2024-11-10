using Config;

public abstract class AUnlockHandle
{
    public EUnlockType EUnlockType;

    public abstract bool Execute(Config_Unlock_Unlock unlockInfo);
}