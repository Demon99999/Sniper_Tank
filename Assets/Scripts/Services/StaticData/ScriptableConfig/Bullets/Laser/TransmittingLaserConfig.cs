using UnityEngine;

namespace Assets.Scripts.Services.StaticData.ScriptableConfig.Bullets.Laser
{
    [CreateAssetMenu(fileName = "TransmittingLaserConfig", menuName = "Configs/Bullets/Create new transmitting laser config", order = 51)]
    public class TransmittingLaserConfig : LaserConfig
    {
        public int TargetsCount;
    }
}