using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.GamePlay.Player.Weapons;
using Assets.Scripts.Infrastructure.Factoris.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Ui.Game.BulletsPanel
{
    public abstract class BulletsPanel : MonoBehaviour
    {
        private IShootable _shootable;
        private IUiFactory _uiFactory;

        private List<BulletIcon> _icons;

        [Inject]
        private async void Construct(IShootable shootable, IUiFactory uiFactory)
        {
            _shootable = shootable;
            _uiFactory = uiFactory;

            _icons = new List<BulletIcon>();

            await CreateIcons((int)_shootable.BulletsCount);

            _shootable.BulletsCountChanged += OnBulletCountChanged;
        }

        private void OnDestroy()
        {
            _shootable.BulletsCountChanged -= OnBulletCountChanged;
        }

        protected abstract UniTask<BulletIcon> CreateBulletIcon(IUiFactory uiFactory);

        private async void OnBulletCountChanged()
        {
            if (_icons.Count > _shootable.BulletsCount)
            {
                DestroyIcons(_icons.Count - (int)_shootable.BulletsCount);
            }
            else
            {
                await CreateIcons((int)_shootable.BulletsCount - _icons.Count);
            }
        }

        private void DestroyIcons(int count)
        {
            List<BulletIcon> destroyedIcons = new List<BulletIcon>();

            destroyedIcons.AddRange(_icons.Take(count));
            _icons.RemoveRange(0, count);

            foreach (BulletIcon icon in destroyedIcons)
            {
                Destroy(icon.gameObject);
            }
        }

        private async UniTask CreateIcons(int count)
        {
            for (int i = 0; i < count; i++)
            {
                BulletIcon icon = await CreateBulletIcon(_uiFactory);
                _icons.Add(icon);
            }
        }
    }
}
