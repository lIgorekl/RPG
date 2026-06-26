using UnityEngine;
using UnityEngine.InputSystem;

namespace Presentation.Player
{
    // Управляет камерой игрока
    // Вращает камеру вокруг персонажа и удерживает его в центре обзора
    public class PlayerCameraController : MonoBehaviour
    {
        // Объект, за которым следует камера
        [SerializeField] private Transform target;

        // Настройки расстояния и чувствительности камеры
        [SerializeField] private float distance = 5f;
        [SerializeField] private float sensitivity = 2f;

        // Ограничения вертикального вращения
        [SerializeField] private float minY = -30f;
        [SerializeField] private float maxY = 60f;

        // Текущие углы вращения камеры
        private float _yaw;
        private float _pitch;

        private void LateUpdate()
        {
            // Проверяем наличие цели
            if (target == null)
                return;

            var mouse = Mouse.current;

            // Проверяем наличие устройства ввода
            if (mouse == null)
                return;

            // Считываем движение мыши
            Vector2 delta = mouse.delta.ReadValue() * sensitivity;

            // Изменяем углы поворота камеры
            _yaw += delta.x;
            _pitch -= delta.y;

            // Ограничиваем угол обзора по вертикали
            _pitch = Mathf.Clamp(_pitch, minY, maxY);

            // Формируем вращение камеры
            Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);

            // Вычисляем смещение относительно игрока
            Vector3 offset = rotation * new Vector3(0, 0, -distance);

            // Перемещаем камеру вокруг цели
            transform.position = target.position + offset;

            // Камера всегда смотрит на персонажа
            transform.LookAt(target.position + Vector3.up * 1.5f);
        }
    }
}