using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.GamePlay.Camera
{
    public class UiCamera : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera _uiCamera;

        public UnityEngine.Camera Camera => _uiCamera;

        public class Factory : PlaceholderFactory<string, UniTask<UiCamera>>
        {
        }
    }
}