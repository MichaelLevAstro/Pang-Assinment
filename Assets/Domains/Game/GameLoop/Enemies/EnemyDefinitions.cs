using JM.LinqFaster;
using UnityEngine;

namespace Domains.Game.GameLoop.Enemies
{
    [CreateAssetMenu(fileName = "EnemyDefinitions", menuName = "Enemies/Enemy Definitions")]
    public class EnemyDefinitions : ScriptableObject
    {
        [SerializeField] private EnemyData[] _enemies;

        public EnemyData GetEnemyByType(EnemyType enemyType)
        {
            return _enemies.SingleOrDefaultF(enemyData => enemyType == enemyData.EmemyType);
        }
    }
}