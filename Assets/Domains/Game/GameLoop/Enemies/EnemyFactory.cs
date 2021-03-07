using Domains.Global_Domain;
using Domains.Global_Domain.Assets;
using Domains.Global_Domain.Log;
using Zenject;

namespace Domains.Game.GameLoop.Enemies
{
    public class EnemyFactory : IFactory<EnemyData, EnemyView>
    {
        [Inject] private AssetsService _assetsService;
        [Inject] private DiContainer _container;
        [Inject] private DestroyService _destroyService;
        [Inject] private LogService _logService;
        
        public EnemyView Create(EnemyData param)
        {
            var enemyBasePrefab = _assetsService.GetEnemyPrefab(param.Path);
            if (enemyBasePrefab == null)
            {
                _logService.LogError("Base Enemy prefab not found!!");
                return null;
            }
            
            var enemyObject = _container.InstantiatePrefab(enemyBasePrefab);;
            if (enemyObject == null)
            {
                _logService.LogError("Could not instantiate enemy!");
                return null;
            }
            
            var enemyView = enemyObject.GetComponent<EnemyView>();
            if (enemyView == null)
            {
                _logService.LogError("Enemy Has No View!");
                _destroyService.DestroyObject(enemyObject.gameObject);
                return null;
            }
            
            enemyView.Setup(param);
            return enemyView;
        }
    }
}