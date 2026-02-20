using UnityEngine;
using UnityEngine.InputSystem;

namespace Presentation.Player
{
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float distance = 5f;
        [SerializeField] private float sensitivity = 2f;
        [SerializeField] private float minY = -30f;
        [SerializeField] private float maxY = 60f;

        private float _yaw;
        private float _pitch;

        private void LateUpdate()
        {
            if (target == null) return;

            var mouse = Mouse.current;
            if (mouse == null) return;

            Vector2 delta = mouse.delta.ReadValue() * sensitivity;

            _yaw += delta.x;
            _pitch -= delta.y;
            _pitch = Mathf.Clamp(_pitch, minY, maxY);

            Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);
            Vector3 offset = rotation * new Vector3(0, 0, -distance);

            transform.position = target.position + offset;
            transform.LookAt(target.position + Vector3.up * 1.5f);
        }
    }
}