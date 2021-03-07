using Domains.Game.GameLoop.Weapons;
using Domains.Global_Domain.Log;
using JM.LinqFaster;
using UnityEngine;
using Zenject;

namespace Domains.Game.GameLoop.Environment
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D _ground;
        [SerializeField] private BoxCollider2D _ceiling;
        [SerializeField] private BoxCollider2D _rightWall;
        [SerializeField] private BoxCollider2D _leftWall;
        [SerializeField] private Transform[] _enemySpawnPoints;
        [SerializeField] private Transform _defaultPlayerSpawnPoint;

        [Inject] private LogService _logService;

        private Vector2? _screenBounds;
        
        public void Initialize(Vector2? screenBounds, float orthographicSize)
        {
            if (screenBounds == null)
            {
                // LOG error cant set boundary positions
                _logService.LogError("Level has no boundaries");
                return;
            }

            _screenBounds = screenBounds;
            InitializesBoundObjects(screenBounds.Value, orthographicSize);
        }

        public Vector2[] GetEnemySpawnPositions()
        {
            return _enemySpawnPoints.SelectF(spawnPoints => new Vector2(spawnPoints.position.x, spawnPoints.position.y));
        }

        public Vector2 GetPlayerSpawnPoint(Vector2 playerColliderSize)
        {
            if (!_screenBounds.HasValue)
            {
                return _defaultPlayerSpawnPoint.position;
            }

            return new Vector2(0, (_screenBounds.Value.y - playerColliderSize.y * 2) * -1);
        }

        private void InitializesBoundObjects(Vector2 screenBounds, float orthographicSize)
        {
            var worldScreenHeight = orthographicSize * 2f;
            var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

            _ground.transform.localScale = Vector3.one;
            var groundBounds = _ground.bounds;
            var groundPosition = new Vector3(0, (screenBounds.y + groundBounds.size.y / 2) * -1, 0);
            var groundNewScale = new Vector2(worldScreenWidth, 1);
            SetBoundaryParameters(_ground, groundPosition, groundNewScale);
            
            _ceiling.transform.localScale = Vector3.one;
            var ceilingBounds = _ceiling.bounds;
            var ceilingPosition = new Vector3(0, screenBounds.y + ceilingBounds.size.y / 2, 0);
            var ceilingNewScale = new Vector2(worldScreenWidth, 1);
            SetBoundaryParameters(_ceiling, ceilingPosition, ceilingNewScale);
            
            _rightWall.transform.localScale = Vector3.one;
            var rightWallBounds = _rightWall.bounds;
            var rightWallPosition = new Vector3((screenBounds.x  + rightWallBounds.size.x / 2) * -1, 0, 0);
            var rightWallNewScale = new Vector2(1, worldScreenHeight);
            SetBoundaryParameters(_rightWall, rightWallPosition, rightWallNewScale);
            
            _leftWall.transform.localScale = Vector3.one;
            var leftWallBounds = _leftWall.bounds;
            var leftWallPosition = new Vector3(screenBounds.x + leftWallBounds.size.x / 2, 0, 0);
            var leftWallNewScale = new Vector2(1, worldScreenHeight);
            SetBoundaryParameters(_leftWall, leftWallPosition, leftWallNewScale);
        }

        private void SetBoundaryParameters(BoxCollider2D spriteRenderer, Vector2 position, Vector3 scale)
        {
            var rendererTransform = spriteRenderer.transform;
            spriteRenderer.size = scale;
            rendererTransform.position = position;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var bulletView = other.gameObject.GetComponentInChildren<BulletView>();
            if (bulletView != null)
            {
                Destroy(bulletView);
            }
        }

        public class Factory : PlaceholderFactory<LevelData, LevelView> { }
    }
}
