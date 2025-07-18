using Assets.Scripts.GamePlay.Enemis.Movement;
using UnityEngine;

namespace Assets.Scripts.GamePlay.Enemis.EnemyShooting
{
    public class PatrolingEnemyShooting : EnemyCharacterShooting
    {
        [SerializeField] private EnemyPatroling _patroling;

        protected override void StartShooting()
        {
        }

        public void CanShooting() =>
            base.StartShooting();
    }
}
