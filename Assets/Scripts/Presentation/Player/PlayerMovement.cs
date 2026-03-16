using UnityEngine;
using UnityEngine.InputSystem;

namespace Presentation.Player
{
    // Отвечает за движение игрока: обработку ввода, перемещение и поворот персонажа.
    // Не является MonoBehaviour — управляется из PlayerController.
    public class PlayerMovement
    {
        private CharacterController _controller;
        private Camera _camera;
        private Animator _animator;

        private float _walkSpeed;
        private float _runSpeed;
        private float _rotationSpeed;

        // Текущее состояние движения (ходьба или бег)
        private enum MovementState
        {
            Walk,
            Run
        }

        private MovementState _state = MovementState.Walk;

        public PlayerMovement(
            CharacterController controller,
            Camera camera,
            Animator animator,
            float walkSpeed,
            float runSpeed,
            float rotationSpeed)
        {
            _controller = controller;
            _camera = camera;
            _animator = animator;

            _walkSpeed = walkSpeed;
            _runSpeed = runSpeed;
            _rotationSpeed = rotationSpeed;
        }

        public void Update()
        {
            if (_camera == null || _controller == null)
                return;

            var keyboard = Keyboard.current;
            if (keyboard == null)
                return;

            // Определяем режим движения (Shift = бег)
            _state = keyboard.leftShiftKey.isPressed
                ? MovementState.Run
                : MovementState.Walk;

            float horizontal = 0f;
            float vertical = 0f;

            // Считываем WASD
            if (keyboard.aKey.isPressed) horizontal = -1f;
            if (keyboard.dKey.isPressed) horizontal = 1f;
            if (keyboard.wKey.isPressed) vertical = 1f;
            if (keyboard.sKey.isPressed) vertical = -1f;

            Vector3 input = new Vector3(horizontal, 0f, vertical).normalized;

            // Если игрок не движется — останавливаем анимацию
            if (input.sqrMagnitude < 0.01f)
            {
                if (_animator != null)
                    _animator.SetFloat("Speed", 0f);

                return;
            }

            // Движение относительно направления камеры
            Vector3 forward = _camera.transform.forward;
            Vector3 right = _camera.transform.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            Vector3 moveDir = forward * input.z + right * input.x;

            float speed = _state == MovementState.Run ? _runSpeed : _walkSpeed;

            _controller.Move(moveDir * speed * Time.deltaTime);

            // Плавный поворот персонажа в сторону движения
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);

            _controller.transform.rotation =
                Quaternion.Slerp(
                    _controller.transform.rotation,
                    targetRotation,
                    _rotationSpeed * Time.deltaTime);

            // Управление анимацией движения
            if (_animator != null)
            {
                float speedPercent =
                    _state == MovementState.Run ? 1f : 0.5f;

                _animator.SetFloat("Speed", speedPercent);
            }
        }
    }
}