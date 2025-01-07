using Telegram.Bot.Types.ReplyMarkups;
namespace TgBotPractice;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
public class WeatherCommand
{
    public async Task WeatherCmd(ITelegramBotClient botClient, Message msg, UpdateType type)
    {
        var test = new InlineKeyboardMarkup().AddButton("Back", "startCall");
        string weatherInfo = "Текущая погода: солнечно, +20°C";
        await botClient.SendMessage(msg.Chat, weatherInfo, replyMarkup: test);
    }
}