using UnityEngine;
using UnityEngine.InputSystem;

namespace Presentation.Player
{
    // Управляет камерой игрока (вид от третьего лица).
    // Камера вращается вокруг цели и всегда смотрит на неё.
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField] private Transform target;

        [SerializeField] private float distance = 5f;
        [SerializeField] private float sensitivity = 2f;

        [SerializeField] private float minY = -30f;
        [SerializeField] private float maxY = 60f;

        // Текущие углы вращения камеры
        private float _yaw;
        private float _pitch;

        private void LateUpdate()
        {
            if (target == null)
                return;

            var mouse = Mouse.current;
            if (mouse == null)
                return;

            // Считываем движение мыши
            Vector2 delta = mouse.delta.ReadValue() * sensitivity;

            _yaw += delta.x;
            _pitch -= delta.y;

            // Ограничиваем вертикальный угол
            _pitch = Mathf.Clamp(_pitch, minY, maxY);

            // Рассчитываем позицию камеры вокруг игрока
            Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);
            Vector3 offset = rotation * new Vector3(0, 0, -distance);

            transform.position = target.position + offset;

            // Камера всегда смотрит на игрока
            transform.LookAt(target.position + Vector3.up * 1.5f);
        }
    }
}