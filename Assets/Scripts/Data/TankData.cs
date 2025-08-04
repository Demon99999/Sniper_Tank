using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class TankData
    {
        public uint Level;
        public bool IsUnlocked;
        public string SkinId;
        public string DecalId;
        public bool IsFirstAppearance;

        public TankData(uint level, bool isUnlocked, string decalId)
        {
            Level = level;
            IsUnlocked = isUnlocked;
            DecalId = decalId;

            SkinId = string.Empty;
            IsFirstAppearance = true;
        }
    }
}
