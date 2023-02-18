using CO2Core.Interfaces;
using HttpSiraStatus;
using HttpSiraStatus.Enums;
using HttpSiraStatus.Interfaces;
using HttpSiraStatus.Util;
using System;
using UnityEngine;
using Zenject;

namespace HttpCO2Status
{
    internal class HttpCO2StatusController : IInitializable, IDisposable
    {
        private bool _disposedValue;
        private readonly ICO2CoreManager _manager;
        private readonly IStatusManager _statusManager;
        public void OnCO2Changed(int co2, double hum, double tmp)
        {
            var rootObj = new JSONObject();
            rootObj["CO2"] = co2;
            rootObj["Temperature"] = tmp;
            rootObj["Humidity"] = hum;
            this._statusManager.OtherJSON["CO2Core"] = rootObj;
            this._statusManager.EmitStatusUpdate(ChangedProperty.Other, BeatSaberEvent.Other);
        }

        public HttpCO2StatusController(ICO2CoreManager manager, IStatusManager statusManager)
        {
            this._manager = manager;
            this._statusManager = statusManager;
        }
        public void Initialize()
        {
            this._manager.OnCO2Changed += this.OnCO2Changed;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    this._manager.OnCO2Changed -= this.OnCO2Changed;
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
