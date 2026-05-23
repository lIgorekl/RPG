using System;

namespace App.Events
{
    public interface IGameEventBus
    {
        void Subscribe<T>(Action<T> handler);
        void Unsubscribe<T>(Action<T> handler);
        void Publish<T>(T eventData);
    }
}
