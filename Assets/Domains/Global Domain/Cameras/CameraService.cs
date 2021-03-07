using UnityEngine;

namespace Domains.Global_Domain.Cameras
{
    public class CameraService : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;

        private Vector2? _screenBounds;
        
        public Vector2? GetScreenBoundaries()
        {
            if (_screenBounds.HasValue)
            {
                return _screenBounds;
            }
            
            var screenCorner = new Vector3(Screen.width, Screen.height, _mainCamera.transform.position.z);
            _screenBounds = _mainCamera.ScreenToWorldPoint(screenCorner);

            return _screenBounds;
        }

        public float GetMainCameraOrthographicSize()
        {
            return _mainCamera.orthographicSize;
        }
    }
}
