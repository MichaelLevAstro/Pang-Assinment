using Global_Domain.Models;
using Zenject;

namespace Domains.Global_Domain
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallInstallers();
        }

        private void InstallInstallers()
        {
            SignalBusInstaller.Install(Container);
            ModelsInstaller.Install(Container);
        }
    }
}