using Assets.Scripts.Data;

namespace Assets.Scripts.Services.PersistentProgressServices
{
    public class PersistentProgressService : IPersistentProgressService
    {
        public PlayerProgress Progress { get; set; }
    }
}
