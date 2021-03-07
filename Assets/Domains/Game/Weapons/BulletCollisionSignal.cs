using Domains.Game.GameLoop.Enemies;
using Domains.Global_Domain.Signals;

namespace Domains.Game.Weapons
{
    public class BulletCollisionSignal : Signal
    {
        public EnemyView EnemyView;

        public BulletCollisionSignal(EnemyView enemyView)
        {
            EnemyView = enemyView;
        }
    }
}