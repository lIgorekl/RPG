namespace Core.Gameplay
{
    // Универсальная система кулдауна
    // Используется для ограничения частоты атак и способностей
    public class Cooldown
    {
        // Длительность кулдауна в секундах
        private float _duration;

        // Оставшееся время до окончания кулдауна
        private float _timer;

        // Проверяет активен ли кулдаун в данный момент
        public bool IsActive => _timer > 0f;

        // Возвращает прогресс кулдауна для UI
        public float Progress =>
            IsActive ? _timer / _duration : 0f;

        public Cooldown(float duration)
        {
            _duration = duration;
        }

        // Запускает кулдаун
        public void Start()
        {
            _timer = _duration;
        }

        // Обновляет таймер кулдауна
        public void Update(float deltaTime)
        {
            if (_timer <= 0f)
                return;

            // Уменьшаем оставшееся время
            _timer -= deltaTime;

            // Не допускаем отрицательных значений
            if (_timer < 0f)
                _timer = 0f;
        }
    }
}