using System;
using System.Collections.Generic;
using App.Services;
using Presentation.AI;
using Presentation.Scene;
using UnityEngine;

namespace App.Services.Spawn
{
    // Сервис спавна врагов.
    // Формирует план появления врагов и создает их через фабрику.
    public class EnemySpawnService : IEnemySpawnService
    {
        // Фабрика создания врагов
        private readonly IEnemyFactory _enemyFactory;

        // Стратегия выбора точек появления
        private readonly ISpawnPointSelector _spawnPointSelector;

        // Стратегия выбора типа врага
        private readonly IEnemySpawnDefinitionSelector _definitionSelector;

        // Счетчик для генерации уникальных идентификаторов врагов
        private int _spawnCounter;

        public EnemySpawnService(
            IEnemyFactory enemyFactory,
            ISpawnPointSelector spawnPointSelector,
            IEnemySpawnDefinitionSelector definitionSelector)
        {
            _enemyFactory = enemyFactory;
            _spawnPointSelector = spawnPointSelector;
            _definitionSelector = definitionSelector;
        }

        // Основной метод создания врагов
        public IReadOnlyList<BaseEnemyView> Spawn(
            EnemySpawnContext context)
        {
            // Проверяем корректность контекста спавна
            if (context == null)
                return Array.Empty<BaseEnemyView>();

            // Получаем список врагов,
            // разрешенных в текущем режиме игры
            var allowedDefinitions =
                FilterDefinitions(context);

            // Если подходящих врагов нет,
            // завершаем работу
            if (allowedDefinitions.Count == 0)
                return Array.Empty<BaseEnemyView>();

            // Проверяем, существуют ли точки спавна
            if (context.SpawnPoints == null ||
                context.SpawnPoints.Count == 0)
            {
                return Array.Empty<BaseEnemyView>();
            }

            // Ищем описание ближнего врага
            var meleeDefinition =
                FindDefinition(
                    allowedDefinitions,
                    EnemySpawnCombatRole.Melee,
                    isBoss: false);

            // Ищем описание дальнего врага
            var rangedDefinition =
                FindDefinition(
                    allowedDefinitions,
                    EnemySpawnCombatRole.Ranged,
                    isBoss: false);

            // Получаем список только обычных врагов
            // (без боссов)
            var regularDefinitions =
                FilterRegularDefinitions(
                    allowedDefinitions);

            // Минимальное количество врагов
            int minCount =
                context.Settings.MinSpawnCount;

            // Максимальное количество врагов
            int maxCount =
                context.Settings.MaxSpawnCount;

            // Количество обязательных типов врагов,
            // которые должны присутствовать
            int requiredTypes = 0;

            if (meleeDefinition != null)
                requiredTypes++;

            if (rangedDefinition != null)
                requiredTypes++;

            // Выбираем случайное количество врагов
            int targetCount =
                context.RandomSource.Next(
                    minCount,
                    maxCount + 1);

            // Гарантируем,
            // что обязательные типы поместятся
            targetCount =
                Mathf.Max(
                    targetCount,
                    requiredTypes);

            // Дополнительно ограничиваем диапазон
            targetCount =
                Mathf.Clamp(
                    targetCount,
                    minCount,
                    maxCount);

            // Формируем план спавна
            var spawnPlan =
                BuildSpawnPlan(
                    targetCount,
                    meleeDefinition,
                    rangedDefinition,
                    regularDefinitions,
                    context.RandomSource);

            // Если создать никого нельзя,
            // завершаем работу
            if (spawnPlan.Count == 0)
                return Array.Empty<BaseEnemyView>();

            // Выбираем случайные точки появления
            var selectedPoints =
                _spawnPointSelector.Select(
                    context.SpawnPoints,
                    spawnPlan.Count,
                    context.RandomSource);

            // Создаем врагов согласно сформированному плану
            return SpawnFromPlan(
                spawnPlan,
                selectedPoints,
                context);
        }

                // Формирует список врагов, которые должны появиться
        private List<EnemySpawnDefinition> BuildSpawnPlan(
            int targetCount,
            EnemySpawnDefinition meleeDefinition,
            EnemySpawnDefinition rangedDefinition,
            List<EnemySpawnDefinition> regularDefinitions,
            System.Random random)
        {
            // Создаем список будущего плана спавна
            var plan =
                new List<EnemySpawnDefinition>(
                    targetCount);

            // Если существует ближний враг,
            // обязательно добавляем его в план
            if (meleeDefinition != null)
                plan.Add(meleeDefinition);

            // Если существует дальний враг
            // и его еще нет в плане,
            // добавляем его
            if (rangedDefinition != null &&
                !ContainsDefinition(
                    plan,
                    rangedDefinition))
            {
                plan.Add(rangedDefinition);
            }

            // Пока не достигли нужного количества врагов,
            // добавляем случайных обычных врагов
            while (plan.Count < targetCount)
            {
                // Если доступных врагов больше нет,
                // прекращаем заполнение
                if (regularDefinitions.Count == 0)
                    break;

                // Выбираем случайного врага
                var definition =
                    _definitionSelector.Select(
                        regularDefinitions,
                        random);

                // Если выбрать никого не удалось,
                // прекращаем цикл
                if (definition == null)
                    break;

                // Добавляем выбранного врага в план
                plan.Add(definition);
            }

            // Возвращаем готовый план спавна
            return plan;
        }

        // Создает врагов согласно сформированному плану
        private List<BaseEnemyView> SpawnFromPlan(
            List<EnemySpawnDefinition> spawnPlan,
            IReadOnlyList<SpawnPoint> spawnPoints,
            EnemySpawnContext context)
        {
            // Список созданных врагов
            var spawnedEnemies =
                new List<BaseEnemyView>(
                    spawnPlan.Count);

            // Количество создаваемых врагов
            // ограничивается количеством точек спавна
            int pointCount =
                Mathf.Min(
                    spawnPlan.Count,
                    spawnPoints.Count);

            // Создаем врагов по одному
            for (int i = 0; i < pointCount; i++)
            {
                // Берем описание врага
                var definition =
                    spawnPlan[i];

                // Берем соответствующую точку появления
                var spawnPoint =
                    spawnPoints[i];

                // Генерируем уникальный идентификатор
                string enemyId =
                    BuildEnemyId(definition);

                // Создаем врага через фабрику
                var enemy =
                    _enemyFactory.Create(
                        definition,
                        spawnPoint.Position,
                        spawnPoint.Rotation,
                        enemyId,
                        context.Player,
                        context.RandomSource);

                // Если создание прошло успешно,
                // добавляем врага в список
                if (enemy != null)
                    spawnedEnemies.Add(enemy);
            }

            // Возвращаем список всех созданных врагов
            return spawnedEnemies;
        }

                // Возвращает список только обычных врагов
        // (исключая боссов)
        private static List<EnemySpawnDefinition>
            FilterRegularDefinitions(
            List<EnemySpawnDefinition> definitions)
        {
            // Создаем список результата
            var result =
                new List<EnemySpawnDefinition>();

            // Перебираем все описания врагов
            foreach (var definition in definitions)
            {
                // Пропускаем пустые ссылки
                // и описания боссов
                if (definition == null ||
                    definition.IsBoss)
                {
                    continue;
                }

                // Добавляем обычного врага
                result.Add(definition);
            }

            // Возвращаем список обычных врагов
            return result;
        }

        // Ищет первое описание врага
        // с указанной боевой ролью
        private static EnemySpawnDefinition
            FindDefinition(
            List<EnemySpawnDefinition> definitions,
            EnemySpawnCombatRole role,
            bool isBoss)
        {
            // Перебираем все описания
            foreach (var definition in definitions)
            {
                // Пропускаем пустые ссылки
                if (definition == null)
                    continue;

                // Проверяем,
                // совпадает ли тип (босс / обычный враг)
                if (definition.IsBoss != isBoss)
                    continue;

                // Проверяем боевую роль
                if (ResolveCombatRole(definition) == role)
                    return definition;
            }

            // Подходящий враг не найден
            return null;
        }

        // Определяет боевую роль врага
        private static EnemySpawnCombatRole
            ResolveCombatRole(
            EnemySpawnDefinition definition)
        {
            // Для босса роль уже задана
            // в описании
            if (definition.IsBoss)
                return definition.CombatRole;

            // Получаем префаб врага
            var prefab = definition.Prefab;

            if (prefab != null)
            {
                // Если на префабе есть
                // RangedEnemyBehaviour,
                // считаем врага дальним
                if (prefab.GetComponent<
                    RangedEnemyBehaviour>() != null)
                {
                    return EnemySpawnCombatRole.Ranged;
                }

                // Если есть EnemyBehaviour,
                // считаем врага ближним
                if (prefab.GetComponent<
                    EnemyBehaviour>() != null)
                {
                    return EnemySpawnCombatRole.Melee;
                }
            }

            // Если определить автоматически
            // не удалось,
            // используем значение из описания
            return definition.CombatRole;
        }

        // Проверяет,
        // содержится ли описание врага в плане
        private static bool ContainsDefinition(
            List<EnemySpawnDefinition> plan,
            EnemySpawnDefinition definition)
        {
            return plan.Contains(definition);
        }

        // Возвращает только тех врагов,
        // которые разрешены
        // в текущем режиме игры
        private List<EnemySpawnDefinition>
            FilterDefinitions(
            EnemySpawnContext context)
        {
            // Создаем список результата
            var result =
                new List<EnemySpawnDefinition>();

            // Если каталог отсутствует,
            // возвращаем пустой список
            if (context.Catalog?.Definitions == null)
                return result;

            // Перебираем все описания врагов
            foreach (var definition in
                context.Catalog.Definitions)
            {
                // Пропускаем пустые ссылки
                if (definition == null)
                    continue;

                // Добавляем только тех врагов,
                // которые разрешены
                // для текущего режима игры
                if (definition.IsAllowedIn(
                    context.GameMode))
                {
                    result.Add(definition);
                }
            }

            // Возвращаем список допустимых врагов
            return result;
        }

        // Генерирует уникальный идентификатор врага
        private string BuildEnemyId(
            EnemySpawnDefinition definition)
        {
            // Увеличиваем счетчик
            _spawnCounter++;

            // Формируем ID
            // Например: Goblin_3
            return
                $"{definition.IdPrefix}_{_spawnCounter}";
        }
    }
}
