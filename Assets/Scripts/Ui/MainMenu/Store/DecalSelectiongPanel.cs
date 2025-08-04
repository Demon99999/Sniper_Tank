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
    public class DecalSelectiongPanel : SelectionPanel<string>
    {
        private const string Texture = "_Texture";
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
            DecalData[] decalDatas = persistentProgressService.Progress.Decals;

            decalDatas = decalDatas.OrderBy(data => _staticDataService.GetDecal(data.Id).SerialNumber).ToArray();

            foreach (DecalData decalData in decalDatas)
            {
                SelectingPanelElement panel = await uiFactory.CreateDecalPanel(content);

                Sprite sprite = await _assetProvider.Load<Sprite>(_staticDataService.GetDecal(decalData.Id).SpriteAssetReference);

                panel.Initialize(sprite);

                if (decalData.IsUnlocked)
                    panel.Unlock();

                panel.Clicked += OnPanelClicked;

                panels.Add(decalData.Id, panel);
            }

            return panels;
        }

        protected override void Select(string key, IPersistentProgressService persistentProgressService)
        {
            DecalData decalData = persistentProgressService.Progress.GetDecal(key);

            if (decalData.IsUnlocked == false)
            {
                //YandexGame.RewVideoShow(RewardID);
                persistentProgressService.Progress.UnlockDecal(key);
            }
            else
            {
                persistentProgressService.Progress.SelectDecal(key);
            }

            ActiveSelectionFrame(key);
        }

        protected override void Subscribe(IPersistentProgressService persistentProgressService)
        {
            persistentProgressService.Progress.DecalUnlocked += Unlock;
        }

        protected override void Unsubscribe(IPersistentProgressService persistentProgressService)
        {
            persistentProgressService.Progress.DecalUnlocked -= Unlock;
        }

        protected override string GetCurrentSelectedPanel(IPersistentProgressService persistentProgressService)
        {
            return persistentProgressService.Progress.GetSelectedTank().DecalId;
        }
    }
}
