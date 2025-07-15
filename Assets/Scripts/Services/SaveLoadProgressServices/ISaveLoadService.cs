using Assets.Scripts.Data;

namespace Assets.Scripts.Services.SaveLoadProgressServices
{
    public interface ISaveLoadService
    {
        void SaveProgress();
        PlayerProgress LoadProgress();
    }
}
