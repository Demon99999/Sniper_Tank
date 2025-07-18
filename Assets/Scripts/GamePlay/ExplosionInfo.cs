using UnityEngine;

namespace Assets.Scripts.GamePlay
{
    public struct ExplosionInfo
    {
        public ExplosionInfo(Vector3 explosionPosition, uint explosionForce, bool isDamageableCollided, uint damage)
        {
            ExplosionPosition = explosionPosition;
            ExplosionForce = explosionForce;
            IsDamageableCollided = isDamageableCollided;
            Damage = damage;
        }

        public Vector3 ExplosionPosition { get; private set; }
        public uint ExplosionForce { get; private set; }
        public bool IsDamageableCollided { get; private set; }
        public uint Damage { get; private set; }
    }
}