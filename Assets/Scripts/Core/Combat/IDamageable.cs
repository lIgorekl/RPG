namespace Core.Combat
{
    // Контракт для объектов, которые могут получать урон.
    public interface IDamageable
    {
        void ReceiveDamage(Damage damage);
    }
}