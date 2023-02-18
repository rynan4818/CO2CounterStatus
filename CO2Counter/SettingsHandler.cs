using BeatSaberMarkupLanguage.Attributes;
using CO2Counter.Configuration;

namespace CO2Counter
{
    internal class SettingsHandler
    {
        [UIValue("DecimalPrecision")]
        public int DecimalPrecision
        {
            get => PluginConfig.Instance.DecimalPrecision;
            set
            {
                PluginConfig.Instance.DecimalPrecision = value;
            }
        }

        [UIValue("EnableLabel")]
        public bool EnableLabel
        {
            get => PluginConfig.Instance.EnableLabel;
            set
            {
                PluginConfig.Instance.EnableLabel = value;
            }
        }

        [UIValue("LabelFontSize")]
        public float LabelFontSize
        {
            get => PluginConfig.Instance.LabelFontSize;
            set
            {
                PluginConfig.Instance.LabelFontSize = value;
            }
        }

        [UIValue("FigureFontSize")]
        public float FigureFontSize
        {
            get => PluginConfig.Instance.FigureFontSize;
            set
            {
                PluginConfig.Instance.FigureFontSize = value;
            }
        }

        [UIValue("OffsetX")]
        public float OffsetX
        {
            get => PluginConfig.Instance.OffsetX;
            set
            {
                PluginConfig.Instance.OffsetX = value;
            }
        }

        [UIValue("OffsetY")]
        public float OffsetY
        {
            get => PluginConfig.Instance.OffsetY;
            set
            {
                PluginConfig.Instance.OffsetY = value;
            }
        }

        [UIValue("OffsetZ")]
        public float OffsetZ
        {
            get => PluginConfig.Instance.OffsetZ;
            set
            {
                PluginConfig.Instance.OffsetZ = value;
            }
        }
    }
}
