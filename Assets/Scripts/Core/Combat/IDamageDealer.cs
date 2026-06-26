namespace Core.Combat
{
    // Интерфейс для объектов, которые могут наносить урон
    public interface IDamageDealer
    {
        // Возвращает урон, который наносит объект
        Damage GetDamage();
    }
}