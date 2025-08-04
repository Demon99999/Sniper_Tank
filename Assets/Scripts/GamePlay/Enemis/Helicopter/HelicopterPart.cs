using System;
using UnityEngine;

namespace Assets.Scripts.GamePlay.Enemis.Helicopter
{
    public class HelicopterPart : MonoBehaviour, IDamageable
    {
        public event Action<ExplosionInfo> Damaged;
        
        public void TakeDamage(ExplosionInfo explosionInfo) =>
            Damaged?.Invoke(explosionInfo);
    }
}
