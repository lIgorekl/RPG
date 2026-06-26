namespace Core.Combat
{
    // Интерфейс для объектов, способных получать урон
    public interface IDamageable
    {
        // Получает входящий урон
        void ReceiveDamage(Damage damage);
    }
}