using UnityEngine;
using UnityEngine.InputSystem;
using Core.Combat;
using Presentation.Player;

namespace Presentation.Scene
{
    public class TestDamageTrigger : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private int damageValue = 20;

        private void Update()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            if (keyboard.kKey.wasPressedThisFrame && playerController != null)
            {
                var player = playerController.GetEntity();
                if (player != null)
                {
                    player.ReceiveDamage(new Damage(damageValue, DamageType.Physical));
                    Debug.Log($"Test damage applied: {damageValue}");
                }
            }
        }
    }
}