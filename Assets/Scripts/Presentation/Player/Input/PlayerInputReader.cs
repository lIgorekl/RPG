using UnityEngine.InputSystem;

namespace Presentation.Player
{
    // Читает ввод игрока
    // Преобразует данные из Input System в PlayerInputData
    public class PlayerInputReader
    {
        public PlayerInputData ReadInput()
        {
            // Создаем объект для хранения текущего ввода
            PlayerInputData data =
                new PlayerInputData();

            var keyboard = Keyboard.current;
            var mouse = Mouse.current;

            if (keyboard != null)
            {
                // Формируем направление движения по клавишам WASD
                if (keyboard.wKey.isPressed)
                    data.Move.y += 1f;

                if (keyboard.sKey.isPressed)
                    data.Move.y -= 1f;

                if (keyboard.aKey.isPressed)
                    data.Move.x -= 1f;

                if (keyboard.dKey.isPressed)
                    data.Move.x += 1f;

                // Проверяем удержание клавиши бега
                data.Sprint =
                    keyboard.leftShiftKey.isPressed;
            }

            if (mouse != null)
            {
                // Нажатие левой кнопки мыши для ближней атаки
                data.MeleeAttackPressed =
                    mouse.leftButton.wasPressedThisFrame;

                // Нажатие правой кнопки мыши для магической атаки
                data.MagicAttackPressed =
                    mouse.rightButton.wasPressedThisFrame;
            }

            return data;
        }
    }
}