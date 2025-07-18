using UnityEngine;

namespace Assets.Scripts.GamePlay.Enemis
{
    public class EnemyCar : EnemyEnginery
    {
        private uint _numberTwo = 2;

        [SerializeField] private DestructedEnemy _attackedEnemy;

        protected override uint CalculateDamga(ExplosionInfo explosionInfo)
        {
            return explosionInfo.IsDamageableCollided ? explosionInfo.Damage : explosionInfo.Damage / _numberTwo;
        }

        protected override void Destruct(ExplosionInfo explosionInfo)
        {
            base.Destruct(explosionInfo);

            _attackedEnemy.Destruct(
                    (explosionInfo.ExplosionPosition + transform.position) / 2,
                    explosionInfo.ExplosionForce + EnemyEngineryExplosion.ExplosionForce);
        }
    }
}
