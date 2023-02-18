using System.Runtime.CompilerServices;
using IPA.Config.Stores;
using System.Linq;
using CO2Core.Util;


[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace CO2Core.Configuration
{
    internal class PluginConfig
    {
        public static PluginConfig Instance { get; set; }
        public virtual bool Enable { get; set; } = true;
        public virtual string Port { get; set; } = SerialPortController.GetPort().LastOrDefault() ?? "NONE";
        public virtual double TempOffset { get; set; } = -4.5; // 温度オフセット :https://twitter.com/sakura_sakusaku/status/1625133221922103300

        /// <summary>
        /// This is called whenever BSIPA reads the config from disk (including when file changes are detected).
        /// </summary>
        public virtual void OnReload()
        {
            // Do stuff after config is read from disk.
        }

        /// <summary>
        /// Call this to force BSIPA to update the config file. This is also called by BSIPA if it detects the file was modified.
        /// </summary>
        public virtual void Changed()
        {
            // Do stuff when the config is changed.
        }

        /// <summary>
        /// Call this to have BSIPA copy the values from <paramref name="other"/> into this config.
        /// </summary>
        public virtual void CopyFrom(PluginConfig other)
        {
            // This instance's members populated from other
        }
    }
}
