namespace TgBotPractice;

public class Program
{
    public static async Task Main()
    {
        BackTask t = new BackTask();
        await t.Start();
    }
}