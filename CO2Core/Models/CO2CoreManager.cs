using System.Threading;
using Zenject;
using System;
using CO2Core.Util;
using CO2Core.Configuration;
using CO2Core.Interfaces;
using System.IO;
using IPA.Utilities.Async;

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
        private readonly CancellationTokenSource connectionClosed = new CancellationTokenSource();
        public void Initialize()
        {
            if (!PluginConfig.Instance.Enable)
                return;
            if (PluginConfig.Instance.Port != "NONE" && port.PortOpen(PluginConfig.Instance.Port))
                port.Send("STA");
            else
            {
                Plugin.Log?.Error("COM PORT OPEN ERROR");
                CheckStandardApp();
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
                            var et0 = 6.1078 * Math.Pow(10.0, 7.5 * tmp / (tmp + 237.3)); //補正前温度の飽和水蒸気圧
                            tmp += PluginConfig.Instance.TempOffset;                      //温度補正
                            var et1 = 6.1078 * Math.Pow(10.0, 7.5 * tmp / (tmp + 237.3)); //補正後温度の飽和水蒸気圧
                            hum *= et0 / et1;                                             //湿度補正
                            if (hum > 99.9)
                                hum = 99.9;
                            hum = Math.Round(hum, 1, MidpointRounding.AwayFromZero);
                            UnityMainThreadTaskScheduler.Factory.StartNew(() =>
                            {
                                UpdateCO2(co2, hum, tmp);
                            }, this.connectionClosed.Token);
                        }
                    }
                }
            }));
            this._thread.Start();
        }
        private void CheckStandardApp()
        {
            var ioDataAppPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "I-O_DATA");
            if (!Directory.Exists(ioDataAppPath))
                return;
            this._thread = new Thread(new ThreadStart(() =>
            {
                while (!this._disposedValue)
                {
                    var files = Directory.EnumerateFiles(ioDataAppPath, "log*.txt", SearchOption.AllDirectories);
                    var maxTime = DateTime.MinValue;
                    var maxTimeFile = "";
                    foreach (string file in files)
                    {
                        var filetime = File.GetLastWriteTime(file);
                        if (maxTime < filetime)
                        {
                            maxTime = filetime;
                            maxTimeFile = file;
                        }
                    }
                    if (!File.Exists(maxTimeFile))
                        break;
                    if (DateTime.Now - maxTime <= new TimeSpan(0, 0, 50))
                    {
                        String text = "";
                        try
                        {
                            using (var fs = new FileStream(maxTimeFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                using (var sr = new StreamReader(fs))
                                {
                                    String line;
                                    while ((line = sr.ReadLine()) != null)
                                        text = line;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Plugin.Log?.Error($"{maxTimeFile}:OpenError:{ex}");
                        }
                        try
                        {
                            //標準アプリログフォーマット 2023-02-18 00:18:44.006 +09:00 [INF] ,PORT:COM5, CO2:1488, TMP:20.9, HUM:34.8, 
                            var csv = text.Split(',');
                            var co2st = csv[2].Substring(csv[2].IndexOf(":") + 1);
                            var tmpst = csv[3].Substring(csv[3].IndexOf(":") + 1);
                            var humst = csv[4].Substring(csv[4].IndexOf(":") + 1);
                            int co2;
                            double hum;
                            double tmp;
                            if (int.TryParse(co2st, out co2) && double.TryParse(tmpst, out tmp) && double.TryParse(humst, out hum))
                            {
                                UnityMainThreadTaskScheduler.Factory.StartNew(() =>
                                {
                                    UpdateCO2(co2, hum, tmp);
                                }, this.connectionClosed.Token);
                            }
                            else
                                Plugin.Log?.Error($"{maxTimeFile}:ParseError");
                        }
                        catch (Exception ex)
                        {
                            Plugin.Log?.Error($"{maxTimeFile}:ParseExceptionError:{ex}");
                        }
                    }
                    var diffTime = (DateTime.Now - maxTime).TotalMilliseconds;
                    var sleepTime = 60000.0;
                    if (diffTime < 30000.0)
                        sleepTime = 30000.0 - diffTime;
                    else if (diffTime <= 60000.0)
                        sleepTime = 90000.0 - diffTime;
                    else
                        sleepTime = 60000.0;
                    Thread.Sleep((int)sleepTime);
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
                    this.connectionClosed.Cancel();
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
