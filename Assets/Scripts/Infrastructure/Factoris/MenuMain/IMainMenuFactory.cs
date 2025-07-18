using Cysharp.Threading.Tasks;

namespace Assets.Scripts.Infrastructure.Factoris.MenuMain
{
    public interface IMainMenuFactory
    {
        UniTask CreateDesk();
    }
}