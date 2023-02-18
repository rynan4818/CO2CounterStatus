using Zenject;
using CO2Core.Views;

namespace CO2Core.Installers
{
    internal class CO2CoreMenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<ConfigViewController>().FromNewComponentAsViewController().AsSingle().NonLazy();
        }
    }
}
