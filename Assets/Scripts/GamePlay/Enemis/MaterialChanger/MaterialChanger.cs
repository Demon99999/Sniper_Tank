using System.Linq;
using Assets.Scripts.Enum;
using Assets.Scripts.Services.PersistentProgressServices;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.GamePlay.Enemis.MaterialChanger
{
    public class MaterialChanger : MonoBehaviour
    {
        [SerializeField] private MatchingMaterialToBiome[] _infos;
        [SerializeField] private Renderer _renderer;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService)
        {
            if (_infos == null || _infos.Length == 0)
            {
                return;
            }

            BiomeType currentBiomeType = persistentProgressService.Progress.CurrentBiomeType;

            MatchingMaterialToBiome currentMaterialsInfo = _infos.FirstOrDefault(info => info.BiomeType == currentBiomeType);

            if (currentMaterialsInfo == null)
            {
                return;
            }

            Material[] materials = _renderer.materials;

            foreach (MaterialInfo info in currentMaterialsInfo.MaterialInfos)
            {
                materials[info.Index] = info.Material;
            }

            _renderer.materials = materials;
        }
    }
}