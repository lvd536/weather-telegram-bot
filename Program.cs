using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBotPractice.DataBase;
using TgBotPractice.Profile;
using TgBotPractice.StartService;
using TgBotPractice.WeatherService;

using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient("7557004382:AAFSqf56fgYQWHvpg1VU6zGJxJ_mdaQnkTI", cancellationToken: cts.Token);
var me = await bot.GetMe();
var weatherCommand = new WeatherCommand();
var startCommand = new StartCommand();
var profileCommand = new ProfileCommand();
bot.OnMessage += OnMessage;
bot.OnUpdate += OnCallbackQuery;
bot.OnError += OnError;
Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
Console.ReadLine();
cts.Cancel();

async Task OnMessage(Message msg, UpdateType type)
{
    if (msg.Text is null) return;
    var commandParts = msg.Text.Split(' ');
    var command = commandParts[0];
    var argument = commandParts.Length == 2 ? commandParts[1] : null;
    var defargument = commandParts.Length == 3 ? commandParts[2] : null;
    if (msg.Text.StartsWith('/'))
    {
        switch (command)
        {
            case "/start":
                await startCommand.StartCmd(bot, msg, type);
                break;
            
            case "/weather":
                if (argument is not null)
                {
                    await weatherCommand.WeatherCmd(bot, msg, type, argument);
                }
                else if (commandParts.Length >= 1 && defargument is not null)
                {
                    if (commandParts[1] == "default")
                    {
                        await DbMethods.DbDefCity(defargument, msg, bot);
                        await bot.SendMessage(msg.Chat.Id, $"Default weather is {defargument} now");
                    }
                }
                else
                {
                    await weatherCommand.WeatherCmd(bot, msg, type);
                }
                break;
            case "/profile":
                await profileCommand.ProfileCmd(bot, msg, type);
                break;
            case "/auto":
                await weatherCommand.WeatherCmdAuto(bot, msg, type);
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
            await weatherCommand.WeatherCmd(bot, update.CallbackQuery.Message ?? new Message(), update.Type);
            break;
        case "startCall":
            await startCommand.StartCmd(bot, update.CallbackQuery.Message ?? new Message(), update.Type);
            break;
    }
}

async Task OnError(Exception exception, HandleErrorSource handler)
{
    Console.WriteLine(exception);
    await Task.Delay(2000, cts.Token);
}