using System.Threading;
using Zenject;
using System;
using CO2Core.Util;
using CO2Core.Configuration;
using CO2Core.Interfaces;

namespace CO2Core.Models
{
    public class CO2CoreManager : IInitializable, IDisposable, ICO2CoreManager

    {
        public int CO2 { get; private set; } = 0;
        public double HUM { get; private set; } = 0;
        public double TMP { get; private set; } = 0;
        public event CO2CoreChangedEventHandler OnCO2Changed;
        private bool _disposedValue;
        private Thread _thread;
        public SerialPortController port = new SerialPortController();
        public void Initialize()
        {
            if (PluginConfig.Instance.Port == "NONE" || !PluginConfig.Instance.Enable)
                return;
            if (port.PortOpen(PluginConfig.Instance.Port))
                port.Send("STA");
            else
            {
                Plugin.Log?.Error("OPEN ERROR");
                return;
            }
            this._thread = new Thread(new ThreadStart(() =>
            {
                while (!this._disposedValue)
                {
                    // UD-CO2Sフォーマット:CO2=3097,HUM=45.7,TMP=26.9
                    int co2;
                    double hum;
                    double tmp;
                    var co2data = port.ReadData();
                    var co2Idx = co2data.IndexOf("CO2=");
                    var co2End = co2data.IndexOf(',');
                    var humIdx = co2data.IndexOf("HUM=");
                    var humEnd = co2data.LastIndexOf(',');
                    var tmpIdx = co2data.IndexOf("TMP=");
                    var tmpEnd = co2data.Length;
                    if (co2Idx > -1 && humIdx > -1 && tmpIdx > -1)
                    {
                        if(int.TryParse(co2data.Substring(co2Idx + 4, co2End - co2Idx - 4), out co2) &&
                           double.TryParse(co2data.Substring(humIdx + 4, humEnd - humIdx - 4), out hum) &&
                           double.TryParse(co2data.Substring(tmpIdx + 4, tmpEnd - tmpIdx - 4), out tmp))
                        {
                            //温度、湿度補正
                            //https://twitter.com/sakura_sakusaku/status/1625133221922103300
                            tmp += PluginConfig.Instance.TempOffset;
                            hum = Math.Round(hum * PluginConfig.Instance.HumCorrection, 1, MidpointRounding.AwayFromZero);
                            HMMainThreadDispatcher.instance?.Enqueue(() =>
                            {
                                UpdateCO2(co2, hum, tmp);
                            });
                        }
                    }
                }
            }));
            this._thread.Start();
        }
        private void UpdateCO2(int co2, double hum, double tmp)
        {
            this.CO2 = co2;
            this.HUM = hum;
            this.TMP = tmp;
            this.OnCO2Changed?.Invoke(co2, hum, tmp);
#if DEBUG
            Plugin.Log?.Debug($"{co2}ppm:{hum}%:{tmp}C");
#endif
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                this._disposedValue = true;
                if (disposing)
                {
                    port.Send("STP");
                    port.PortClose();
#if DEBUG
                    Plugin.Log.Info("CO2CoreManager Dispose");
#endif
                }
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
