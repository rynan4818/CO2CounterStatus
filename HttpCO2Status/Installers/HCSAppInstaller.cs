using Zenject;

namespace HttpCO2Status.Installers
{
    public class HCSAppInstaller : Installer
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<HttpCO2StatusController>().AsSingle().NonLazy();
        }
    }
}
