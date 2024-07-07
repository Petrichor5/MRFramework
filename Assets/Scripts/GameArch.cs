using MRFramework;

public class GameArch : Architecture<GameArch>
{
    protected override void Init()
    {
        RegisterController(new CounterControl());
    }
}