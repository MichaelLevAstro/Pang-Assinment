using Domains.Game.GameLoop.Commands;
using Domains.Game.GameLoop.Enemies;
using Domains.Game.GameLoop.Enemies.Commands;
using Domains.Game.GameLoop.Enemies.Models;
using Domains.Game.GameLoop.Enemies.Signals;
using Domains.Game.GameLoop.Environment;
using Domains.Game.GameLoop.Score;
using Domains.Game.GameLoop.Weapons;
using Domains.Game.GameLoop.Weapons.Models;
using Domains.Game.Player;
using Domains.Game.Weapons;
using Domains.Global_Domain;
using Domains.Global_Domain.Assets;
using Domains.Global_Domain.Cameras;
using Domains.Global_Domain.Log;
using Domains.Global_Domain.Messages;
using Domains.Global_Domain.Player;
using Domains.Global_Domain.Popups;
using Domains.Global_Domain.Score;
using Domains.Global_Domain.Signals;
using Domains.Global_Domain.TTInput;
using Global_Domain;
using UnityEngine;
using Zenject;

namespace Domains.Game.GameLoop
{
    public class GameWorldInstaller : MonoInstaller<GameWorldInstaller>
    {
        [SerializeField] private GameInputService gameInputService;
        [SerializeField] private ScoreController scoreController;
        [SerializeField] private PopupService _popupService;
        [SerializeField] private CameraService _cameraService;
        [SerializeField] private BulletPoolController _bulletPoolController;
        
        public override void InstallBindings()
        {
            DeclareSignals();
            BindModels();
            BindServices();
            InstallFactories();
            BindControllers();
            BindCommands();
        }
        
        private void InstallFactories()
        {
            Container.BindFactory<PlayerData, PlayerView, PlayerView.Factory>().FromFactory<PlayerFactory>();
            Container.BindFactory<EnemyData, EnemyView, EnemyView.Factory>().FromFactory<EnemyFactory>();
            Container.BindFactory<LevelData, LevelView, LevelView.Factory>().FromFactory<LevelFactory>();
            Container.BindFactory<BulletData, BulletView, BulletView.Factory>().FromFactory<BulletFactory>();
            Container.BindFactory<MessageData, MessageView, MessageView.Factory>().FromFactory<MessageFactory>();
            Container.BindFactory<PopupData, BasePopupView, BasePopupView.Factory>().FromFactory<PopupFactory>();
        }

        private void BindServices()
        {
            Container.BindInstance(gameInputService).AsSingle().NonLazy();
            Container.Bind<SceneService>().AsSingle().NonLazy();
            Container.Bind<AssetsService>().AsSingle().NonLazy();
            Container.Bind<LevelService>().AsSingle().NonLazy();
            Container.Bind<ScoreService>().AsSingle().NonLazy();
            Container.Bind<LogService>().AsSingle().NonLazy();
            Container.Bind<DestroyService>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.Bind<CoroutineService>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.Bind<EnemyDeployService>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.Bind<MessagesService>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.BindInstance(_popupService).AsSingle().NonLazy();
            Container.BindInstance(_cameraService).AsSingle().NonLazy();
        }

        private void BindModels()
        {
            Container.BindInterfacesAndSelfTo<EnemiesModel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<WeaponsModel>().AsSingle().NonLazy();
        }

        private void BindControllers()
        {
            Container.BindInterfacesAndSelfTo<GameLoopController>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.Bind<ScoreController>().FromInstance(scoreController).AsSingle().NonLazy();
            Container.BindInstance(_bulletPoolController).AsSingle().NonLazy();
        }

        private void DeclareSignals()
        {
            Container.DeclareSignal<FireBulletSignal>();
            Container.DeclareSignal<BulletCollisionSignal>();
            Container.DeclareSignal<EnemyHitSignal>();
            Container.DeclareSignal<ScoreChangedSignal>();
            Container.DeclareSignal<QuitToMainMenuSignal>();
            Container.DeclareSignal<RetryLevelSignal>();
            Container.DeclareSignal<ResumeGameSignal>();
            Container.DeclareSignal<StartNextLevelSignal>();
        }

        private void BindCommands()
        {
            Container.BindInterfacesAndSelfTo<EnemyHitCommand>().AsCached().Lazy();
            Container.BindInterfacesAndSelfTo<QuitToMainMenuCommand>().AsCached().Lazy();
            Container.BindInterfacesAndSelfTo<ResumeGameCommand>().AsCached().Lazy();
        }
    }
}