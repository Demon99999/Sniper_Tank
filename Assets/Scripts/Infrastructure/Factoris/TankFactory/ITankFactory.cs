using Assets.Scripts.GamePlay.Player;
using Assets.Scripts.GamePlay.Player.Wrappers;
using Assets.Scripts.GamePlay.Tanks;
using Assets.Scripts.MenuScene;
using Assets.Scripts.MenuScene.Desk;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Factoris.TankFactory
{
    public interface ITankFactory
    {
        UniTask<DeskTankWrapper> CreateDeskTankWrapper(Vector3 position, Transform parent);
        UniTask<Drone> CreateDrone(Vector3 position, Quaternion rotation);
        UniTask<PlayerCharacter> CreatePlayerCharacter(string id, Vector3 position, Quaternion rotation, Transform parent);
        UniTask CreatePlayerDroneContoller(Vector3 position, Quaternion rotation, Transform parent);
        UniTask CreatePlayerDroneWrapper(Vector3 position, Quaternion rotation);
        UniTask<PlayerAccessor> CreatePlayerGlasses(Vector3 position, Quaternion rotation, Transform parent);
        UniTask<PlayerTankWrapper> CreatePlayerTankWrapper(uint tankLevel, Vector3 position, Quaternion rotation);
        UniTask<Tank> CreateTank(
            uint level,
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            string skinId,
            string decalId,
            bool isDecalsChangable = false);
        UniTask<TankShootingWrapper> CreateTankShootingWrapper(uint tankLevel, Vector3 position, Quaternion rotation, Transform parent);
    }
}
