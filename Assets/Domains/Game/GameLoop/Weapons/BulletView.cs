using System;
using Domains.Game.Weapons;
using UnityEngine;
using Zenject;

namespace Domains.Game.GameLoop.Weapons
{
    public class BulletView : MonoBehaviour
    {
        [SerializeField] private BulletMoveController _bulletMoveController;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private TrailRenderer _trailRenderer;

        public void Setup(BulletData bulletData)
        {
            if (bulletData == null)
            {
                return;
            }
            _bulletMoveController.Setup(bulletData.Speed);
        }

        public void Initialize(float upperBoundary, Action<BulletView> onBulletDestroyed)
        {
            _bulletMoveController.Initialize(upperBoundary, () => onBulletDestroyed(this));
        }

        private void OnEnable()
        {
            _trailRenderer.Clear();
        }

        // Get the top of the bullet since that's all the data needed (for this type of bullet at least)
        public Vector2 GetBulletPosition()
        {
            var position = transform.position;
            return new Vector2(position.x, position.y + _renderer.size.y / 2f);
        }

        public void DisableBullet()
        {
            gameObject.SetActive(false);
        }

        public class Factory : PlaceholderFactory<BulletData, BulletView> { } 
    }
}