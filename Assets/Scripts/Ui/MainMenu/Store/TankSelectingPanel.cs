using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Factoris.UI;
using Assets.Scripts.Services.AssetManagementServices;
using Assets.Scripts.Services.PersistentProgressServices;
using Assets.Scripts.Services.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.Ui.MainMenu.Store
{
    public class TankSelectingPanel : SelectionPanel<uint>
    {
        [SerializeField] private Image _icon;

        private IStaticDataService _staticDataService;
        private IAssetProvider _assetProvider;

        [Inject]
        private void Construct(IStaticDataService staticDataService, IAssetProvider assetProvider)
        {
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
        }

        protected override void Unsubscribe(IPersistentProgressService persistentProgressService)
        {
            persistentProgressService.Progress.TankUnlocked -= Unlock;
        }

        protected override void Subscribe(IPersistentProgressService persistentProgressService)
        {
            persistentProgressService.Progress.TankUnlocked += Unlock;
        }

        protected override async UniTask<Dictionary<uint, SelectingPanelElement>> FillContent(
            IUiFactory uiFactory,
            IPersistentProgressService persistentProgressService,
            Transform content)
        {
            Dictionary<uint, SelectingPanelElement> panels = new Dictionary<uint, SelectingPanelElement>();
            IOrderedEnumerable<TankData> tankDatas = persistentProgressService.Progress.Tanks.OrderBy(data => data.Level);

            foreach (TankData tankData in tankDatas)
            {
                SelectingPanelElement tankPanel = await uiFactory.CreateTankPanel(content);

                Sprite icon = await _assetProvider.Load<Sprite>(_staticDataService.GetTank(tankData.Level).Icon);

                tankPanel.Initialize(icon);

                if (tankData.IsUnlocked)
                    tankPanel.Unlock();

                tankPanel.Clicked += OnPanelClicked;

                panels.Add(tankData.Level, tankPanel);
            }

            return panels;
        }

        protected override void Select(uint key, IPersistentProgressService persistentProgressService)
        {
            persistentProgressService.Progress.TrySelectTank(key);
        }

        protected override uint GetCurrentSelectedPanel(IPersistentProgressService persistentProgressService)
        {
            return persistentProgressService.Progress.GetSelectedTank().Level;
        }
    }
}
