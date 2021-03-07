using UnityEngine;
using Zenject;

namespace Domains.Global_Domain.Popups
{
    public class BasePopupView : MonoBehaviour
    {
        protected SignalBus _signalBus;

        [Inject]
        public void Initialize(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        protected void ClosePopup()
        {
            Destroy(gameObject);
        }

        public virtual void SetupPopup(PopupData popupData) { }
        
        public class Factory : PlaceholderFactory<PopupData, BasePopupView> { }
    }
}
