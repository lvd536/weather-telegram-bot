using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBotPractice;

using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient("yourApiKey", cancellationToken: cts.Token);
//TODO: 7557004382:AAFSqf56fgYQWHvpg1VU6zGJxJ_mdaQnkTI
var me = await bot.GetMe();
WeatherCommand weatherCommand = new WeatherCommand();
StartCommand startCommand = new StartCommand();
bot.OnMessage += OnMessage;
bot.OnUpdate += OnCallbackQuery;
Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
Console.ReadLine();
cts.Cancel();

async Task OnMessage(Message msg, UpdateType type)
{
    if (msg.Text is null) return;
    if (msg.Text.StartsWith('/'))
    {
        switch (msg.Text)
        {
            case "/start":
                await startCommand.StartCmd(bot, msg, type);
                break;
            
            case "/weather":
                await weatherCommand.WeatherCmd(bot, msg, type);
                break;
        }
    }
    Console.WriteLine($"[Debug] Received {type} '{msg.Text}' in {msg.Chat}");
}

async Task OnCallbackQuery(Update update)
{
    if (update.Type != UpdateType.CallbackQuery) return;
    switch (update.CallbackQuery?.Data)
    {
        case "weatherCall":
            await weatherCommand.WeatherCmd(bot, update.CallbackQuery.Message, update.Type);
            break;
        case "startCall":
            await startCommand.StartCmd(bot, update.CallbackQuery.Message, update.Type);
            break;
    }
}