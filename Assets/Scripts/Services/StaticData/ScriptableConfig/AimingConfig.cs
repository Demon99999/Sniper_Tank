using UnityEngine;

namespace Assets.Scripts.Services.StaticData.ScriptableConfig
{
    [CreateAssetMenu(fileName = "AimingConfig", menuName = "Configs/Create new aiming config", order = 51)]
    public class AimingConfig : ScriptableObject
    {
        public float AimingDuration;
        public float ShootingAimDuration;
        public int TankTurretRotation;
        public float TankMovingDistanceModifier;
        public Vector2 MaxRotation;
        public Vector2 MinRotation;
        public Vector2 DroneStartRotation;
    }
}