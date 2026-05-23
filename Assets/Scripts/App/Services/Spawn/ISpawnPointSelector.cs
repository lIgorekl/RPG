using System;
using System.Collections.Generic;
using Presentation.Scene;

namespace App.Services.Spawn
{
    public interface ISpawnPointSelector
    {
        IReadOnlyList<SpawnPoint> Select(
            IReadOnlyList<SpawnPoint> allPoints,
            int count,
            Random random);
    }
}
