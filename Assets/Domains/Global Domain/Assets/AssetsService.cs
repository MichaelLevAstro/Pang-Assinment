using System.Collections.Generic;
using Domains.Game.GameLoop.Enemies;
using Domains.Game.GameLoop.Environment;
using Domains.Game.GameLoop.Weapons;
using Domains.Global_Domain.Definitions;
using Global_Domain;
using UnityEngine;
using Zenject;

namespace Domains.Global_Domain.Assets
{
    public class AssetsService : IInitializable
    {
        private EnemyDefinitions _enemyDefinitions; // Cache to not Load more than once
        private LevelsDefinition _levelDefinitions; // Cache to not Load more than once
        private BulletDefinitions _bulletDefinitions; // Cache to not Load more than once

        private readonly Dictionary<int, LevelData> _cachedLevelData = new Dictionary<int, LevelData>();
        private readonly Dictionary<int, BulletData> _cachedBulletData = new Dictionary<int, BulletData>();
        private readonly Dictionary<EnemyType, EnemyData> _cachedEnemyData = new Dictionary<EnemyType, EnemyData>();
        private readonly Dictionary<string, GameObject> _cachedEnemyPrefabs = new Dictionary<string, GameObject>();
        
        private GameObject _cachedMessagePrefab;
        private GameObject _cachedBulletPrefab;
        
        public void Initialize()
        {
            _enemyDefinitions = Resources.Load<EnemyDefinitions>(AssetsPathConstants.ENEMY_DEFINITIONS_PATH);
            _levelDefinitions = Resources.Load<LevelsDefinition>(AssetsPathConstants.LEVEL_DEFINITIONS_PATH);
            _bulletDefinitions = Resources.Load<BulletDefinitions>(AssetsPathConstants.BULLET_DEFINITIONS_PATH);
            
        }

        public List<EnemyData> GetEnemiesByTypes(EnemyType[] enemyTypes)
        {
            if (enemyTypes == null || enemyTypes.Length <= 0)
            {
                return null;
            }
            
            var returnList = new List<EnemyData>();
            foreach (var enemyType in enemyTypes)
            {
                if (_cachedEnemyData.TryGetValue(enemyType, out var enemyData))
                {
                    returnList.Add(enemyData);
                    continue;
                }
            
                if (_enemyDefinitions == null)
                {
                    _enemyDefinitions = Resources.Load<EnemyDefinitions>(AssetsPathConstants.ENEMY_DEFINITIONS_PATH);
                }

                var newElementData = _enemyDefinitions.GetEnemyByType(enemyType);
                if (newElementData == null)
                {
                    continue;
                }
                _cachedEnemyData.Add(newElementData.EmemyType, newElementData);
                returnList.Add(newElementData);
            }
            
            return returnList;
        }

        public LevelData GetLevelPrefab(int level)
        {
            if (_cachedLevelData.TryGetValue(level, out var levelData))
            {
                return levelData;
            }
            
            if (_levelDefinitions == null)
            {
                _levelDefinitions = Resources.Load<LevelsDefinition>(AssetsPathConstants.LEVEL_DEFINITIONS_PATH);
            }

            var levelByLevel = _levelDefinitions.GetLevelByLevel(level);
            if (levelByLevel == null)
            {
                return null;
            }
            _cachedLevelData.Add(level, levelByLevel);
            return levelByLevel;
        }
        
        public BulletData GetBulletData(int level)
        {
            if (_cachedBulletData.TryGetValue(level, out var bulletData))
            {
                return bulletData;
            }
            
            if (_bulletDefinitions == null)
            {
                _bulletDefinitions = Resources.Load<BulletDefinitions>(AssetsPathConstants.BULLET_DEFINITIONS_PATH);
            }

            var bulletByLevel = _bulletDefinitions.GetBulletByLevel(level);
            if (bulletByLevel == null)
            {
                return null;
            }
            _cachedBulletData.Add(level, bulletByLevel);
            return bulletByLevel;
        }

        public GameObject GetMessagePrefab()
        {
            if (_cachedMessagePrefab == null)
            {
                _cachedMessagePrefab = LoadObjectFromPath(AssetsPathConstants.MESSAGE_PREFAB_PATH);
            }

            return _cachedMessagePrefab;
        }

        public GameObject GetBulletPrefab()
        {
            if (_cachedBulletPrefab == null)
            {
                _cachedBulletPrefab = LoadObjectFromPath(AssetsPathConstants.BULLET_PREFAB_PATH);
            }

            return _cachedBulletPrefab;
        }
        
        public GameObject GetEnemyPrefab(string path)
        {
            if (_cachedEnemyPrefabs.ContainsKey(path))
            {
                return _cachedEnemyPrefabs[path];
            }
            _cachedEnemyPrefabs.Add(path, Resources.Load<GameObject>(path));

            return _cachedEnemyPrefabs[path];
        }

        public GameObject LoadObjectFromPath(string path)
        {
            return Resources.Load<GameObject>(path);
        }
    }
}