using System;
using UnityEngine;

namespace Domains.Game.Weapons
{
    public class BulletMoveController : MonoBehaviour
    {
        private float _bulletSpeed = 50;
        private float _upperBoundary = float.MaxValue;

        public Action OnBulletDestroyed { get; private set; }

        public void Setup(float bulletSpeed)
        {
            _bulletSpeed = bulletSpeed;
        }

        public void Initialize(float upperBoundary, Action onBulletDestroyed)
        {
            _upperBoundary = upperBoundary;
            OnBulletDestroyed = onBulletDestroyed;
        }

        private void Update()
        {
            if (transform.position.y > _upperBoundary)
            {
                OnBulletDestroyed?.Invoke();
                return;
            }
            transform.Translate(new Vector3(0, _bulletSpeed, 0) * Time.deltaTime);
        }
    }
}