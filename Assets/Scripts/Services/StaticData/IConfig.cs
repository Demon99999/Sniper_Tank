namespace Assets.Scripts.Services.StaticData
{
    public interface IConfig<TKey>
    {
        TKey Key { get; }
    }
}