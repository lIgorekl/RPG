using UnityEngine;

namespace Presentation.Scene
{
    // Точка спавна объектов на сцене.
    // Хранит только позицию и поворот,
    // в которых должен появиться объект.
    public class SpawnPoint : MonoBehaviour
    {
        // Координаты точки спавна
        public Vector3 Position => transform.position;

        // Поворот точки спавна
        public Quaternion Rotation => transform.rotation;
    }
}