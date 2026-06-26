using System;
using System.Collections.Generic;

namespace App.Events
{
    // Реализация шины игровых событий.
    // Хранит подписчиков и обеспечивает
    // публикацию событий между системами.
    public sealed class GameEventBus : IGameEventBus
    {
        // Словарь обработчиков.
        // Для каждого типа события хранится
        // список подписанных методов.
        private readonly Dictionary<Type, Delegate> _handlers = new();

        // Подписывает обработчик
        // на событие указанного типа
        public void Subscribe<T>(Action<T> handler)
        {
            // Получаем тип события
            var type = typeof(T);

            // Если обработчики уже существуют,
            // добавляем новый
            if (_handlers.TryGetValue(type, out var existing))
            {
                _handlers[type] =
                    Delegate.Combine(existing, handler);
            }
            else
            {
                // Иначе создаем первую подписку
                _handlers[type] = handler;
            }
        }

        // Отписывает обработчик
        // от события указанного типа
        public void Unsubscribe<T>(Action<T> handler)
        {
            // Получаем тип события
            var type = typeof(T);

            // Если подписчиков нет,
            // ничего делать не нужно
            if (!_handlers.TryGetValue(type, out var existing))
                return;

            // Удаляем обработчик
            var updated =
                Delegate.Remove(existing, handler);

            // Если подписчиков больше не осталось,
            // удаляем запись из словаря
            if (updated == null)
            {
                _handlers.Remove(type);
            }
            else
            {
                // Иначе сохраняем обновленный список
                _handlers[type] = updated;
            }
        }

        // Публикует событие
        public void Publish<T>(T eventData)
        {
            // Ищем подписчиков данного типа события
            if (_handlers.TryGetValue(
                typeof(T),
                out var handlers))
            {
                // Вызываем всех подписчиков
                (handlers as Action<T>)
                    ?.Invoke(eventData);
            }
        }
    }
}