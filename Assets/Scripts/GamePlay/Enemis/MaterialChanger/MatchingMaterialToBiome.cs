using System;
using Assets.Scripts.Enum;

namespace Assets.Scripts.GamePlay.Enemis.MaterialChanger
{
    [Serializable]
    public class MatchingMaterialToBiome
    {
        public BiomeType BiomeType;
        public MaterialInfo[] MaterialInfos;
    }
}
