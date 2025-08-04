using Assets.Scripts.GamePlay.Destructions;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts.GamePlay.Player
{
    public class DroneEffects : MonoBehaviour
    {
        private const string MixerVolume = "Volume";

        [SerializeField] private DroneExplosion _explosion;
        [SerializeField] private GameObject _droneModel;
        [SerializeField] private Collider _collider;
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private float _soundVolume = 0f;

        private float _startSoundVolume;

        public void PlayActivationSound()
        {
            _audioMixer.GetFloat(MixerVolume, out _startSoundVolume);
            _audioMixer.SetFloat(MixerVolume, _soundVolume);
            _audioSource.Play();
        }

        public void PlayExplosion()
        {
            _collider.enabled = false;
            _explosion.Explode();
            Destroy(_droneModel);
            _audioSource.Stop();
            _audioMixer.SetFloat(MixerVolume, _startSoundVolume);
        }

        public void ShowCursor(bool enable) => Cursor.visible = enable;
        public void HideCursor(bool enable) => Cursor.visible = !enable;
    }
}