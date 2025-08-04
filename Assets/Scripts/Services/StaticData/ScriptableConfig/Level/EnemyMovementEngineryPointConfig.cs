using System;
using Assets.Scripts.GamePlay.Enemis;
using Assets.Scripts.GamePlay.Enemis.Movement;
using Assets.Scripts.GamePlay.Enemis.Points;
using Assets.Scripts.Infrastructure.Factoris.GamePlayFactory;
using Assets.Sources.Services.StaticDataService.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Services.StaticData.ScriptableConfig.Level
{
    [Serializable]
    public class EnemyMovementEngineryPointConfig : HelicopterPointConfig
    {
        public uint MaxRotationAngle;
        public float SpeedAfterAttack;

        public EnemyMovementEngineryPointConfig(string id, Vector3 startPosition,
            Quaternion startRotation, EnemyType enemyType, EnemyPathPoint[] path)
            : base(id, startPosition, startRotation, enemyType, path)
        {
        }

        public override async UniTask<Enemy> Create(IGameplayFactory gameplayFactory)
        {
            Enemy enemy = await gameplayFactory.CreateEnemy(EnemyType, StartPosition, StartRotation);

            EnemyEngineryMovement enemyEngineryMovement = enemy.gameObject.AddComponent<EnemyEngineryMovement>();
            enemyEngineryMovement.Initialize(Path, Speed, MaxRotationAngle);
            enemyEngineryMovement.Initialize(SpeedAfterAttack, IsWaitedAttack, IsPathLooped, WaitingTimeOnPoint);

            return enemy;
        }
    }
}