using System;
using Domains.Game.GameLoop.Weapons;
using UnityEngine;
using Zenject;

namespace Domains.Game.GameLoop.Enemies
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private float _minimumHorizontalVelocity;
        [SerializeField] private float _minPositionHeight;
        [SerializeField] private float _bounceUpMagnitude;
        
        [Inject] private BulletPoolController _bulletProvider;
        [Inject] private GameLoopController _gameLoopController;
        
        private Action _onEnemyHit;
        private float _targetVelocity;
        private bool _isInMotion;
        private float _enemyRadius;
        private float _enemyBaseSize;
        private int _spawnDirection;

        private void Awake()
        {
            gameObject.SetActive(false);
            _enemyRadius = _renderer.bounds.size.x / 2f;
        }

        public void Setup(EnemyData data)
        {
            _targetVelocity = data.InitialVelocity;
            _renderer.sprite = data.EnemySprite;
            _enemyBaseSize = data.Size;
            _spawnDirection = data.InitialDirection;
        }

        public void Initialize(Action onEnemyHit)
        {
            _onEnemyHit = onEnemyHit;
            gameObject.SetActive(true);
            EnableMotion(true);
            _rigidbody2D.velocity = Vector2.right * (_targetVelocity * _spawnDirection);
        }

        public void EnableMotion(bool enable)
        {
            _rigidbody2D.bodyType = enable ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
            _isInMotion = enable;
        }
        
        public void SetScale(int scaleMultiplier)
        {
            transform.localScale = Vector3.one * _enemyBaseSize / scaleMultiplier;
            _enemyRadius = _renderer.bounds.size.x / 2f;
        }

        private void Update()
        {
            if (!_isInMotion)
            {
                return;
            }

            UpdateMotion();
            CheckBulletOverlap();
        }

        // Right now there's 1 bullet type and it's a pang
        // If more bullets are to be made, with different behaviours, this would move to a strategy class
        // based on the bullet type, and the enemy position and size would be passed on for a check
        private void CheckBulletOverlap()
        {
            var allActiveBulletViews = _bulletProvider.GetCurrentlyActiveBullets();
            var playerSize = _gameLoopController.PlayerSize;
            var playerPosition = _gameLoopController.PlayerPosition;
            float bottomLimit = 0f;
            if (playerSize.HasValue && playerPosition.HasValue)
            {
                bottomLimit = playerPosition.Value.y + playerSize.Value.y / 2f;
            }
            
            foreach (var bulletView in allActiveBulletViews)
            {
                var bulletPosition = bulletView.GetBulletPosition();
                var enemyPosition = transform.position;
                var enemyBottomPosition = enemyPosition.y - _enemyRadius;
                // Did the bullet top reach the bottom height of the enemy, or enemy is below player top
                if (bulletPosition.y < enemyBottomPosition || enemyBottomPosition <= bottomLimit)
                {
                    continue;
                }

                // Is the bullet top height between the bottom of the enemy and it's center
                if (bulletPosition.y >= enemyBottomPosition && bulletPosition.y <= enemyPosition.y)
                {
                    // If the distance to the center is equal or less than the radius, it's a hit
                    if (Vector3.Distance(bulletView.transform.position, enemyPosition) > _enemyRadius)
                    {
                        continue;
                    }
                }
                else
                {
                    // If the bullets y position is touching an enemy
                    var enemyBoundLeft = enemyPosition.x - _enemyRadius;
                    var enemyBoundRight = enemyPosition.x + _enemyRadius;
                    if (bulletPosition.x < enemyBoundLeft || bulletPosition.x > enemyBoundRight)
                    {
                        continue;
                    }
                }
                
                _bulletProvider.DestroyBullet(bulletView);
                _onEnemyHit?.Invoke();
                return;
            }
        }

        // Mega Lazy way of making enemies not slow down too much
        // Ideally i wouldn't use the physics engine at all but had no time to implement that
        private void UpdateMotion()
        {
            if (_rigidbody2D.velocity.x < _minimumHorizontalVelocity)
            {
                RefreshHorizontalVelocity();
            }

            if (Math.Abs(_rigidbody2D.velocity.y) < 0.5f && transform.position.y < _minPositionHeight)
            {
                RefreshVerticalVelocity();
            }
        }

        private void RefreshHorizontalVelocity()
        {
            var velocity = _rigidbody2D.velocity;
            var direction = velocity.x > 0 ? 1 : -1;
            velocity = new Vector2(direction * _targetVelocity, velocity.y);
            _rigidbody2D.velocity = velocity;
        }
        
        private void RefreshVerticalVelocity()
        {
            var velocity = _rigidbody2D.velocity;
            velocity = new Vector2(velocity.x, _targetVelocity * _bounceUpMagnitude);
            _rigidbody2D.velocity = velocity;
        }

        public class Factory : PlaceholderFactory<EnemyData, EnemyView> { }
    }
}