using Assets.Scripts.GamePlay.Camera;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.GamePlay.Player.DronePlayer
{
    public class DroneCameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private float _cameraBankMultiplier = 0.6f;
        [SerializeField] private float _cameraBankSmoothness = 3f;
        [SerializeField] private float _cameraBlendDuration;

        private Quaternion _cameraBaseRotation;
        private float _currentCameraBankAngle;
        private DroneMovement _droneMovement;

        private GameplayCamera _gameplayCamera;

        private void Awake()
        {
            _cameraBaseRotation = _camera.transform.localRotation;
            _droneMovement = GetComponent<DroneMovement>();
        }

        [Inject]
        private void Construct(GameplayCamera gameplayCamera)
        {
            _gameplayCamera = gameplayCamera;
        }

        private void Update()
        {
            UpdateCameraBanking();
        }

        public void SetActive(bool active, float blendDuration = -1)
        {
            _camera.Priority = active ? 1 : 0;
            _gameplayCamera.SetBlednDuration(_cameraBlendDuration);
        }


        private void UpdateCameraBanking()
        {
            float targetCameraBank = _droneMovement.CurrentBankAngle * _cameraBankMultiplier;
            _currentCameraBankAngle = Mathf.Lerp(
                _currentCameraBankAngle,
                targetCameraBank,
                _cameraBankSmoothness * Time.deltaTime
            );

            _camera.transform.localRotation = _cameraBaseRotation *
                                              Quaternion.Euler(0, 0, _currentCameraBankAngle);
        }
    }
}
