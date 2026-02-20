using UnityEngine;

namespace Presentation.Scene
{
    public class RangedEnemyView : BaseEnemyView
    {
        [Header("Ranged Enemy Settings")]
        [SerializeField] private float attackRange = 8f;

        // Логика дистанционного поведения появится позже
    }
}