using System;
using System.Linq;
using Assets.Scripts.GamePlay.Tanks;
using Assets.Scripts.Infrastructure.Factoris.TankFactory;
using Assets.Scripts.Services.PersistentProgressServices;
using Assets.Scripts.Services.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.MenuScene.Desk
{
    public class DeskCell : MonoBehaviour
    {
        [SerializeField] private Transform _tankPoint;
        [SerializeField] private Marker _marker;
        [SerializeField] private Quaternion _tankRotation;
        [SerializeField] private float _tankScale;
        [SerializeField] private string _id;

        private ITankFactory _tankFactory;
        private IPersistentProgressService _persistentProgressService;
        private IStaticDataService _staicDataService;
        private DeskHandler _deskHandler;

        private DeskTankWrapper _tankWrapper;
        private uint _maxTankLevel;

        public event Action EmploymentChanged;

        public bool IsEmpty => _tankWrapper == null;
        public uint TankLevel => _tankWrapper.TankLevel;

        [Inject]
        private async void Construct(
            ITankFactory tankFactory,
            IPersistentProgressService persistentProgressService,
            IStaticDataService staticDataService,
            DeskHandler deskHandler)
        {
            _tankFactory = tankFactory;
            _persistentProgressService = persistentProgressService;
            _staicDataService = staticDataService;
            _deskHandler = deskHandler;

            _maxTankLevel = _staicDataService.TankConfigs.OrderByDescending(config => config.Level).First().Level;

            await Initialize();
        }

        public async UniTask CreateTank(uint level, bool needAnimate, bool needParticles)
        {
            _deskHandler.SetActive(false);

            uint actualyTankLevel = level > _maxTankLevel ? level - ((uint)(level / _maxTankLevel) * _maxTankLevel) : level;

            _tankWrapper = await _tankFactory.CreateDeskTankWrapper(_tankPoint.position, transform);
            _tankWrapper.Initialize(level);

            Tank tank = await _tankFactory.CreateTank(actualyTankLevel, _tankWrapper.TankPoint.position,
                _tankRotation, _tankWrapper.TankPoint, string.Empty, string.Empty);

            tank.SetLevel(level);
            tank.transform.localScale = Vector3.one * _tankScale;

            if (needAnimate)
                _tankWrapper.Animate(needParticles);

            _persistentProgressService.Progress.DeskData.UpdateCellInfo(_id, tank.Level);

            _deskHandler.SetActive(true);

            EmploymentChanged?.Invoke();
        }

        public async UniTask UpgradeTank()
        {
            uint targetLevel = _tankWrapper.TankLevel + 1;

            if (targetLevel <= _maxTankLevel)
                _persistentProgressService.Progress.TryUnlockTank(targetLevel);

            _tankWrapper.Destroy();

            await CreateTank(targetLevel, true, true);
        }

        public void PlaceTank(DeskTankWrapper tankWrapper)
        {
            _tankWrapper = tankWrapper;
            _tankWrapper.transform.position = _tankPoint.position;
            _tankWrapper.transform.parent = transform;
            _persistentProgressService.Progress.DeskData.UpdateCellInfo(_id, _tankWrapper.TankLevel);

            EmploymentChanged?.Invoke();
        }

        public DeskTankWrapper GetTankWrapper()
        {
            DeskTankWrapper tankWrapper = _tankWrapper;
            _tankWrapper = null;
            _persistentProgressService.Progress.DeskData.UpdateCellInfo(_id, 0);

            return tankWrapper;
        }

        public bool CanPlace(uint tankLevel)
        {
            return (IsEmpty == false && _tankWrapper.TankLevel != tankLevel) == false;
        }

        public void Mark(uint tankLevel)
        {
            _marker.Mark(CanPlace(tankLevel));
        }

        public void HideMark()
        {
            _marker.Hide();
        }

        private async UniTask Initialize()
        {
            uint tankLevel = _persistentProgressService.Progress.DeskData.GetCellInfo(_id);

            if (tankLevel != 0)
                await CreateTank(tankLevel, false, false);
        }
    }
}
