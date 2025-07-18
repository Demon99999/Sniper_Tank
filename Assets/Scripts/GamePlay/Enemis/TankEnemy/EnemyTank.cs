using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GamePlay.Enemis.TankEnemy
{
    public class EnemyTank : EnemyEnginery
    {
        private uint numberModifier = 3;

        [SerializeField] private EnemyTankPart[] _parts;

        protected override uint CalculateDamga(ExplosionInfo explosionInfo)
        {
            if (explosionInfo.IsDamageableCollided)
            {
                EnemyTankPart enemyPart = _parts.OrderBy(part => part.GetDistanceTo(explosionInfo.ExplosionPosition)).First();

                return (uint)(explosionInfo.Damage * enemyPart.DamageModifier);
            }
            else
            {
                return explosionInfo.Damage / numberModifier;
            }
        }
    }
}