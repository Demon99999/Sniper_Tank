using Cysharp.Threading.Tasks;

namespace Assets.Scripts.Ui.LoadingCurtain
{
    public class LoadingCurtainProxy : ILoadingCurtain
    {
        public const string Curtain = "Curtain";

        private readonly LoadingCurtain.Factory _factory;

        private ILoadingCurtain _implementation;

        public LoadingCurtainProxy(LoadingCurtain.Factory factory)
        {
            _factory = factory;
        }

        public async UniTask InitializeAsync()
        {
            _implementation = await _factory.Create(Curtain);
        }

        public void Show()
        {
            _implementation.Show();
        }

        public void Hide()
        {
            _implementation.Hide();
        }
    }
}