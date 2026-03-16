using UnityEngine;
using UnityEngine.InputSystem;
using Core.Combat;
using Presentation.Player;

namespace Presentation.Scene
{
    // Тестовый скрипт для нанесения урона игроку через клавишу.
    // Используется только для отладки.
    public class TestDamageTrigger : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private int damageValue = 20;

        private void Update()
        {
            var keyboard = Keyboard.current;

            if (keyboard == null || playerController == null)
                return;

            if (keyboard.kKey.wasPressedThisFrame)
            {
                var player = playerController.GetEntity();

                if (player == null)
                    return;

                player.ReceiveDamage(new Damage(damageValue, DamageType.Physical));

                Debug.Log($"Test damage applied: {damageValue}");
            }
        }
    }
}