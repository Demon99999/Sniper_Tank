using Assets.Scripts.GamePlay.Enemis;
using UnityEngine;

namespace Assets.Scripts.GamePlay.Destructions
{
    public class ExplosionBuilding : EnemyEngineryExplosion, IDamageable
    {
        [SerializeField] private DestructionedMaterialsRenderer _destructionedMaterialsRenderer;
        [SerializeField] private DestructionPart[] _destructionParts;

        private bool _isExplosded;

        private void Start() =>
            _isExplosded = false;

        public void TakeDamage(ExplosionInfo explosionInfo)
        {
            if (_isExplosded)
                return;

            _isExplosded = true;

            _destructionedMaterialsRenderer.Render();

            foreach (DestructionPart destructionPart in _destructionParts)
            {
                destructionPart.transform.parent = null;
                destructionPart.Destruct((explosionInfo.ExplosionPosition + transform.position) / 2, explosionInfo.ExplosionForce + ExplosionForce);
            }

            Explode();
        }
    }
}
