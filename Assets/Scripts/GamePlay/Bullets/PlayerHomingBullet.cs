using Assets.Scripts.GamePlay.Player.Wrappers;
using Zenject;

namespace Assets.Scripts.GamePlay.Bullets
{
    public class PlayerHomingBullet : HomingBullet
    {
        private PlayerWrapper _playerWrapper;

        [Inject]
        private void Construct(PlayerWrapper playerWrapper)
        {
            _playerWrapper = playerWrapper;
        }

        protected override void SearchTarget(uint searchRadius)
        {
            Target = _playerWrapper.transform;
            StartCoroutine(TargetLocator());
        }
    }
}
