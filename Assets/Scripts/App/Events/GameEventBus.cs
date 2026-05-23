using System;
using System.Collections.Generic;

namespace App.Events
{
    public sealed class GameEventBus : IGameEventBus
    {
        private readonly Dictionary<Type, Delegate> _handlers = new();

        public void Subscribe<T>(Action<T> handler)
        {
            var type = typeof(T);

            if (_handlers.TryGetValue(type, out var existing))
                _handlers[type] = Delegate.Combine(existing, handler);
            else
                _handlers[type] = handler;
        }

        public void Unsubscribe<T>(Action<T> handler)
        {
            var type = typeof(T);

            if (!_handlers.TryGetValue(type, out var existing))
                return;

            var updated = Delegate.Remove(existing, handler);

            if (updated == null)
                _handlers.Remove(type);
            else
                _handlers[type] = updated;
        }

        public void Publish<T>(T eventData)
        {
            if (_handlers.TryGetValue(typeof(T), out var handlers))
                (handlers as Action<T>)?.Invoke(eventData);
        }
    }
}
