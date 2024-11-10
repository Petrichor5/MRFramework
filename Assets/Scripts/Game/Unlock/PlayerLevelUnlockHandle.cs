using Config;

/// <summary>
/// 玩家等级达到X级
/// </summary>
public class PlayerLevelUnlockHandle : AUnlockHandle
{
    public override bool Execute(Config_Unlock_Unlock unlockInfo)
    {
        // 通过PlayerControl获取玩家的等级
        int palyerLevel = 1;
        return palyerLevel >= unlockInfo.ValueInt;
    }
}