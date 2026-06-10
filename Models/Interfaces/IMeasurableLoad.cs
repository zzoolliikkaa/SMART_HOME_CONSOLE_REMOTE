public interface IMeasurableLoad
{
    double CurrentWatts { get; }
    double TotalWh { get; }
    void ResetEnergy();
}