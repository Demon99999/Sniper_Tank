using Assets.Scripts.Data;

namespace Assets.Scripts.Services.PersistentProgressServices
{
    public interface IPersistentProgressService
    {
        PlayerProgress Progress { get; set; }
    }
}
