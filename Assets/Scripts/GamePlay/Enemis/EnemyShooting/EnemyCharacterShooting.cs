using Assets.Scripts.GamePlay.Enemis.Animation;
using UnityEngine;

namespace Assets.Scripts.GamePlay.Enemis.EnemyShooting
{
    public class EnemyCharacterShooting : RotationEnemyShooting
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private EnemyAnimation _enemyAnimation;

        private void OnEnable() =>
            _enemyAnimation.BulletNeedetToCreate += CreateBullet;

        private void OnDisable() =>
            _enemyAnimation.BulletNeedetToCreate -= CreateBullet;

        protected override void Shoot() =>
            _animator.SetTrigger(AnimationPath.Shoot);

        protected override void StartShooting()
        {
            _animator.SetTrigger(AnimationPath.IsShooted);
            base.StartShooting();
        }
    }
}
