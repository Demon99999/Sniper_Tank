using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.GamePlay.Player
{
    public class CameraNoise : MonoBehaviour
    {
        [SerializeField] private GameObject _noise;

        private void Start()
        {
            _noise.SetActive(false);
        }

        public void SetActive(bool isActive)
        {
            _noise.SetActive(isActive);
        }

        public class Factory : PlaceholderFactory<string, Transform, UniTask<CameraNoise>>
        {
        }
    }
}