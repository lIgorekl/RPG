namespace Core.Combat
{
    // Контракт для объектов, которые могут наносить урон.
    public interface IDamageDealer
    {
        Damage GetDamage();
    }
}