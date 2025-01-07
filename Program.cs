using OpenWeatherMap.Standard;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBotPractice;

using OpenWeatherMap.Standard.Enums;
using OpenWeatherMap.Standard.Models;

using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient("7557004382:AAFSqf56fgYQWHvpg1VU6zGJxJ_mdaQnkTI", cancellationToken: cts.Token);
var me = await bot.GetMe(); // Гетаем нейм бота
WeatherCommand weatherCommand = new WeatherCommand();
StartCommand startCommand = new StartCommand();
bot.OnMessage += OnMessage;
bot.OnUpdate += OnCallbackQuery;
Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
Console.ReadLine();
cts.Cancel(); // stop the bot
var current = new Current("Key")
{
    Languages = Languages.Russian,
    FetchIcons = true,
    ForecastTimestamps = 5
};
var data = await current.GetWeatherDataByCityNameAsync("samara", "ru");
Console.WriteLine($"current temperature in samara, Ru is: {data.WeatherDayInfo.Temperature}");

// МСГ Хендл
async Task OnMessage(Message msg, UpdateType type)
{
    if (msg.Text is null) return;	// Если текс нулл - идет нахуй
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
    Console.WriteLine($"Received {type} '{msg.Text}' in {msg.Chat}"); // DEBUG data
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