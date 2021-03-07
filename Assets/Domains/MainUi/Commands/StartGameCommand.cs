using Game.GameLoop.Signals;
using Global_Domain;
using Global_Domain.Commands;
using Zenject;

namespace Domains.MainUi.Commands
{
    public class StartGameCommand : BaseCommand<StartGameSignal>
    {
        [Inject] private SceneService _sceneService;
        
        protected override void OnCallbackAction(StartGameSignal startGameSignal)
        {
            _sceneService.LoadScene(SceneNamesConstants.GAME_SCENE);
        }
    }
}