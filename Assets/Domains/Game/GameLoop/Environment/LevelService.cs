using Domains.Game.GameLoop.Environment.Models;
using UnityEngine;
using Zenject;

namespace Domains.Game.GameLoop.Environment
{
    public class LevelService
    {
        [Inject] private LevelView.Factory _gridFactory;
        [Inject] private ILevelModelRead _levelModelRead;
        [Inject] private ILevelModelWrite _levelModelWrite;

        private LevelView _currentLevelView;
        private LevelData _currentLevelData;

        public LevelView LoadLevel(LevelData levelData)
        {
            _currentLevelView = _gridFactory.Create(levelData);
            _currentLevelData = levelData;
            _levelModelWrite.SetLevel(levelData.Level);
            return _currentLevelView;
        }

        public int GetNextLevel()
        {
            return GetCurrentLevel() + 1;
        }

        public LevelData GetCurrentLevelData()
        {
            return _currentLevelData;
        }

        public int GetCurrentLevel()
        {
            return _levelModelRead.GetLevel();
        }

        public Vector2[] GetEnemySpawnPoints()
        {
            if (_currentLevelView == null)
            {
                return null;
            }
            return _currentLevelView.GetEnemySpawnPositions();
        }

        public Vector2 GetPlayerSpawnPoint(Vector2 playerColliderSize)
        {
            if (_currentLevelView == null)
            {
                return Vector2.zero;
            }
            return _currentLevelView.GetPlayerSpawnPoint(playerColliderSize);
        }
    }
}