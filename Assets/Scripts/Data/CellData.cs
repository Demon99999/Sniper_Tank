using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class CellData
    {
        public string Id;
        public uint TankLevel;

        public CellData(string id)
        {
            Id = id;
            TankLevel = 0;
        }
    }
}
