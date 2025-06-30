using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class CharacterData
    {
        public string Id;
        public bool IsUnlocked;
        public bool IsBuyed;

        public CharacterData(string id, bool isUnlocked)
        {
            Id = id;
            IsUnlocked = isUnlocked;
            IsBuyed = isUnlocked;
        }
    }
}
