using System;
using UnityEngine;

namespace Gameplay.Combat.Boss
{
    [CreateAssetMenu(
        fileName = "BossElementLoadout",
        menuName = "RPG/Combat/Boss Element Loadout")]
    public class BossElementLoadout : ScriptableObject
    {
        [SerializeField] private BossElementConfig[] elements;

        public BossElementConfig PickRandom(System.Random random)
        {
            if (elements == null || elements.Length == 0)
                return null;

            int index = random.Next(0, elements.Length);
            return elements[index];
        }
    }
}
