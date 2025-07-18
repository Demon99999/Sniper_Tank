using System;
using Assets.Scripts.GamePlay.Enemis.Points;
using Assets.Scripts.Infrastructure.Factoris.GamePlayFactory;
using Assets.Sources.Services.StaticDataService.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Services.StaticData.ScriptableConfig.Level
{
    [Serializable]
    public class PatrolingEnemyPointConfig : PathMovementEnemyPointConfig
    {
        public uint MaxRotationAngle;
        public float StoppingDuration;

        public PatrolingEnemyPointConfig(string id, Vector3 startPosition, Quaternion startRotation, EnemyType enemyType, EnemyPathPoint[] path)
            : base(id, startPosition, startRotation, enemyType, path)
        {
        }

        //public override async UniTask<Enemy> Create(IGameplayFactory gameplayFactory)
        //{
        //    Enemy enemy = await base.Create(gameplayFactory);

        //    EnemyPatroling enemyPatroling = enemy.gameObject.AddComponent<EnemyPatroling>();
        //    enemyPatroling.Initialize(Path, Speed, MaxRotationAngle);
        //    enemyPatroling.Initialize(StoppingDuration);

        //    return enemy;
        //}
    }
}