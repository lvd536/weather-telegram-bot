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
        string city = String.Empty;
        bool isAdmin = false;
        using (ApplicationContext db = new ApplicationContext())
        {
            Human user = new Human { ChatId = msg.Chat.Id, City = String.Empty, Autosend = false, IsAdmin = false };
            var findUser = db.Users.FirstOrDefault(u => u.ChatId == user.ChatId);
            if (findUser is not null)
            {
                city = findUser.City;
                isAdmin = findUser.IsAdmin;
            }
            else
            {
                city = "Samara";
                isAdmin = false;
                await DbMethods.DBCheck(msg, botClient);
            }
        }
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