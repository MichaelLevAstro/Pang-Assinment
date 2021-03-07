using Domains.Global_Domain.Signals;
using Zenject;

namespace Global_Domain.Commands
{
    public abstract class BaseCommand<T> : IInitializable where T : Signal 
    {
        [Inject] private SignalBus _signalBus;

        public void Initialize()
        {
            SubscribeToSignals();
        }

        public void SubscribeToSignals()
        {
            _signalBus.Subscribe<T>(OnCallbackAction);
        }
        
        protected abstract void OnCallbackAction(T signal = null);
    }
}