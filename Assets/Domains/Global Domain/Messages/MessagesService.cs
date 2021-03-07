using UnityEngine;
using Zenject;

namespace Domains.Global_Domain.Messages
{
    public class MessagesService : MonoBehaviour
    {
        [Inject] private MessageView.Factory _messageFactory;
        
        public void ShowMessage(string message, Vector3 atPosition)
        {
            var messageView = _messageFactory.Create(new MessageData(message));
            var messageViewTransform = messageView.transform;
            messageViewTransform.SetParent(transform);
            messageViewTransform.position = atPosition;
        }
    }
}
