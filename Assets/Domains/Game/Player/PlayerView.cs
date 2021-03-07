using System;
using Domains.Game.GameLoop.Enemies;
using Domains.Global_Domain.Player;
using UnityEngine;
using Zenject;

namespace Domains.Game.Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Transform _bulletSpawnPosition;
        [SerializeField] private PlayerMovementController _movementController;
        [SerializeField] private CapsuleCollider2D _playerCollider;
        
        public Vector3 BulletSpawnPosition => _bulletSpawnPosition.position;
        public Vector2 ColliderSize => _playerCollider.size;

        private Action _onEnemyCollision;

        private float _currentMoveDirection;
        private bool _canMove;
        
        public void Initialize(Action onEnemyCollision)
        {
            _onEnemyCollision = onEnemyCollision;
        }
        
        public void SetPlayerBound(float bound)
        {
            _movementController.SetPlayerBound(bound - _playerCollider.size.x * transform.localScale.x / 2f);
        }

        public void SetPlayerMoving(bool enable)
        {
            _canMove = enable;
        }

        public void SetMoveDirection(float direction)
        {
            _currentMoveDirection = direction;
        }

        public void Update()
        {
            if (!_canMove)
            {
                return;
            }
            _movementController.UpdatePosition(_currentMoveDirection);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var enemyView = other.gameObject.GetComponentInChildren<EnemyView>();
            if (enemyView != null)
            {
                _onEnemyCollision?.Invoke();
            }
        }
        
        public class Factory : PlaceholderFactory<PlayerData, PlayerView> { }
    }
}