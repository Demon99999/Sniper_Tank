using System.Linq;
using Assets.Scripts.Enemis.Points;
using Assets.Sources.Services.StaticDataService.Configs;
using UnityEngine;

namespace Assets.Scripts.Services.StaticData.ScriptableConfig.Level
{
    public abstract class PathMovementEnemyPointConfig : StaticEnemyPointConfig
    {
        public PathPointConfig[] Path;
        public float Speed;

        public PathMovementEnemyPointConfig(
            string id,
            Vector3 startPosition,
            Quaternion startRotation,
            EnemyType enemyType,
            EnemyPathPoint[] path)
            : base(id, startPosition, startRotation, enemyType)
        {
            Path = path.Select(value => new PathPointConfig(value.transform.position, value.RotationAngle, value.RotationDelta)).ToArray();
        }
    }
}