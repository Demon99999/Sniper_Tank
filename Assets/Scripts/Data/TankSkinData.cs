using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class TankSkinData
    {
        public string Id;
        public bool IsUnlocked;

        public TankSkinData(string id)
        {
            Id = id;
            IsUnlocked = false;
        }
    }
}
