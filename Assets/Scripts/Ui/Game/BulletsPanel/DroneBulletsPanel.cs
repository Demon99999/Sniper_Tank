using Assets.Scripts.Infrastructure.Factoris.UI;
using Cysharp.Threading.Tasks;

namespace Assets.Scripts.Ui.Game.BulletsPanel
{
    public class DroneBulletsPanel : BulletsPanel
    {
        protected override async UniTask<BulletIcon> CreateBulletIcon(IUiFactory uiFactory)
        {
            return await uiFactory.CreateDroneBulletIcon(transform);
        }
    }
}
