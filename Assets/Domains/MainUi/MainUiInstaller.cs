using Domains.Global_Domain.Commands;
using Domains.Global_Domain.Signals;
using Domains.MainUi.Commands;
using Game.GameLoop.Signals;
using Global_Domain;
using Global_Domain.Commands;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Domains.MainUi
{
    public class MainUiInstaller : MonoInstaller<MainUiInstaller>
    {
        [SerializeField] private MainUiController _mainUiController;
        
        public override void InstallBindings()
        {
            // Install UI Controllers (Main Buttons, Settings etc...)
            BindServices();
            DeclareSignals();
            BindControllers();
            BindCommands();
        }
        
        private void BindServices()
        {
            Container.Bind<SceneService>().AsSingle().NonLazy();
            Container.Bind<CoroutineService>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }

        private void BindControllers()
        {
            Container.BindInstance(_mainUiController);
        }

        private void BindCommands()
        {
            Container.BindInterfacesAndSelfTo<QuitGameCommand>().AsSingle();
            Container.BindInterfacesAndSelfTo<StartGameCommand>().AsSingle();
        }

        private void DeclareSignals()
        {
            Container.DeclareSignal<QuitGameSignal>();
            Container.DeclareSignal<StartGameSignal>();
        }
    }
}
