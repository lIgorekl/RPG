using Gameplay.Combat.Boss;
using UnityEngine;

namespace Presentation.Combat
{
    // Управляет визуальными эффектами стихии босса.
    // Изменяет цвет всех ParticleSystem в соответствии
    // с выбранной стихией.
    public class BossElementVisualController : MonoBehaviour
    {
        // Список систем частиц.
        // Если не заполнен вручную,
        // будет найден автоматически.
        [SerializeField] private ParticleSystem[] particleSystems;

        // Применяет параметры выбранной стихии
        public void ApplyElement(BossElementConfig config)
        {
            // Если конфигурация отсутствует,
            // ничего не делаем
            if (config == null)
                return;

            // Получаем цвет выбранной стихии
            var color = config.ParticleColor;

            // Перебираем все системы частиц
            foreach (var particleSystem in ResolveParticleSystems())
            {
                // Пропускаем пустые ссылки
                if (particleSystem == null)
                    continue;

                // Получаем модуль Main системы частиц
                var main = particleSystem.main;

                // Меняем цвет новых создаваемых частиц
                main.startColor = color;

                // Получаем компонент,
                // отвечающий за отображение частиц
                var renderer =
                    particleSystem.GetComponent<ParticleSystemRenderer>();

                // Если материал существует,
                // меняем и его цвет
                if (renderer != null &&
                    renderer.material != null)
                {
                    renderer.material.color = color;
                }

                // Если система частиц еще не запущена,
                // запускаем её
                if (!particleSystem.isPlaying)
                    particleSystem.Play();
            }
        }

        // Возвращает массив систем частиц
        private ParticleSystem[] ResolveParticleSystems()
        {
            // Если список задан вручную,
            // используем его
            if (particleSystems != null &&
                particleSystems.Length > 0)
            {
                return particleSystems;
            }

            // Иначе автоматически ищем все ParticleSystem
            // у объекта и его дочерних объектов
            return GetComponentsInChildren<ParticleSystem>(true);
        }
    }
}