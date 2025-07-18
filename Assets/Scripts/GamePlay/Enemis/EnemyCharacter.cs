using Assets.Scripts.GamePlay.Destructions;
using Assets.Scripts.GamePlay.Enemis.Animation;
using UnityEngine;

namespace Assets.Scripts.GamePlay.Enemis
{
    public class EnemyCharacter : Enemy, IDamageable
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private DestructionPart _weapon;
        [SerializeField] private Collider _weaponCollider;
        [SerializeField] private Ragdoll _ragdoll;

        public void TakeDamage(ExplosionInfo explosionInfo)
        {
            OnDestructed();

            _animator.enabled = false;

            _ragdoll.transform.parent = null;
            _ragdoll.Destruct(explosionInfo.ExplosionPosition, explosionInfo.ExplosionForce);

            _weaponCollider.enabled = true;
            _weapon.transform.parent = null;
            _weapon.Destruct(explosionInfo.ExplosionPosition, explosionInfo.ExplosionForce);

            Destroy(gameObject);
        }
    }
}
