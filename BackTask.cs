using TelegramBotBase;
using TelegramBotBase.Builder;
using TelegramBotBase.Commands;
namespace TgBotPractice;

public class BackTask
{
    private async Task Init(string token)
    {
        var bBase = BotBaseBuilder.Create()
        .WithAPIKey(token)
        .DefaultMessageLoop()
        .WithStartForm(typeof(StartForm))
        .NoProxy()
        .CustomCommands(a =>
        {
            a.Start("Starts the bot");
            a.Add("open", "open");
        })
        .NoSerialization()
        .UseRussian()
        .UseThreadPool()
        .Build();
        bBase.BotCommand += async (s, ru) =>
        {
            switch (ru.Command)
            {
                case "/start":
                    await ru.Device.ActiveForm.NavigateTo(new StartForm());
                    break;
                case "/open":
                    await ru.Device.ActiveForm.NavigateTo(new OpenForm());
                    break;
            }
        };
        await bBase.UploadBotCommands();
        await bBase.Start();
    }

    public async Task Start()
    {
        Console.WriteLine("Введите токен: ");
        string? tok = Console.ReadLine();
       await Init(tok ?? "invalid");
       Console.WriteLine("Сервис запущен.");
       Console.ReadLine();
    }
}