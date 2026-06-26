using System;

namespace App.Events
{
    // Контракт шины игровых событий.
    // Определяет способы подписки,
    // отписки и публикации событий.
    public interface IGameEventBus
    {
        // Подписывает обработчик
        // на событие указанного типа
        void Subscribe<T>(Action<T> handler);

        // Отписывает обработчик
        // от события указанного типа
        void Unsubscribe<T>(Action<T> handler);

        // Публикует событие,
        // уведомляя всех подписчиков
        void Publish<T>(T eventData);
    }
}