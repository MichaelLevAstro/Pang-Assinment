using System;
using Domains.Game.Weapons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Domains.Global_Domain.TTInput
{
    public class GameInputService : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Button _fireButton;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private PointerClickView _moveLeftButton;
        [SerializeField] private PointerClickView _moveRightButton;
        
        [Inject] private SignalBus _signalBus;
        
        private bool _isInDragEvent;
        private event Action<ArrowClickData> OnArrowEventAction;
        private event Action OnBackButtonEventAction;

        private void Awake()
        {
            // Couldn't figure out which implementation i liked better, so made multiple choices
            // In the real world, that's a slap on the wrist
            _fireButton.onClick.AddListener( () => _signalBus.Fire<FireBulletSignal>());
            _pauseButton.onClick.AddListener(OnPauseButtonClicked);
            _moveLeftButton.OnArrowEvent += OnArrowEvent;
            _moveRightButton.OnArrowEvent += OnArrowEvent;
        }

        private void Update()
        {
            // The association from key to action could better be set as a scriptable
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _signalBus.Fire<FireBulletSignal>();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                OnArrowEventAction?.Invoke(new ArrowClickData{ ArrowType = ArrowType.Right, IsDown = true });
            }
            
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                OnArrowEventAction?.Invoke(new ArrowClickData{ ArrowType = ArrowType.Left, IsDown = true });
            }
            
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                OnArrowEventAction?.Invoke(new ArrowClickData{ ArrowType = ArrowType.Right, IsDown = false });
            }
            
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                OnArrowEventAction?.Invoke(new ArrowClickData{ ArrowType = ArrowType.Left, IsDown = false });
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnBackButtonEventAction?.Invoke();   
            }
        }

        public void OnPauseButtonClicked()
        {
            OnBackButtonEventAction?.Invoke();
        }

        public void OnArrowEvent(ArrowClickData clickData)
        {
            OnArrowEventAction?.Invoke(clickData);
        }

        public void SubscribeToArrowEvents(Action<ArrowClickData> actionToInvoke)
        {
            OnArrowEventAction += actionToInvoke;
        }
        
        public void UnsubscribeToArrowEvents(Action<ArrowClickData> actionToInvoke)
        {
            OnArrowEventAction -= actionToInvoke;
        }
        
        public void SubscribeToBackButtonEvents(Action actionToInvoke)
        {
            OnBackButtonEventAction += actionToInvoke;
        }
        
        public void UnsubscribeToBackButtonEvents(Action actionToInvoke)
        {
            OnBackButtonEventAction -= actionToInvoke;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isInDragEvent = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isInDragEvent = false;
        }
    }
}
