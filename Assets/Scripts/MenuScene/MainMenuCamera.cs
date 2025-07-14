using UnityEngine;

namespace Assets.Scripts.MenuScene
{
    public class MainMenuCamera : MonoBehaviour
    {
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private Camera _mainCamera;

        public Camera UiCamera => _uiCamera;
        public Camera MainCamera => _mainCamera;

        public Ray GetRay(Vector2 handlePosition)
        {
            return _mainCamera.ScreenPointToRay(new Vector3(handlePosition.x, handlePosition.y, 1));
        }
    }
}
