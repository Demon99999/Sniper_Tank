using UnityEngine;

namespace Assets.Scripts.GamePlay.Enemis.EnemyShooting
{
    public class EnemyTankShooting : RotationEnemyShooting
    {
        [SerializeField] private Transform _turret;
        [SerializeField] private uint _turretRotationSpeed;

        private bool _isTurretTurnedToTarget;

        protected override bool CanShoot => base.CanShoot && _isTurretTurnedToTarget;

        protected override void StartShooting()
        {
            base.StartShooting();
            StartCoroutine(RotateTowardsTarget(PlayerWrapper.transform, _turret, _turretRotationSpeed,
                () => _isTurretTurnedToTarget = true,
                () => _isTurretTurnedToTarget = false));
        }

        protected override Quaternion GetTargetRotation(Vector3 targetDirection, Transform rotatingPart)
        {
            return Quaternion.LookRotation(targetDirection, Vector3.right);
        }
    }
}
