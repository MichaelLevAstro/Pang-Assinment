using System;
using System.Collections.Generic;
using Domains.Game.GameLoop.Enemies;
using Domains.Game.GameLoop.Enemies.Models;
using Domains.Game.GameLoop.Enemies.Signals;
using Domains.Game.GameLoop.Environment;
using Domains.Game.GameLoop.Popups;
using Domains.Game.GameLoop.Weapons;
using Domains.Game.Player;
using Domains.Game.Weapons;
using Domains.Global_Domain.Assets;
using Domains.Global_Domain.Cameras;
using Domains.Global_Domain.Log;
using Domains.Global_Domain.Popups;
using Domains.Global_Domain.Score;
using Domains.Global_Domain.Signals;
using Domains.Global_Domain.TTInput;
using Global_Domain;
using UnityEngine;
using Zenject;

namespace Domains.Game.GameLoop
{
    public class GameLoopController : MonoBehaviour, IInitializable, IDisposable
    {
        private const float ARBITRARY_UPPER_LIMIT = 30;
        
        [Inject] private LevelService _levelService;
        [Inject] private AssetsService _assetsService;
        [Inject] private CameraService _cameraService;
        [Inject] private EnemyDeployService _enemyDeployService;
        [Inject] private BulletPoolController _bulletProvider;
        [Inject] private PlayerView.Factory _playerFactory;
        [Inject] private SignalBus _signalBus;
        [Inject] private IEnemiesModelRead _enemiesModelRead;
        [Inject] private IEnemiesModelWrite _enemiesModelWrite;
        [Inject] private ScoreService _scoreService;
        [Inject] private PopupService _popupService;
        [Inject] private GameInputService _gameInputService;
        [Inject] private LogService _logService;

        public Vector2? PlayerSize 
        {
            get
            {
                if (_currentPlayer == null)
                {
                    return null;
                }

                return _currentPlayer.ColliderSize;
            }
        }
        
        public Vector2? PlayerPosition
        {
            get
            {
                if (_currentPlayer == null)
                {
                    return null;
                }

                return _currentPlayer.transform.position;
            }
        }

        private readonly Dictionary<string, EnemyView> _enemies = new Dictionary<string, EnemyView>();
        private PlayerView _currentPlayer;
        private LevelView _currentlyLoadedLevel;
        private bool _isGameRunning;

        public void Initialize()
        {
            LoadGame(_levelService.GetCurrentLevel());
        }

        public void Dispose()
        {
            UnsubscribeSignals();
        }

        public void InitializeNewEnemy(EnemyView enemyView, EnemyData enemyData, string uniqueId)
        {
            if (enemyView == null || uniqueId == null)
            {
                return;
            }
            
            _enemies.Add(uniqueId, enemyView);
            enemyView.Setup(enemyData);
            enemyView.Initialize(() => OnEnemyHitByBullet(uniqueId));
        }
        
        public void OnPlayerPause()
        {
            StopGame();

            var popupData = new GamePopupData()
            {
                PopupPath = AssetsPathConstants.GAME_POPUP_PREFAB_PATH,
                IsPause = true,
                PlayerScore = _scoreService.GetPlayerScore()
            };
            
            _popupService.ShowPopup(popupData);
        }
        
        public void OnPlayerWon()
        {
            StopGame();

            var popupData = new GamePopupData()
            {
                PopupPath = AssetsPathConstants.GAME_POPUP_PREFAB_PATH,
                DidPlayerWinLevel = true,
                PlayerScore = _scoreService.GetPlayerScore()
            };
            
            _popupService.ShowPopup(popupData);
        }

        public void ResumeGame()
        {
            StartGame();
        }

        private void LoadGame(int level)
        {
            bool shouldLoadLevel = _currentlyLoadedLevel == null || _levelService.GetCurrentLevel() != level;
            if (shouldLoadLevel)
            {
                if(!LoadLevel(level))
                {
                    // Debug couldnt load level
                    _logService.LogError("Couldn't load level");
                    return;
                }   
            }

            UnsubscribeSignals(); // Safe guard
            SubscribeSignals();
            CreatePlayer();
            DeployEnemies();
            _isGameRunning = true;
        }
        
        private void OnLoadNextLevel()
        {
            var nextLevel = _levelService.GetNextLevel();
            LoadGame(nextLevel);
        }

        private void SubscribeSignals()
        {
            _signalBus.Subscribe<FireBulletSignal>(OnFireBullet);
            _signalBus.Subscribe<RetryLevelSignal>(OnPlayerRetry);
            _signalBus.Subscribe<StartNextLevelSignal>(OnLoadNextLevel);
            _gameInputService.SubscribeToArrowEvents(UpdatePlayerPosition);
            _gameInputService.SubscribeToBackButtonEvents(OnPlayerPause);
        }

        private void UnsubscribeSignals()
        {
            _signalBus.TryUnsubscribe<FireBulletSignal>(OnFireBullet);
            _signalBus.TryUnsubscribe<RetryLevelSignal>(OnPlayerRetry);
            _signalBus.TryUnsubscribe<StartNextLevelSignal>(OnLoadNextLevel);
            _gameInputService.UnsubscribeToArrowEvents(UpdatePlayerPosition);
            _gameInputService.UnsubscribeToBackButtonEvents(OnPlayerPause);
        }

        private void StopGame()
        {
            _isGameRunning = false;
            if(_enemies.Count > 0)
            {
                foreach (var enemy in _enemies.Values)
                {
                    enemy.EnableMotion(false);
                }
            }
            _currentPlayer.SetPlayerMoving(false);
            _signalBus.TryUnsubscribe<FireBulletSignal>(OnFireBullet);
            _gameInputService.UnsubscribeToBackButtonEvents(OnPlayerPause);
        }
        
        private void StartGame()
        {
            _isGameRunning = true;
            foreach (var enemy in _enemies.Values)
            {
                enemy.EnableMotion(true);
            }
            _signalBus.Subscribe<FireBulletSignal>(OnFireBullet);
            _gameInputService.SubscribeToBackButtonEvents(OnPlayerPause);
        }
        
        private void CreatePlayer()
        {
            if (_currentPlayer != null)
            {
                ResetPlayer();
                return;
            }
            
            _currentPlayer = _playerFactory.Create(null);
            _currentPlayer.Initialize(OnPlayerDied);
            var screenBounds = _cameraService.GetScreenBoundaries();
            if (screenBounds.HasValue)
            {
                var horizontalBound = screenBounds.Value.x;
                _currentPlayer.SetPlayerBound(horizontalBound);
            }
            ResetPlayer();
        }

        private void UpdatePlayerPosition(ArrowClickData clickData)
        {
            if (_currentPlayer == null || !_isGameRunning)
            {
                return;
            }
            _currentPlayer.SetMoveDirection((int)clickData.ArrowType);
            _currentPlayer.SetPlayerMoving(clickData.IsDown);
        }

        private void ResetPlayer()
        {
            if (_currentPlayer == null || _levelService == null)
            {
                return;
            }
            _currentPlayer.transform.position = _levelService.GetPlayerSpawnPoint(_currentPlayer.ColliderSize);
        }

        private bool LoadLevel(int level)
        {
            var levelData = _assetsService.GetLevelPrefab(level);
            if (levelData == null)
            {
                _logService.LogError("Failed to load Level");
                return false;
            }
            
            if (_currentlyLoadedLevel != null)
            {
                Destroy(_currentlyLoadedLevel.gameObject);
            }
            _currentlyLoadedLevel = _levelService.LoadLevel(levelData);
            return true;
        }

        private void DeployEnemies()
        {
            if (_enemies != null && _enemies.Count > 0)
            {
                var enemyIds = new List<string>(_enemies.Keys);
                foreach (var keyValuePair in enemyIds)
                {
                    DestroyEnemy(keyValuePair);
                }
            }

            var deployedEnemies = _enemyDeployService.DeployLevelEnemies();
            if (deployedEnemies == null || deployedEnemies.Count <= 0)
            {
                return;
            }
            foreach (var keyValuePair in deployedEnemies)
            {
                var enemyData = _enemiesModelRead.GetEnemyById(keyValuePair.Key);
                if (enemyData?.EnemyData == null)
                {
                    _logService.LogError("Couldn't find level data");
                    // Maybe enemy deploy service should be the only one to handle the enemies model
                    _enemiesModelWrite.RemoveEnemy(keyValuePair.Key);
                    continue;
                }
                
                InitializeNewEnemy(keyValuePair.Value, enemyData.EnemyData, keyValuePair.Key);
            }
        }

        private void OnEnemyHitByBullet(string enemyId)
        {
            var hitEnemyModelData = _enemiesModelRead.GetEnemyById(enemyId);
            var deathPosition = Vector3.zero;
            if (_enemies.TryGetValue(enemyId, out var enemyView))
            {
                deathPosition = enemyView.transform.position;
                DestroyEnemy(enemyId);
            }
            
            _signalBus.Fire(new EnemyHitSignal(hitEnemyModelData, deathPosition));
        }

        private void DestroyEnemy(string id)
        {
            if (_enemies.TryGetValue(id, out var enemyView))
            {
                Destroy(enemyView.gameObject);
                _enemies.Remove(id);
                _enemiesModelWrite.RemoveEnemy(id);
            }
        }

        private void OnPlayerDied()
        {
            StopGame();
            
            var popupData = new GamePopupData()
            {
                PopupPath = AssetsPathConstants.GAME_POPUP_PREFAB_PATH,
                DidPlayerWinLevel = false,
                PlayerScore = _scoreService.GetPlayerScore()
            };
            
            _popupService.ShowPopup(popupData);
        }

        private void OnPlayerRetry()
        {
            LoadGame(_levelService.GetCurrentLevel());
        }
        
        private void OnFireBullet()
        {
            var bulletView = _bulletProvider.CreateBullet(_currentPlayer.BulletSpawnPosition);
            var screenBounds = _cameraService.GetScreenBoundaries();
            bulletView.Initialize(screenBounds?.y ?? ARBITRARY_UPPER_LIMIT, OnDestroyBullet);
        }

        private void OnDestroyBullet(BulletView view)
        {
            _bulletProvider.DestroyBullet(view);
        }
    }
}
