using UnityEngine;
using Zenject;

namespace Domains.Global_Domain.Popups
{
    // Super primitive popup service, this ideally should handle popup queues
    // And enable sorting by priority
    public class PopupService : MonoBehaviour
    {
        [Inject] private BasePopupView.Factory _popupFactory;

        public Transform _PopupsParentTransform => transform;
        
        public void ShowPopup(PopupData popupData)
        {
            var popupView = _popupFactory.Create(popupData);
            var popupViewTransform = popupView.transform as RectTransform;
            if (popupViewTransform == null)
            {
                return;
            }
            
            popupViewTransform.SetParent(transform);
        }
    }
}
