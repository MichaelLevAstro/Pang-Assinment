using System.Collections.Generic;
using Domains.Global_Domain.Log;
using Zenject;

namespace Domains.Game.GameLoop.Enemies.Models
{
    public class EnemiesModel : IEnemiesModelWrite, IEnemiesModelRead
    {
        [Inject] private LogService _logService;
        
        private Dictionary<string, EnemyModelData> _currentlyLiveEnemies = new Dictionary<string, EnemyModelData>();

        public void AddEnemy(EnemyModelData enemyModelData, string uniqueId)
        {
            if (_currentlyLiveEnemies.ContainsKey(uniqueId))
            {
                _logService.LogError("tried adding an existing enemy");
                return;
            }
            
            _currentlyLiveEnemies.Add(uniqueId, enemyModelData);
        }

        public void RemoveEnemy(string uniqueId)
        {
            if (!_currentlyLiveEnemies.ContainsKey(uniqueId))
            {
                _logService.LogError("couldn't remove enemy, doesn't exist in model.");
                return;
            }
            
            _currentlyLiveEnemies.Remove(uniqueId);
        }

        public EnemyModelData GetEnemyById(string uniqueId)
        {
            if (_currentlyLiveEnemies.ContainsKey(uniqueId))
            {
                return _currentlyLiveEnemies[uniqueId];
            }
            
            _logService.LogError("tried fetching a non existing enemy");
            return null;
        }

        public int GetCurrentlyLiveEnemiesAmount()
        {
            return _currentlyLiveEnemies.Count;
        }
    }
}