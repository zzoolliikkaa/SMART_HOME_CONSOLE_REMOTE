namespace Smart_Home_Console_Remote.Models.Interfaces;

public interface IDimmable
{
    int Brightness { get; }
    void SetBrightness(int value);
}