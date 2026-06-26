using UnityEngine;

// Поворачивает объект в сторону камеры
// Используется для отображения UI над персонажами
public class Billboard : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        // Получаем основную камеру сцены
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        if (_camera == null)
            return;

        // Разворачиваем объект в ту же сторону, что и камера
        transform.forward = _camera.transform.forward;
    }
}