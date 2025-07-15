using Assets.Scripts.Infrastructure.Factoris;
using Assets.Scripts.MenuScene.Desk;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.MenuScene
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuCamera _camera;

        public override void InstallBindings()
        {
            
        }

        
    }
}
