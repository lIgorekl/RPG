using UnityEngine;

// Поворачивает объект лицом к камере.
// Используется для UI элементов над персонажами (например HP bar).
public class Billboard : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        if (_camera == null)
            return;

        transform.forward = _camera.transform.forward;
    }
}