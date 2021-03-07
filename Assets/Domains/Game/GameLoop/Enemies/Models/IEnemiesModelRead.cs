namespace Domains.Game.GameLoop.Enemies.Models
{
    public interface IEnemiesModelRead
    {
        EnemyModelData GetEnemyById(string uniqueId);
        int GetCurrentlyLiveEnemiesAmount();
    }
}