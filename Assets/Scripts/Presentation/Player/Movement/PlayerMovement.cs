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

        public void Update(PlayerInputData input)
        {
            if (_camera == null || _controller == null)
                return;

            Vector2 moveInput = input.Move;

            // Нормализация диагонального движения
            if (moveInput.sqrMagnitude > 1f)
                moveInput.Normalize();

            // Если игрок не движется — останавливаем анимацию
            if (moveInput.sqrMagnitude < 0.01f)
            {
                if (_animator != null)
                    _animator.SetFloat("Speed", 0f);

                return;
            }

            // Движение относительно камеры
            Vector3 forward = _camera.transform.forward;
            Vector3 right = _camera.transform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            Vector3 moveDir =
                forward * moveInput.y +
                right * moveInput.x;

            // Переключение состояния
            _state =
                input.Sprint
                    ? MovementState.Run
                    : MovementState.Walk;

            float speed =
                _state == MovementState.Run
                    ? _runSpeed
                    : _walkSpeed;

            _controller.Move(
                moveDir * speed * Time.deltaTime);

            // Поворот персонажа
            Quaternion targetRotation =
                Quaternion.LookRotation(moveDir);

            _controller.transform.rotation =
                Quaternion.Slerp(
                    _controller.transform.rotation,
                    targetRotation,
                    _rotationSpeed * Time.deltaTime);

            // Анимация
            if (_animator != null)
            {
                float speedPercent =
                    _state == MovementState.Run
                        ? 1f
                        : 0.5f;

                _animator.SetFloat("Speed", speedPercent);
            }
        }
    }
}