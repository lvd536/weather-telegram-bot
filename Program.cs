using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBotPractice;
using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient("7557004382:AAFSqf56fgYQWHvpg1VU6zGJxJ_mdaQnkTI", cancellationToken: cts.Token);
var me = await bot.GetMe(); // Гетаем нейм бота
WeatherCommand weatherCommand = new WeatherCommand();
bot.OnMessage += OnMessage;
bot.OnUpdate += OnCallbackQuery;
Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
Console.ReadLine();
cts.Cancel(); // stop the bot
// МСГ Хендл
async Task OnMessage(Message msg, UpdateType type)
{
    if (msg.Text is null) return;	// Если текс нулл - идет нахуй
    if (msg.Text.StartsWith('/'))
    {
        switch (msg.Text)
        {
            case "/start":
                
                break;
            
            case "/weather":
                await weatherCommand.StartCommand(bot, msg, type);
                break;
        }
    }
    Console.WriteLine($"Received {type} '{msg.Text}' in {msg.Chat}"); // DEBUG data
        //await bot.SendMessage(msg.Chat, $"{msg.From} said: {msg.Text}");
}

async Task OnCallbackQuery(Update update)
{
    if (update.Type != UpdateType.CallbackQuery) return;
    switch (update.CallbackQuery?.Data)
    {
        case "weatherCall":
            string weatherInfo = "Текущая погода: солнечно, +20°C";
            await bot.SendMessage(update.CallbackQuery.Message.Chat.Id, weatherInfo);
            break;
    }
}