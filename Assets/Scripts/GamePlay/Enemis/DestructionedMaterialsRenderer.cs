using Assets.Scripts.Services.StaticData;
using Assets.Scripts.Services.StaticData.ScriptableConfig;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.GamePlay.Enemis
{
    public class DestructionedMaterialsRenderer : MonoBehaviour
    {
        private const string ColorValue = "_BaseColor";

        [SerializeField] private MeshRenderer[] _renderers;

        private DestructionConfig _destructionConfig;

        [Inject]
        private void Construct(IStaticDataService staticDataService)
        {
            _destructionConfig = staticDataService.DestructionConfig;
        }

        public void Render()
        {
            foreach (MeshRenderer meshRenderer in _renderers)
            {
                if (meshRenderer == null)
                    continue;

                foreach (Material material in meshRenderer.materials)
                {
                    material.SetColor(ColorValue, material.GetColor(ColorValue) * _destructionConfig.DestructionColor);
                }
            }
        }
    }
}