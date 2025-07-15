using System;
using UnityEngine;

namespace Assets.Scripts.Services.StaticData.ScriptableConfig.Level
{
    [Serializable]
    public class PathPointConfig
    {
        public Vector3 Position;
        public uint RotationAngle;
        public float RotationDelta;

        public PathPointConfig(Vector3 position, uint rotationAngle, float rotationDelta)
        {
            Position = position;
            RotationAngle = rotationAngle;
            RotationDelta = rotationDelta;
        }
    }
}