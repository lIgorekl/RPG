namespace Core.Gameplay
{
    // Простая система кулдауна.
    // Используется для атак, способностей и других таймеров.
    public class Cooldown
    {
        private float _duration;
        private float _timer;

        // Активен ли кулдаун
        public bool IsActive => _timer > 0f;

        // Прогресс кулдауна (для UI)
        public float Progress =>
            IsActive ? _timer / _duration : 0f;

        public Cooldown(float duration)
        {
            _duration = duration;
        }

        // Запуск кулдауна
        public void Start()
        {
            _timer = _duration;
        }

        // Обновление таймера
        public void Update(float deltaTime)
        {
            if (_timer <= 0f)
                return;

            _timer -= deltaTime;

            if (_timer < 0f)
                _timer = 0f;
        }
    }
}