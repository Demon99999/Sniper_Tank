using System;
using System.Linq;
using Assets.Scripts.Enum;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public TankData[] Tanks;
        public TankSkinData[] TankSkins;
        public DecalData[] Decals;
        public BiomeType CurrentBiomeType;
        public CharacterData[] PlayerCharacters;
        public Wallet Wallet;
        public TankBuyingData TankBuyingData;
        public DeskData DeskData;

        public uint SelectedTankLevel;
        public uint CurrentLevelIndex;
        public uint CompletedLevelsCount;
        public string SelectedPlayerCharacterId;
        public bool IsSoundOn;

        public PlayerProgress(
            TankData[] tanks,
            TankSkinData[] skins,
            DecalData[] decals,
            BiomeType startLevelType,
            CharacterData[] characterSkins,
            uint startTankBuyingCost)
        {
            Tanks = tanks;
            TankSkins = skins;
            Decals = decals;
            CurrentBiomeType = startLevelType;
            PlayerCharacters = characterSkins;

            SelectedTankLevel = Tanks.First(tank => tank.IsUnlocked).Level;
            CurrentLevelIndex = 0;
            SelectedPlayerCharacterId = PlayerCharacters.First(skin => skin.IsUnlocked).Id;
            Wallet = new Wallet();
            TankBuyingData = new TankBuyingData(startTankBuyingCost);
            CompletedLevelsCount = 0;
            DeskData = new DeskData();
            IsSoundOn = true;
        }

        public event Action<uint> TankUnlocked;
        public event Action<uint> SelectedTankChanged;
        public event Action<string> TankSkinUnlocked;
        public event Action<string> DecalUnlocked;
        public event Action<string> DecalChanged;
        public event Action<string> CharacterSkinBuyed;
        public event Action<string> CharacterSkinChanged;

        public void TryUnlockTank(uint level)
        {
            TankData tank = Tanks.First(t => t.Level == level);

            if (tank.IsUnlocked == false)
            {
                tank.IsUnlocked = true;
                TankUnlocked?.Invoke(level);
                TrySelectTank(level);
            }
        }

        public void TrySelectTank(uint level)
        {
            if (GetTank(level).IsUnlocked)
            {
                SelectedTankLevel = level;
                SelectedTankChanged?.Invoke(SelectedTankLevel);
            }
        }

        public TankData GetTank(uint level)
        {
            if (Tanks.Any(tank => tank.Level == level) == false)
            {
                Debug.LogError("Tank of this level not found");
                return null;
            }

            return Tanks.First(tank => tank.Level == level);
        }

        public TankData GetSelectedTank()
        {
            return GetTank(SelectedTankLevel);
        }

        public void UnlockTankSkin(string id)
        {
            GetSkin(id).IsUnlocked = true;
            TankSkinUnlocked?.Invoke(id);

            SelectTankSkin(id);
        }

        public TankSkinData GetSkin(string id)
        {
            return TankSkins.First(skin => skin.Id == id);
        }

        public void SelectTankSkin(string id)
        {
            GetTank(SelectedTankLevel).SkinId = id;
            SelectedTankChanged?.Invoke(SelectedTankLevel);
        }

        public DecalData GetDecal(string id)
        {
            return Decals.First(decal => decal.Id == id);
        }

        public void UnlockDecal(string id)
        {
            GetDecal(id).IsUnlocked = true;
            DecalUnlocked?.Invoke(id);

            SelectDecal(id);
        }

        public void SelectDecal(string id)
        {
            GetTank(SelectedTankLevel).DecalId = id;
            DecalChanged?.Invoke(id);
        }

        public CharacterData GetPlayerCharacter(string id)
        {
            return PlayerCharacters.First(skin => skin.Id == id);
        }

        public void UnlockCharacterSkin(string id)
        {
            GetPlayerCharacter(id).IsUnlocked = true;
        }

        public void BuyCharacterSkin(string id)
        {
            GetPlayerCharacter(id).IsBuyed = true;

            CharacterSkinBuyed?.Invoke(id);

            SelectCharacterSkin(id);
        }

        public void SelectCharacterSkin(string id)
        {
            SelectedPlayerCharacterId = id;
            CharacterSkinChanged?.Invoke(id);
        }
    }
}