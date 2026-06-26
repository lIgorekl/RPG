using UnityEngine;
using App.SaveLoad;
using Gameplay.Characters;

namespace Presentation.Player
{
    // Отвечает за сохранение и загрузку данных игрока
    // Преобразует состояние игрока в SaveData и обратно
    public class PlayerSavePresenter
    {
        // Трансформ игрока для сохранения позиции
        private readonly Transform _playerTransform;

        // Игровая сущность игрока
        private readonly PlayerEntity _player;

        public PlayerSavePresenter(
            Transform playerTransform,
            PlayerEntity player)
        {
            _playerTransform = playerTransform;
            _player = player;
        }

        // Создает объект сохранения на основе текущего состояния игрока
        public PlayerSaveData CreateSaveData()
        {
            return new PlayerSaveData
            {
                // Сохраняем позицию игрока
                PositionX = _playerTransform.position.x,
                PositionY = _playerTransform.position.y,
                PositionZ = _playerTransform.position.z,

                // Сохраняем текущее и максимальное здоровье
                CurrentHp = _player.CurrentHP,
                MaxHp = _player.MaxHP
            };
        }

        // Применяет данные сохранения к игроку
        public void ApplySaveData(PlayerSaveData data)
        {
            if (data == null)
                return;

            // Восстанавливаем позицию игрока
            _playerTransform.position = new Vector3(
                data.PositionX,
                data.PositionY,
                data.PositionZ);

            // Восстанавливаем здоровье игрока
            _player.SetHP((int)data.CurrentHp);
        }
    }
}