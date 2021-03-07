using Domains.Global_Domain.Assets;
using Domains.Global_Domain.Log;
using Zenject;

namespace Domains.Global_Domain.Messages
{
    public class MessageFactory : IFactory<MessageData, MessageView>
    {
        [Inject] private AssetsService _assetsService;
        [Inject] private DiContainer _container;
        [Inject] private DestroyService _destroyService;
        [Inject] private LogService _logService;
    
        public MessageView Create(MessageData param)
        {
            var messagePrefab = _assetsService.GetMessagePrefab();
            if (messagePrefab == null)
            {
                _logService.LogError("Message prefab not found!!");
                return null;
            }
            
            var messageObject = _container.InstantiatePrefab(messagePrefab);
            if (messageObject == null)
            {
                _logService.LogError("Could not instantiate message!");
                return null;
            }
            
            var messageView = messageObject.GetComponent<MessageView>();
            if (messageView == null)
            {
                _logService.LogError("Message Has No View!");
                _destroyService.DestroyObject(messageObject.gameObject);
                return null;
            }
            
            messageView.OnMessageReceived(param);
            return messageView;
        }
    }
}
