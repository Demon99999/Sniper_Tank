using Assets.Scripts.GamePlay.Bullets;
using UnityEngine;

namespace Assets.Scripts.GamePlay.Enemis.Robot
{
    public class RobotLaser : MonoBehaviour
    {
        private const float Size = 0.5f;

        [SerializeField] private LaserLine[] _lasers;

        public void SetLaser(Vector3 startPosition, Vector3 endPosition)
        {
            foreach (LaserLine laser in _lasers)
            {
                laser.Initialize(laser.transform.position, endPosition, Size);
                laser.SetActive(true);
            }
        }
    }
}
