using UnityEngine.InputSystem;

namespace Presentation.Player
{
    // Читает input из Unity Input System
    // и преобразует в PlayerInputData.
    public class PlayerInputReader
    {
        public PlayerInputData ReadInput()
        {
            PlayerInputData data =
                new PlayerInputData();

            var keyboard = Keyboard.current;
            var mouse = Mouse.current;

            if (keyboard != null)
            {
                if (keyboard.wKey.isPressed)
                    data.Move.y += 1f;

                if (keyboard.sKey.isPressed)
                    data.Move.y -= 1f;

                if (keyboard.aKey.isPressed)
                    data.Move.x -= 1f;

                if (keyboard.dKey.isPressed)
                    data.Move.x += 1f;

                data.Sprint =
                    keyboard.leftShiftKey.isPressed;
            }

            if (mouse != null)
            {
                data.MeleeAttackPressed =
                    mouse.leftButton.wasPressedThisFrame;

                data.MagicAttackPressed =
                    mouse.rightButton.wasPressedThisFrame;
            }

            return data;
        }
    }
}