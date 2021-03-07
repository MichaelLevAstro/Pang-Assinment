using System;
using System.Collections.Generic;
using Domains.Game.GameLoop.Environment;
using Domains.Global_Domain.Assets;
using Domains.Global_Domain.Cameras;
using JM.LinqFaster;
using UnityEngine;
using Zenject;

namespace Domains.Game.GameLoop.Weapons
{
    public class BulletPoolController : MonoBehaviour
    {
        [SerializeField] private int _maxBulletsCount;
        [SerializeField] private bool _shouldPreloadBullet;
        
        [Inject] private LevelService _levelService;
        [Inject] private AssetsService _assetsService;
        [Inject] private BulletView.Factory _bulletFactory;
        [Inject] private CameraService _cameraService;

        private List<BulletView> _bulletsQueue = new List<BulletView>();

        private void Awake()
        {
            if (!_shouldPreloadBullet)
            {
                return;
            }

            var bulletView = CreateBullet(Vector3.zero);
            bulletView.gameObject.SetActive(false);
            _bulletsQueue.Add(bulletView);
        }

        public BulletView CreateBullet(Vector3 position)
        {
            var bulletView = _bulletsQueue.FirstOrDefaultF(b => !b.gameObject.activeInHierarchy);
            var bulletData = _assetsService.GetBulletData(_levelService.GetCurrentLevel());
            if (bulletView == null)
            {
                if (_bulletsQueue.Count >= _maxBulletsCount)
                {
                    bulletView = _bulletsQueue.FirstOrDefaultF(b => b.gameObject.activeInHierarchy);
                }
                else
                {
                    bulletView = _bulletFactory.Create(bulletData);
                    _bulletsQueue.Add(bulletView);
                }
            }

            var bulletTransform = bulletView.transform;
            bulletTransform.SetParent(transform);
            bulletTransform.position = position;
            bulletView.gameObject.SetActive(true);
            return bulletView;
        }

        public void DestroyBullet(BulletView bulletView)
        {
            bulletView.DisableBullet();
        }

        public List<BulletView> GetCurrentlyActiveBullets()
        {
            return _bulletsQueue.WhereF(bullet => bullet.gameObject.activeInHierarchy);
        }
    }
}