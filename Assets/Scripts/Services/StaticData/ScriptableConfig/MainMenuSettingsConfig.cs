using UnityEngine;

namespace Assets.Scripts.Services.StaticData.ScriptableConfig
{
    [CreateAssetMenu(fileName = "MainMenuSettingsConfig", menuName = "Configs/Create new main menu settings config", order = 51)]
    public class MainMenuSettingsConfig : ScriptableObject
    {
        public uint RequiredTanksNumberToPriceUpdate;
        public uint StartPrice;
        public uint PriceIncrease;
        public uint RequiredTanksNumberToSpawnUpdate;
    }
}