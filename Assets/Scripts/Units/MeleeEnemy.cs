namespace Units
{
    public class MeleeEnemy : AbstractEnemy
    {
        public override void Attack()
        {
            Player.Instance.DamagePlayer(_damageValue);
            EnemyManager.Instance.Dispose(this);
        }
    }
}
