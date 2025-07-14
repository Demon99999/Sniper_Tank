using UnityEngine;

namespace Assets.Scripts.Enemis.Points
{
    public class MovementEnemyEngineryPoint : PatrolingEnemyPoint
    {
        protected override Vector3 GetEnemySize()
        {
            return StartPoint.rotation * new Vector3(2, 2, 3);
        }
    }
}