using UnityEngine;
using UnityEngine.InputSystem;

namespace Presentation.Player
{
    // Отвечает за движение и поворот игрока
    // Обрабатывает перемещение относительно камеры
    public class PlayerMovement
    {
        // Компонент перемещения персонажа
        private CharacterController _controller;

        // Камера, относительно которой рассчитывается движение
        private Camera _camera;

        // Компонент анимации персонажа
        private Animator _animator;

        // Параметры скорости движения
        private float _walkSpeed;
        private float _runSpeed;
        private float _rotationSpeed;

        // Текущее состояние движения
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

        // Обновляет движение игрока
        public void Update(PlayerInputData input)
        {
            // Проверяем наличие необходимых компонентов
            if (_camera == null || _controller == null)
                return;

            Vector2 moveInput = input.Move;

            // Нормализуем движение по диагонали
            if (moveInput.sqrMagnitude > 1f)
                moveInput.Normalize();

            // Если игрок стоит на месте, останавливаем анимацию
            if (moveInput.sqrMagnitude < 0.01f)
            {
                if (_animator != null)
                    _animator.SetFloat("Speed", 0f);

                return;
            }

            // Получаем направления вперед и вправо относительно камеры
            Vector3 forward = _camera.transform.forward;
            Vector3 right = _camera.transform.right;

            // Убираем влияние наклона камеры
            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            // Рассчитываем итоговое направление движения
            Vector3 moveDir =
                forward * moveInput.y +
                right * moveInput.x;

            // Определяем режим движения
            _state =
                input.Sprint
                    ? MovementState.Run
                    : MovementState.Walk;

            // Выбираем скорость в зависимости от режима
            float speed =
                _state == MovementState.Run
                    ? _runSpeed
                    : _walkSpeed;

            // Перемещаем персонажа
            _controller.Move(
                moveDir * speed * Time.deltaTime);

            // Поворачиваем персонажа в сторону движения
            Quaternion targetRotation =
                Quaternion.LookRotation(moveDir);

            _controller.transform.rotation =
                Quaternion.Slerp(
                    _controller.transform.rotation,
                    targetRotation,
                    _rotationSpeed * Time.deltaTime);

            // Обновляем анимацию движения
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