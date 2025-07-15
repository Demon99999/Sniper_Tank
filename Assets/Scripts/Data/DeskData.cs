using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class DeskData
    {
        public List<CellData> Cells;

        public DeskData()
        {
            Cells = new List<CellData>();
        }

        public uint GetCellInfo(string id)
        {
            CellData data = Cells.FirstOrDefault(cell => cell.Id == id);

            if (data == null)
            {
                data = new CellData(id);
                Cells.Add(data);
            }

            return data.TankLevel;
        }

        public void UpdateCellInfo(string id, uint tankLevel)
        {
            CellData data = Cells.FirstOrDefault(cell => cell.Id == id);
            data.TankLevel = tankLevel;
        }
    }
}
