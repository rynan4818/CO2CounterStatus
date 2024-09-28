using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Settings;
using CO2Core.Configuration;
using CO2Core.Util;
using System;
using System.Collections.Generic;
using Zenject;

namespace CO2Core.Views
{
    public class ConfigViewController : IInitializable, IDisposable
    {
        private bool _disposedValue;
        private readonly BSMLSettings _bSMLSettings;
        public string ResourceName => string.Join(".", GetType().Namespace, GetType().Name);
        public ConfigViewController(BSMLSettings bSMLSettings)
        {
            PortChoices.Add("NONE");
            foreach (var port in SerialPortController.GetPort())
                PortChoices.Add(port);
            this._bSMLSettings = bSMLSettings;
        }
        public void Initialize()
        {
            this._bSMLSettings.AddSettingsMenu("CO2Core", this.ResourceName, this);
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
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    this._bSMLSettings?.RemoveSettingsMenu(this);
                }
                this._disposedValue = true;
            }
        }
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

    }
}
