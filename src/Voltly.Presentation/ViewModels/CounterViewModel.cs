namespace Voltly.Presentation.ViewModels;

public class CounterViewModel
{
    public int CurrentCount { get; private set; }
    public void Increment() => CurrentCount++;
}