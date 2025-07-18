using System;
using Assets.Scripts.GamePlay.Enemis;
using Assets.Scripts.Infrastructure.Factoris.GamePlayFactory;
using Assets.Sources.Services.StaticDataService.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Services.StaticData.ScriptableConfig.Level
{
    [Serializable]
    public class StaticEnemyPointConfig
    {
        public string Id;
        public Vector3 StartPosition;
        public Quaternion StartRotation;
        public EnemyType EnemyType;

        public StaticEnemyPointConfig(string id, Vector3 startPosition, Quaternion startRotation, EnemyType enemyType)
        {
            Id = id;
            StartPosition = startPosition;
            StartRotation = startRotation;
            EnemyType = enemyType;
        }

        public virtual async UniTask<Enemy> Create(IGameplayFactory gameplayFactory)
        {
            return await gameplayFactory.CreateEnemy(EnemyType, StartPosition, StartRotation);
        }
    }
}