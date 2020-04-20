using Godot;

public static class Debug
{
    public static void Log(string message)
    {
        GD.Print(message);
    }

    public static void LogError(string message)
    {
        GD.PrintErr(message);
    }

    public static void Assert(bool condition)
    {
        if (!condition)
        {
            GD.PrintErr("Assertion failed");
        }
    }

    public static void Assert(bool condition, string message)
    {
        if (!condition)
        {
            GD.PrintErr($"Assertion failed: {message}");
        }
    }
}
