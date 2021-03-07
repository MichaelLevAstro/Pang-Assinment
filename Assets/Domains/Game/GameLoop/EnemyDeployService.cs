using System;
using System.Collections.Generic;
using Domains.Game.GameLoop.Enemies;
using Domains.Game.GameLoop.Enemies.Models;
using Domains.Game.GameLoop.Environment;
using Domains.Global_Domain.Assets;
using Domains.Global_Domain.Log;
using Global_Domain;
using JM.LinqFaster;
using UnityEngine;
using Zenject;

namespace Domains.Game.GameLoop
{
    public class EnemyDeployService : MonoBehaviour
    {
        [Inject] private LevelService _levelService;
        [Inject] private SignalBus _signalBus;
        [Inject] private CoroutineService _coroutineService;
        [Inject] private EnemyView.Factory _enemiesFactory;
        [Inject] private AssetsService _assetsService;
        [Inject] private IEnemiesModelWrite _enemiesModelWrite;
        [Inject] private LogService _logService;

        public Dictionary<string, EnemyView> DeployLevelEnemies()
        {
            var currentLevelData = _levelService.GetCurrentLevelData();
            var enemySpawnPoints = _levelService.GetEnemySpawnPoints();
            if (enemySpawnPoints == null || enemySpawnPoints.Length <= 0)
            {
                _logService.LogError("Invalid level data, missing spawn points");
                return null;
            }

            var enemyDatas = _assetsService.GetEnemiesByTypes(currentLevelData.Enemies);
            
            var enemyUniqueIds = new Dictionary<string, EnemyView>();
            int enemyIndex = 0;
            foreach (var enemySpawnPoint in enemySpawnPoints)
            {
                if (enemyDatas.Count <= 0)
                {
                    break;
                }
                var nextEnemyToSpawn = enemyDatas[enemyIndex++];
                if (nextEnemyToSpawn == null)
                {
                    break;
                }

                var deployedEnemy = DeployEnemy(nextEnemyToSpawn, enemySpawnPoint, 0);
                enemyUniqueIds.Add(deployedEnemy.Item1, deployedEnemy.Item2);
            }

            return enemyUniqueIds;
        }

        /// <summary>
        /// Returns unique id as Item1, and enemyView as Item2
        /// </summary>
        /// <param name="position"></param>
        /// <param name="enemyData"></param>
        /// <param name="iteration"></param>
        /// <returns></returns>
        public RSG.Tuple<string, EnemyView> DeployEnemy(EnemyData enemyData, Vector3 position, ushort iteration)
        {
            var newEnemy = _enemiesFactory.Create(enemyData);
            newEnemy.transform.position = position;
            newEnemy.SetScale(iteration + 1);
                
            var enemyUniqueId = Guid.NewGuid().ToString(); 
            _enemiesModelWrite.AddEnemy(new EnemyModelData
            {
                EnemyData = enemyData.Copy(), // Copy just to be safe
                Iteration = iteration,
            }, enemyUniqueId);
            
            // Ideally we would create a new data type that replaces the Tuple
            return RSG.Tuple.Create(enemyUniqueId, newEnemy);
        }
    }
}