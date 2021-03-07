using UnityEngine;

namespace Domains.Game.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        
        private float _playerLimit;

        public void SetPlayerBound(float bound)
        {
            _playerLimit = bound;
        }
        
        public void UpdatePosition(float horizontalAxis)
        {
            var originalPosition = transform.position;
            transform.Translate(new Vector3(horizontalAxis * _moveSpeed, 0, 0) * Time.deltaTime);
            if (transform.position.x >= _playerLimit ||
                transform.position.x <= -_playerLimit)
            {
                transform.position = originalPosition;
            }
        }
    }
}
