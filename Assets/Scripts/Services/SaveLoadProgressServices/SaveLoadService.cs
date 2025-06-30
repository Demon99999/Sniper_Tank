using Assets.Scripts.Data;
using Assets.Scripts.Services.PersistentProgressServices;
using UnityEngine;

namespace Assets.Scripts.Services.SaveLoadProgressServices
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string Key = "Progress";

        private readonly IPersistentProgressService _progressService;

        public SaveLoadService(IPersistentProgressService progressService)
        {
            _progressService = progressService;
        }

        public PlayerProgress LoadProgress()
        {
            return PlayerPrefs.GetString(Key)?.ToDeserialized<PlayerProgress>();
        }

        public void SaveProgress()
        {
            PlayerPrefs.SetString(Key, _progressService.Progress.ToJson());
        }
    }
}
