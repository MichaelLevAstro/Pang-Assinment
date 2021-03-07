using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Domains.Global_Domain.TTInput
{
    public class PointerClickView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private ArrowType _arrowType;
        
        public event Action<ArrowClickData> OnArrowEvent;

        public void OnPointerDown(PointerEventData eventData)
        {
            OnArrowEvent?.Invoke(new ArrowClickData
            {
                IsDown = true,
                ArrowType = _arrowType
            });
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnArrowEvent?.Invoke(new ArrowClickData
            {
                IsDown = false,
                ArrowType = _arrowType
            });
        }
    }
}