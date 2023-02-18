using HttpCO2Status.Installers;
using IPA;
using SiraUtil.Zenject;
using IPALogger = IPA.Logging.Logger;

namespace HttpCO2Status
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }

        [Init]
        public Plugin(IPALogger logger, Zenjector zenjector)
        {
            Instance = this;
            Log = logger;
            Log.Info("HttpCO2Status initialized.");
            zenjector.Install<HCSAppInstaller>(Location.App);
        }

        [OnStart]
        public void OnApplicationStart()
        {
            Plugin.Log.Info("OnApplicationStart");
        }

        [OnExit]
        public void OnApplicationQuit()
        {

        }

    }
}
