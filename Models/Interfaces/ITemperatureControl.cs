public interface ITemperatureControl
{
    double TargetCelsius { get; }
    public void SetTarget(double celsius);
}