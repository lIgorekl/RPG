namespace Gameplay.Combat.Boss
{
    // Интерфейс для компонентов,
    // которые поддерживают случайную настройку вариантов босса
    // (например, оружия, стихии и т.д.)
    public interface IBossVariantAssignable
    {
        // Назначает случайные варианты босса
        // с использованием переданного генератора случайных чисел
        void AssignRandomVariants(System.Random random);
    }
}