using System.Collections.Generic;
using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Factoris.UI;
using Assets.Scripts.Services.AssetManagementServices;
using Assets.Scripts.Services.PersistentProgressServices;
using Assets.Scripts.Services.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

namespace Assets.Scripts.Ui.MainMenu.Store
{
    public class TankSkinSelectingPanel : SelectionPanel<string>
    {
        private const string Texture = "_Texture";
        private const int RewardID = 6;

        [SerializeField] private Button _baseSkinButton;

        private IStaticDataService _staticDataServcie;
        private IAssetProvider _assetProvider;

        [Inject]
        private void Construct(IStaticDataService staticDataService, IAssetProvider assetProvider)
        {
            _staticDataServcie = staticDataService;
            _assetProvider = assetProvider;

            YandexGame.RewardVideoEvent += OnRewarded;
        }

        private void OnDestroy()
        {
            YandexGame.RewardVideoEvent -= OnRewarded;
        }

        protected override async UniTask<Dictionary<string, SelectingPanelElement>> FillContent(
            IUiFactory uiFactory,
            IPersistentProgressService persistentProgressService,
            Transform content)
        {
            Dictionary<string, SelectingPanelElement> panels = new Dictionary<string, SelectingPanelElement>();

            foreach (TankSkinData tankSkinData in persistentProgressService.Progress.TankSkins)
            {
                SelectingPanelElement panel = await uiFactory.CreateUnlockingPanel(content);

                Material material = await _assetProvider.Load<Material>(_staticDataServcie.GetSkin(tankSkinData.Id).MaterialAssetReference);
                Texture2D texture = material.GetTexture(Texture) as Texture2D;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(texture.width / 2, texture.height / 2));

                panel.Initialize(sprite);

                if (tankSkinData.IsUnlocked)
                    panel.Unlock();

                panel.Clicked += OnPanelClicked;

                panels.Add(tankSkinData.Id, panel);
            }

            return panels;
        }

        protected override string GetCurrentSelectedPanel(IPersistentProgressService persistentProgressService)
        {
            return persistentProgressService.Progress.GetSelectedTank().SkinId;
        }

        protected override void Select(string key, IPersistentProgressService persistentProgressService)
        {
            TankSkinData tankSkinData = persistentProgressService.Progress.GetSkin(key);

            if (tankSkinData.IsUnlocked == false)
            {
                YandexGame.RewVideoShow(RewardID);
                persistentProgressService.Progress.UnlockTankSkin(key);

//#if !UNITY_WEBGL || UNITY_EDITOR
//                persistentProgressService.Progress.UnlockTankSkin(key);
//#else
//            Agava.YandexGames.InterstitialAd.Show(onCloseCallback: (value) =>
//            {
//                persistentProgressService.Progress.UnlockTankSkin(key);
//            });
//#endif
            }
            else
            {
                persistentProgressService.Progress.SelectTankSkin(key);
            }
        }

        protected override void Subscribe(IPersistentProgressService persistentProgressService)
        {
            persistentProgressService.Progress.TankSkinUnlocked += Unlock;
            _baseSkinButton.onClick.AddListener(OnBaseSkinButtonClicked);
        }

        protected override void Unsubscribe(IPersistentProgressService persistentProgressService)
        {
            persistentProgressService.Progress.TankSkinUnlocked -= Unlock;
            _baseSkinButton.onClick.RemoveListener(OnBaseSkinButtonClicked);
        }

        private void OnBaseSkinButtonClicked()
        {
            PersistentProgressService.Progress.SelectTankSkin(string.Empty);
        }

        private void OnRewarded(int id)
        {
            if (id == RewardID)
            {

            }
        }
    }
}
