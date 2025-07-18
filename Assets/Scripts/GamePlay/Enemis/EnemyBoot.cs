namespace Assets.Scripts.GamePlay.Enemis
{
    public class EnemyBoot : EnemyCar
    {
        protected override uint CalculateDamga(ExplosionInfo explosionInfo)
        {
            return explosionInfo.Damage;
        }
    }
}