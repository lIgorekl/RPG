using Core.Combat;

namespace Core.Combat
{
    public interface IDamageable
    {
        void ReceiveDamage(Damage damage);
    }
}