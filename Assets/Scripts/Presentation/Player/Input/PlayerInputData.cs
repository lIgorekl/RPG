using UnityEngine;

namespace Presentation.Player
{
    // Данные ввода игрока.
    public struct PlayerInputData
    {
        public Vector2 Move;

        public bool Sprint;

        public bool MeleeAttackPressed;

        public bool MagicAttackPressed;
    }
}