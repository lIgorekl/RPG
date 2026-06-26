using System;
using System.Collections.Generic;
using Presentation.Scene;

namespace App.Services.Spawn
{
    // Стратегия случайного выбора точек спавна.
    // Возвращает указанное количество случайных точек
    // из доступного списка.
    public class RandomSpawnPointSelector : ISpawnPointSelector
    {
        // Выбирает случайные точки появления
        public IReadOnlyList<SpawnPoint> Select(
            IReadOnlyList<SpawnPoint> allPoints,
            int count,
            Random random)
        {
            // Проверяем корректность входных данных
            if (allPoints == null ||
                allPoints.Count == 0 ||
                count <= 0)
            {
                return Array.Empty<SpawnPoint>();
            }

            // Создаем список,
            // содержащий только существующие точки спавна
            var shuffled =
                new List<SpawnPoint>(allPoints.Count);

            for (int i = 0; i < allPoints.Count; i++)
            {
                if (allPoints[i] != null)
                    shuffled.Add(allPoints[i]);
            }

            // Перемешиваем список точек
            // алгоритмом Фишера-Йетса
            for (int i = shuffled.Count - 1; i > 0; i--)
            {
                // Выбираем случайный индекс
                int swapIndex =
                    random.Next(0, i + 1);

                // Меняем элементы местами
                (shuffled[i], shuffled[swapIndex]) =
                    (shuffled[swapIndex], shuffled[i]);
            }

            // Если доступных точек достаточно,
            // возвращаем первые count элементов
            if (count <= shuffled.Count)
                return shuffled.GetRange(0, count);

            // Если требуется больше точек,
            // чем существует,
            // создаем новый список результата
            var result =
                new List<SpawnPoint>(count);

            // Добавляем все уникальные точки
            result.AddRange(shuffled);

            // Недостающие точки выбираем повторно случайным образом
            for (int i = shuffled.Count; i < count; i++)
            {
                int index =
                    random.Next(0, shuffled.Count);

                result.Add(shuffled[index]);
            }

            // Возвращаем итоговый список
            return result;
        }
    }
}