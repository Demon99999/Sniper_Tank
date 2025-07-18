using System;
using UnityEngine;

namespace Assets.Scripts.GamePlay.Enemis.Animation
{
    public class EnemyAnimation : MonoBehaviour
    {
        public event Action BulletNeedetToCreate;

        public void CreateBullet()
        {
            BulletNeedetToCreate?.Invoke();
        }
    }
}
