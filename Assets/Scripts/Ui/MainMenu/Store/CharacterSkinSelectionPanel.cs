using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Factoris.UI;
using Assets.Scripts.Services.AssetManagementServices;
using Assets.Scripts.Services.PersistentProgressServices;
using Assets.Scripts.Services.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YG;
using Zenject;

namespace Assets.Scripts.Ui.MainMenu.Store
{
    public class CharacterSkinSelectionPanel : SelectionPanel<string>
    {
        private const int RewardID = 4;

        [SerializeField] private int _tankRotationAngle;
        [SerializeField] private UiSelectedTankPoint _tankPoint;

        private IStaticDataService _staticDataService;
        private IAssetProvider _assetProvider;

        [Inject]
        private void Construct(IStaticDataService staticDataService, IAssetProvider assetProvider)
        {
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
        }

        public override void Open()
        {
            base.Open();
            _tankPoint.SetTargetRotation(_tankRotationAngle);
        }

        public override void Hide()
        {
            base.Hide();
            _tankPoint.ResetTargetRotation();
        }

        protected override async UniTask<Dictionary<string, SelectingPanelElement>> FillContent(
            IUiFactory uiFactory,
            IPersistentProgressService persistentProgressService,
            Transform content)
        {
            Dictionary<string, SelectingPanelElement> panels = new Dictionary<string, SelectingPanelElement>();

            CharacterData[] allDatas = persistentProgressService.Progress.PlayerCharacters
                .OrderBy(data => data.IsUnlocked ? 0 : 1)
                .ToArray();

            foreach (CharacterData skinData in allDatas)
            {
                SelectingPanelElement panel = await uiFactory.CreateCharacterSkinPanel(content);
                Sprite icon = await _assetProvider.Load<Sprite>(_staticDataService.GetPlayerCharacter(skinData.Id).Icon);

                panel.Initialize(icon);

                if (skinData.IsBuyed)
                    panel.Unlock();

                if (skinData.IsUnlocked)
                    ((PlayerCharacterPanel)panel).RemoveBackground();

                panel.Clicked += OnPanelClicked;

                panels.Add(skinData.Id, panel);
            }

            return panels;
        }

        protected override void Select(string key, IPersistentProgressService persistentProgressService)
        {
            CharacterData skinData = persistentProgressService.Progress.GetPlayerCharacter(key);

            if (skinData.IsBuyed == false && skinData.IsUnlocked)
            {
                YandexGame.RewVideoShow(RewardID);
                persistentProgressService.Progress.UnlockCharacterSkin(key);
                persistentProgressService.Progress.BuyCharacterSkin(key);
            }
            else if (skinData.IsUnlocked && skinData.IsBuyed)
            {
                persistentProgressService.Progress.SelectCharacterSkin(key);
            }

            ActiveSelectionFrame(key);
        }

        protected override void Subscribe(IPersistentProgressService persistentProgressService)
        {
            persistentProgressService.Progress.CharacterSkinBuyed += Unlock;
        }

        protected override void Unsubscribe(IPersistentProgressService persistentProgressService)
        {
            persistentProgressService.Progress.CharacterSkinBuyed -= Unlock;
        }

        protected override string GetCurrentSelectedPanel(IPersistentProgressService persistentProgressService)
        {
            return persistentProgressService.Progress.SelectedPlayerCharacterId;
        }
    }
}
