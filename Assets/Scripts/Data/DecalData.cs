using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class DecalData
    {
        public string Id;
        public bool IsUnlocked;

        public DecalData(string id, bool isUnlocked)
        {
            Id = id;
            IsUnlocked = isUnlocked;
        }
    }
}
