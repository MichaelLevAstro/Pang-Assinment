using Domains.Global_Domain.Signals;
using Global_Domain;
using Global_Domain.Commands;
using Zenject;

namespace Domains.Game.GameLoop.Commands
{
    public class QuitToMainMenuCommand : BaseCommand<QuitToMainMenuSignal>
    {
        [Inject] private SceneService _sceneService;
        
        protected override void OnCallbackAction(QuitToMainMenuSignal signal = null)
        {
            _sceneService.LoadScene(SceneNamesConstants.MAIN_MENU_SCENE);
        }
    }
}