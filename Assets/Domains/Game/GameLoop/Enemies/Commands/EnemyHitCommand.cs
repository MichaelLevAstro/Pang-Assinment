using System;
using Domains.Game.GameLoop.Enemies.Models;
using Domains.Game.GameLoop.Enemies.Signals;
using Domains.Global_Domain;
using Domains.Global_Domain.Messages;
using Domains.Global_Domain.Score;
using Global_Domain.Commands;
using UnityEngine;
using Zenject;

namespace Domains.Game.GameLoop.Enemies.Commands
{
    public class EnemyHitCommand : BaseCommand<EnemyHitSignal>
    {
        [Inject] private DestroyService _destroyService;
        [Inject] private EnemyDeployService _enemyDeployService;
        [Inject] private ScoreService _scoreService;
        [Inject] private MessagesService _messageService;
        [Inject] private GameLoopController _gameLoopController;
        [Inject] private IEnemiesModelRead _enemiesModelRead;
        
        protected override void OnCallbackAction(EnemyHitSignal signal = null)
        {
            if (signal == null)
            {
                return;
            }

            var data = signal.EnemyModelData;
            var spawnPosition = signal.DeathPosition;
            var addedScore = data.EnemyData.KillScore * (data.Iteration + 1);
            HandleScoreEvent(addedScore, spawnPosition);

            if (data.EnemyData.MaxSplits <= data.Iteration)
            {
                if (_enemiesModelRead.GetCurrentlyLiveEnemiesAmount() > 0)
                {
                    return;
                }
            
                _gameLoopController.OnPlayerWon();
                return;
            }
            
            ushort iteration = Convert.ToUInt16(data.Iteration + 1);
            var enemy2DataCopy = data.EnemyData.Copy();
            enemy2DataCopy.FlipDirection();

            var deployedEnemy1 = _enemyDeployService.DeployEnemy(data.EnemyData, spawnPosition, iteration);
            _gameLoopController.InitializeNewEnemy(deployedEnemy1.Item2, data.EnemyData, deployedEnemy1.Item1);
            
            var deployedEnemy2 = _enemyDeployService.DeployEnemy(enemy2DataCopy, spawnPosition, iteration);
            _gameLoopController.InitializeNewEnemy(deployedEnemy2.Item2, enemy2DataCopy, deployedEnemy2.Item1);
        }

        private void HandleScoreEvent(long score, Vector3 position)
        {
            _scoreService.AddToPlayerScore(score);
            _messageService.ShowMessage(score.ToString(), position);
        }
    }
}