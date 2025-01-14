using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TgBotPractice.DataBase;

namespace TgBotPractice.Profile;
public class ProfileCommand
{
    private string _city;
    private int _autoWeather;
    private bool _autoSend;
    private bool _isAdmin;
    public async Task ProfileCmd(ITelegramBotClient botClient, Message msg, UpdateType type)
    {
        await using (ApplicationContext db = new ApplicationContext())
        {
            Human user = new Human { ChatId = msg.Chat.Id, City = String.Empty, Autosend = false, IsAdmin = false };
            var findUser = db.Users.FirstOrDefault(u => u.ChatId == user.ChatId);
            if (findUser is not null)
            {
                _city = findUser.City;
                _autoSend = findUser.Autosend; // bool
                _autoWeather = Convert.ToInt32(findUser.AutoWeather);
                _isAdmin = findUser.IsAdmin;
            }
            else
            {
                _city = "Samara";
                _autoSend = false; // bool
                _autoWeather = 0;
                _isAdmin = false;
                await DbMethods.DbCheck(msg, botClient);
            }
        }
        string command = $"""
         Профиль пользователя в чате: {msg.Chat.Id}
         Установлекнный город: {_city}
         Статус автоотправки: {_autoSend}
         Время авто отправки: {_autoWeather}
         Админ Статус: {_isAdmin}
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
                InlineKeyboardButton.WithUrl("⭐️ GitHub source проекта", "https://github.com/lvd536/weather-telegram-bot"),
            }
        });
        await botClient.SendMessage(msg.Chat, command, ParseMode.Html, replyMarkup: keyboard);
    }
}