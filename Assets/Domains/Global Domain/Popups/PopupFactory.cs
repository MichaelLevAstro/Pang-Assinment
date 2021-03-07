using Domains.Global_Domain.Assets;
using Domains.Global_Domain.Log;
using Zenject;

namespace Domains.Global_Domain.Popups
{
    public class PopupFactory : IFactory<PopupData, BasePopupView>
    {
        [Inject] private AssetsService _assetsService;
        [Inject] private DiContainer _container;
        [Inject] private DestroyService _destroyService;
        [Inject] private LogService _logService;
        [Inject] private PopupService _popupService;
        
        public BasePopupView Create(PopupData param)
        {
            var popupPrefab = _assetsService.LoadObjectFromPath(param.PopupPath);
            if (popupPrefab == null)
            {
                _logService.LogError("Base popup prefab not found!!");
                return null;
            }
            
            var popupObject = _container.InstantiatePrefab(popupPrefab, _popupService._PopupsParentTransform);;
            if (popupObject == null)
            {
                _logService.LogError("Could not instantiate popup!");
                return null;
            }
            
            var popupView = popupObject.GetComponent<BasePopupView>();
            if (popupView == null)
            {
                _logService.LogError("Popup Has No base View!");
                _destroyService.DestroyObject(popupObject.gameObject);
                return null;
            }
            
            popupView.SetupPopup(param);
            return popupView;
        }
    }
}