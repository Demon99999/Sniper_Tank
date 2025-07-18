using System;
using System.Linq;
using Assets.Scripts.Services.SaveLoadProgressServices;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Assets.Scripts.MenuScene.Desk
{
    public class Desk : MonoBehaviour
    {
        [SerializeField] private DeskCell[] _cells;

        private ISaveLoadService _saveLoadService;

        public event Action<bool> EmploymentChanged;

        public bool HasEmptyCells => _cells.Any(cell => cell.IsEmpty);

        [Inject]
        private void Construct(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;

            foreach (DeskCell deskCell in _cells)
                deskCell.EmploymentChanged += OnDeskCellEmploymentChanged;

            OnDeskCellEmploymentChanged();
        }

        private void OnDestroy()
        {
            foreach (DeskCell deskCell in _cells)
            {
                deskCell.EmploymentChanged -= OnDeskCellEmploymentChanged;
            }
        }

        public async UniTask CreateTank(uint level)
        {
            DeskCell[] emptyCells = _cells.Where(c => c.IsEmpty).ToArray();

            DeskCell cell = emptyCells[Random.Range(0, emptyCells.Length)];

            await cell.CreateTank(level, true, false);

            _saveLoadService.SaveProgress();
        }

        private void OnDeskCellEmploymentChanged()
        {
            EmploymentChanged?.Invoke(HasEmptyCells);
        }

        public class Factory : PlaceholderFactory<string, UniTask<Desk>>
        {
        }
    }
}
