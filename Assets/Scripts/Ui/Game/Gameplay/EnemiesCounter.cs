using Assets.Scripts.GamePlay.Handlers;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Ui.Game.Gameplay
{
    public class EnemiesCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _value;

        private VictoryHandler _vinHandler;

        [Inject]
        private void Construct(VictoryHandler winHandler)
        {
            _vinHandler = winHandler;

            ChangeValue(0);

            _vinHandler.DestructedEnemiesCountChanger += ChangeValue;
        }

        private void OnDestroy()
        {
            _vinHandler.DestructedEnemiesCountChanger -= ChangeValue;
        }

        private void ChangeValue(int destructedEnemiesCount)
        {
            _value.text = $"{destructedEnemiesCount}/{_vinHandler.MaxEnemiesCount}";
        }
    }
}
