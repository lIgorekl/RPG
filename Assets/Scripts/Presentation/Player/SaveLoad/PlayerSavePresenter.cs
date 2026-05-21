using UnityEngine;
using App.SaveLoad;
using Gameplay.Characters;

namespace Presentation.Player
{
    // Отвечает за преобразование игрока
    // в save data и обратно.
    public class PlayerSavePresenter
    {
        private readonly Transform _playerTransform;
        private readonly PlayerEntity _player;

        public PlayerSavePresenter(
            Transform playerTransform,
            PlayerEntity player)
        {
            _playerTransform = playerTransform;
            _player = player;
        }

        // Создание save data
        public PlayerSaveData CreateSaveData()
        {
            return new PlayerSaveData
            {
                PositionX = _playerTransform.position.x,
                PositionY = _playerTransform.position.y,
                PositionZ = _playerTransform.position.z,

                CurrentHp = _player.CurrentHP,
                MaxHp = _player.MaxHP
            };
        }

        // Применение save data
        public void ApplySaveData(PlayerSaveData data)
        {
            if (data == null)
                return;

            _playerTransform.position = new Vector3(
                data.PositionX,
                data.PositionY,
                data.PositionZ);

            _player.SetHP((int)data.CurrentHp);
        }
    }
}