namespace CO2Core.Interfaces
{
    public interface ICO2CoreManager
    {
        int CO2 { get; }
        double HUM { get; }
        double TMP { get; }
        event CO2CoreChangedEventHandler OnCO2Changed;
    }
    public delegate void CO2CoreChangedEventHandler(int co2, double hum, double tmp);
}
