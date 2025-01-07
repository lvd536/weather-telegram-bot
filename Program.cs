using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient("7557004382:AAFSqf56fgYQWHvpg1VU6zGJxJ_mdaQnkTI", cancellationToken: cts.Token);
var me = await bot.GetMe(); // Гетаем нейм бота
bot.OnMessage += OnMessage;

Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
Console.ReadLine();
cts.Cancel(); // stop the bot

// МСГ Хендл
async Task OnMessage(Message msg, UpdateType type)
{
    if (msg.Text is null) return;	// Если текс нулл - идет нахуй
    Console.WriteLine($"Received {type} '{msg.Text}' in {msg.Chat}");
    // Возврат соо 
    await bot.SendMessage(msg.Chat, $"{msg.From} said: {msg.Text}");
    
}