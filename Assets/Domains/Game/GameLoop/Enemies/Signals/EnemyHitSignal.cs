using Domains.Game.GameLoop.Enemies.Models;
using Domains.Global_Domain.Signals;
using UnityEngine;

namespace Domains.Game.GameLoop.Enemies.Signals
{
    public class EnemyHitSignal : Signal
    {
        public EnemyModelData EnemyModelData;
        public Vector3 DeathPosition;

        public EnemyHitSignal(EnemyModelData enemyModelData, Vector3 deathPosition)
        {
            EnemyModelData = enemyModelData;
            DeathPosition = deathPosition;
        }
    }
}