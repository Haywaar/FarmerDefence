namespace Units
{
    public class MeleeEnemy : AbstractEnemy
    {
        public override void Attack()
        {
            _player.DamagePlayer(_damageValue);
            _enemyManager.Dispose(this);
        }
    }
}
