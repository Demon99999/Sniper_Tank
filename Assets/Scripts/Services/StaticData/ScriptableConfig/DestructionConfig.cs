using UnityEngine;

namespace Assets.Scripts.Services.StaticData.ScriptableConfig
{
    [CreateAssetMenu(fileName = "DestructionConfig", menuName = "Configs/Create new destruction config", order = 51)]
    public class DestructionConfig : ScriptableObject
    {
        public Vector3 AdditionalDestructionDirection;
        public float RotationForce = 10;
        public float DestroyDelay;
        public float DestroyDuration;
        public Color DestructionColor;
        public int Layer;
    }
}