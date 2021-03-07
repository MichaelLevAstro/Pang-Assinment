namespace Domains.Game.GameLoop.Enemies.Models
{
    public interface IEnemiesModelWrite
    {
        void AddEnemy(EnemyModelData enemyModelData, string uniqueId);
        void RemoveEnemy(string uniqueId);
    }
}