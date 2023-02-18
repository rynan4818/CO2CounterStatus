using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Settings;
using BeatSaberMarkupLanguage.ViewControllers;
using CO2Core.Configuration;
using CO2Core.Util;
using System.Collections.Generic;
using Zenject;

namespace CO2Core.Views
{
    public class ConfigViewController : BSMLAutomaticViewController, IInitializable
    {
        public string ResourceName => string.Join(".", GetType().Namespace, GetType().Name);
        public ConfigViewController()
        {
            PortChoices.Add("NONE");
            foreach (var port in SerialPortController.GetPort())
                PortChoices.Add(port);
        }
        public void Initialize()
        {
            BSMLSettings.instance.AddSettingsMenu("CO2Core", this.ResourceName, this);
        }
        [UIValue("Enable")]
        public bool Enable
        {
            get => PluginConfig.Instance.Enable;
            set => PluginConfig.Instance.Enable = value;
        }
        [UIValue("port-set")]
        public string PortSet
        {
            get => PluginConfig.Instance.Port;
            set => PluginConfig.Instance.Port = value;
        }
        [UIValue("port-choices")]
        public List<object> PortChoices { get; set; } = new List<object>();
    }
}
