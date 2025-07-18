using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Ui.Game.BulletsPanel
{
    public class BulletIcon : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<string, Transform, UniTask<BulletIcon>>
        {
        }
    }
}
