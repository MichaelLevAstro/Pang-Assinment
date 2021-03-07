using Domains.Global_Domain;
using Domains.Global_Domain.Assets;
using Domains.Global_Domain.Log;
using Zenject;

namespace Domains.Game.GameLoop.Weapons
{
    public class BulletFactory : IFactory<BulletData, BulletView>
    {
        [Inject] private DiContainer _container;
        [Inject] private AssetsService _assetsService;
        [Inject] private DestroyService _destroyService;
        [Inject] private LogService _logService;

        public BulletView Create(BulletData param)
        {
            var bulletPrefab = _assetsService.GetBulletPrefab();
            if (bulletPrefab == null)
            {
                _logService.LogError("Bullet prefab not found!!");
                return null;
            }
            
            var bulletObject = _container.InstantiatePrefab(bulletPrefab);
            if (bulletObject == null)
            {
                _logService.LogError("Could not instantiate bullet!");
                return null;
            }
            
            var bulletView = bulletObject.GetComponent<BulletView>();
            if (bulletView == null)
            {
                _logService.LogError("Bullet Has No View!");
                _destroyService.DestroyObject(bulletObject.gameObject);
                return null;
            }
            
            bulletView.Setup(param);
            return bulletView;
        }
    }
}