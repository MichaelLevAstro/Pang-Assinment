using Domains.Game.GameLoop.Environment.Models;
using Domains.Global_Domain.Models.Player;
using Zenject;

namespace Global_Domain.Models
{
    public class ModelsInstaller :Installer<ModelsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerModel>().AsSingle().Lazy();
            Container.BindInterfacesAndSelfTo<LevelModel>().AsSingle().NonLazy();
        }
    }
}