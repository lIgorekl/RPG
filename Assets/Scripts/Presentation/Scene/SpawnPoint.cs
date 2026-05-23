using UnityEngine;

namespace Presentation.Scene
{
    /// <summary>
    /// Маркер точки спавна. Хранит только позицию и поворот.
    /// </summary>
    public class SpawnPoint : MonoBehaviour
    {
        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;
    }
}
