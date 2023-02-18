using CO2Core.Interfaces;
using CO2Counter.Configuration;
using CountersPlus.Counters.Custom;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace CO2Counter
{
    internal class CO2Counter : BasicCustomCounter
    {
        private readonly ICO2CoreManager _manager;
        private float x = PluginConfig.Instance.OffsetX;
        private float y = PluginConfig.Instance.OffsetY;
        private float z = PluginConfig.Instance.OffsetZ;
        private TMP_Text _counterCO2;
        public CO2Counter(ICO2CoreManager manager)
        {
            this._manager = manager;
        }
        public override void CounterInit()
        {
            this._manager.OnCO2Changed += this.OnCO2Changed;
            if (PluginConfig.Instance.EnableLabel)
            {
                var label = CanvasUtility.CreateTextFromSettings(Settings, new Vector3(x, y, z));
                label.text = PluginConfig.Instance.LabelText;
                label.fontSize = PluginConfig.Instance.LabelFontSize;
            }
            _counterCO2 = CanvasUtility.CreateTextFromSettings(Settings, new Vector3(x, y - 0.2f, z));
            _counterCO2.lineSpacing = -26;
            _counterCO2.fontSize = PluginConfig.Instance.FigureFontSize;
            _counterCO2.alignment = TextAlignmentOptions.Top;
            OnCO2Changed(0, 0, 0);
        }
        public override void CounterDestroy()
        {
            this._manager.OnCO2Changed -= this.OnCO2Changed;
        }
        private void OnCO2Changed(int co2, double hum, double tmp)
        {
            _counterCO2.text = $"{co2}<size=50%>ppm</size>  {StringFormat(tmp)}<size=50%>℃</size>  {StringFormat(hum)}<size=50%>%</size>";
        }
        private string StringFormat(double distance)
        {
            return $"{distance.ToString($"F{PluginConfig.Instance.DecimalPrecision}", CultureInfo.InvariantCulture)}";
        }
    }
}
