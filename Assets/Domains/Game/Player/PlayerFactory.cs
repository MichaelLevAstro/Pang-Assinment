using Domains.Game.Player;
using Domains.Global_Domain.Assets;
using Domains.Global_Domain.Log;
using Global_Domain;
using Zenject;

namespace Domains.Global_Domain.Player
{
    public class PlayerFactory : IFactory<PlayerData, PlayerView>
    {
        [Inject] private AssetsService _assetsService;
        [Inject] private DiContainer _container;
        [Inject] private DestroyService _destroyService;
        [Inject] private LogService _logService;
        
        public PlayerView Create(PlayerData param)
        {
            var playerPrefab = _assetsService.LoadObjectFromPath(AssetsPathConstants.PLAYER_PREFAB_PATH);
            if (playerPrefab == null)
            {
                _logService.LogError("Base Enemy prefab not found!!");
                return null;
            }
            
            var playerObject = _container.InstantiatePrefab(playerPrefab);;
            if (playerObject == null)
            {
                _logService.LogError("Could not instantiate player!");
                return null;
            }
            
            var playerView = playerObject.GetComponent<PlayerView>();
            if (playerView == null)
            {
                _logService.LogError("Player Has No View!");
                _destroyService.DestroyObject(playerView.gameObject);
                return null;
            }
            
            return playerView;
        }
    }
}