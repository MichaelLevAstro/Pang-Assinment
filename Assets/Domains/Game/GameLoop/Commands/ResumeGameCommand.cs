using Domains.Global_Domain.Signals;
using Global_Domain.Commands;
using Zenject;

namespace Domains.Game.GameLoop.Commands
{
    public class ResumeGameCommand : BaseCommand<ResumeGameSignal>
    {
        [Inject] private GameLoopController _gameLoopController;
        
        protected override void OnCallbackAction(ResumeGameSignal signal = null)
        {
            _gameLoopController.ResumeGame();
        }
    }
}