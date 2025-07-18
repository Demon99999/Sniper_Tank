using Assets.Scripts.Infrastructure.Factoris.UI;
using Cysharp.Threading.Tasks;

namespace Assets.Scripts.Ui.Game.BulletsPanel
{
    public class TankBulletsPanel : BulletsPanel
    {
        protected override async UniTask<BulletIcon> CreateBulletIcon(IUiFactory uiFactory)
        {
            return await uiFactory.CreateTankBulletIcon(transform);
        }
    }
}