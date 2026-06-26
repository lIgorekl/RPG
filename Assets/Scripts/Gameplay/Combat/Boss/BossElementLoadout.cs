using System;
using UnityEngine;

namespace Gameplay.Combat.Boss
{
    // Набор возможных стихий босса.
    // Хранится как ScriptableObject и используется
    // для случайного выбора стихии при создании босса.
    [CreateAssetMenu(
        fileName = "BossElementLoadout",
        menuName = "RPG/Combat/Boss Element Loadout")]
    public class BossElementLoadout : ScriptableObject
    {
        // Список доступных конфигураций стихий
        [SerializeField] private BossElementConfig[] elements;

        // Возвращает случайную конфигурацию стихии
        public BossElementConfig PickRandom(System.Random random)
        {
            // Если список пуст, выбирать нечего
            if (elements == null || elements.Length == 0)
                return null;

            // Выбираем случайный индекс массива
            int index = random.Next(0, elements.Length);

            // Возвращаем выбранную конфигурацию
            return elements[index];
        }
    }
}