using Domains.Game.GameLoop.Environment.Models;
using Domains.Global_Domain.Signals;
using Game.GameLoop.Signals;
using UnityEngine;
using Zenject;

namespace Domains.MainUi
{
    public class MainUiController : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private ILevelModelWrite _levelModelWrite;

        public void OnStartGameClick()
        {
            _levelModelWrite.SetLevel(1);
            _signalBus.Fire<StartGameSignal>();
        }
        
        public void OnQuitClick()
        {
            _signalBus.Fire<QuitGameSignal>();
        }
    }
}
