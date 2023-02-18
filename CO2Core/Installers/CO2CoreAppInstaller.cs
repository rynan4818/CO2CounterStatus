using Zenject;
using CO2Core.Models;

namespace CO2Core.Installers
{
    internal class CO2CoreAppInstaller : Installer
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<CO2CoreManager>().AsSingle().NonLazy();
        }
    }
}
