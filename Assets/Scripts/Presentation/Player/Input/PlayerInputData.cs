using UnityEngine;

namespace Presentation.Player
{
    // Структура для хранения ввода игрока
    // Содержит данные движения и действий за текущий кадр
    public struct PlayerInputData
    {
        // Направление движения игрока
        public Vector2 Move;

        // Флаг бега
        public bool Sprint;

        // Флаг ближней атаки
        public bool MeleeAttackPressed;

        // Флаг магической атаки
        public bool MagicAttackPressed;
    }
}