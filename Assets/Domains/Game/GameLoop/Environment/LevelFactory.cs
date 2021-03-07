using Domains.Global_Domain;
using Domains.Global_Domain.Assets;
using Domains.Global_Domain.Cameras;
using Domains.Global_Domain.Log;
using Zenject;

namespace Domains.Game.GameLoop.Environment
{
    public class LevelFactory : IFactory<LevelData, LevelView>
    {
        [Inject] private DiContainer _container;
        [Inject] private AssetsService _assetsService;
        [Inject] private CameraService _cameraService;
        [Inject] private DestroyService _destroyService;
        [Inject] private LogService _logService;
        
        public LevelView Create(LevelData param)
        {
            if (param == null)
            {
                _logService.LogError("Tried to create level, but no data was passed!");
                return null;
            }
            
            var levelPrefab = _assetsService.LoadObjectFromPath(param.LevelPath);
            if (levelPrefab == null)
            {
                _logService.LogError("Base level prefab not found!!");
                return null;
            }

            var levelObject = _container.InstantiatePrefab(levelPrefab);
            if (levelObject == null)
            {
                _logService.LogError("Could not instantiate level!");
                return null;
            }
            
            var levelView = levelObject.GetComponent<LevelView>();
            if (levelView == null)
            {
                levelView = levelObject.AddComponent<LevelView>();
            }
            levelView.Initialize(_cameraService.GetScreenBoundaries(), _cameraService.GetMainCameraOrthographicSize());
            return levelView;
        }
    }
}