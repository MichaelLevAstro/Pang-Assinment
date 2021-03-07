using Domains.Global_Domain.Animations;
using Global_Domain.Animations;
using TMPro;
using UnityEngine;
using Zenject;

namespace Domains.Global_Domain.Messages
{
    public class MessageView : MonoBehaviour
    {
        [SerializeField] private AnimationView _animationView;
        [SerializeField] private TextMeshPro _messageText;
        
        public void OnMessageReceived(MessageData messageData)
        {
            _messageText.text = messageData.Message;
            _animationView.Play(AnimationsConstants.MESSAGE_APPEAR_ANIMATION).Then(() =>
            {
                Destroy(gameObject);
            });
        }

        public class Factory : PlaceholderFactory<MessageData, MessageView> { }
    }
}
