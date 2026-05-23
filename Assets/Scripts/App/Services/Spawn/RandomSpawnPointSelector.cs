using System;
using System.Collections.Generic;
using Presentation.Scene;

namespace App.Services.Spawn
{
    public class RandomSpawnPointSelector : ISpawnPointSelector
    {
        public IReadOnlyList<SpawnPoint> Select(
            IReadOnlyList<SpawnPoint> allPoints,
            int count,
            Random random)
        {
            if (allPoints == null || allPoints.Count == 0 || count <= 0)
                return Array.Empty<SpawnPoint>();

            var shuffled = new List<SpawnPoint>(allPoints.Count);
            for (int i = 0; i < allPoints.Count; i++)
            {
                if (allPoints[i] != null)
                    shuffled.Add(allPoints[i]);
            }

            for (int i = shuffled.Count - 1; i > 0; i--)
            {
                int swapIndex = random.Next(0, i + 1);
                (shuffled[i], shuffled[swapIndex]) =
                    (shuffled[swapIndex], shuffled[i]);
            }

            int takeCount = Math.Min(count, shuffled.Count);
            return shuffled.GetRange(0, takeCount);
        }
    }
}
