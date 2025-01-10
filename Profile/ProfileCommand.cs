using Microsoft.VisualBasic;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TgBotPractice.DataBase;

namespace TgBotPractice.Profile;
public class ProfileCommand
{
    public async Task ProfileCmd(ITelegramBotClient botClient, Message msg, UpdateType type)
    {
        string city = "Samara";
        bool isAdmin = false;
        await DbMethods.DBProfile(city, isAdmin, msg, botClient);
        string command = $"""
         Профиль пользователя в чате: {msg.Chat.Id}
         Установлекнный город: {city}
         Админ Статус: {isAdmin}
         """;
        var keyboard = new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("🌤 Узнать погоду", "weatherCall"),
            },
            new []
            {
                InlineKeyboardButton.WithUrl("📱 Telegram разработчика", "https://t.me/lvdshka"),
                InlineKeyboardButton.WithUrl("⭐️ GitHub проекта", "https://github.com/lvd536/weather-telegram-bot"),
            }
        });
        await botClient.SendMessage(msg.Chat, command, ParseMode.Html, replyMarkup: keyboard);
    }
}