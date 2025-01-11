namespace TgBotPractice.DataBase;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

public class DbMethods
{
    public static async Task DbCheck(Message msg, ITelegramBotClient botClient)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            Human user = new Human { ChatId = msg.Chat.Id, City = "Samara", Autosend = false, IsAdmin = false };
            if (db.Users.Any(u => u.ChatId == user.ChatId))
            { 
                Console.WriteLine($"ChatId: {user.ChatId} already exists.");
            }
            else
            {
                await botClient.SendMessage(msg.Chat.Id, "Вы еще не зарегистрированы в нашей базе данных. Регистрируем", ParseMode.Html);
                await db.Users.AddAsync(user);
                await db.SaveChangesAsync();
                Console.WriteLine($"ChatId: {user.ChatId} has been added.");
            }

            var users = db.Users.ToList();
            foreach (var u in users)
            {
                Console.WriteLine(u.ChatId);
            }

        }
    }
    public static async Task DbDefCity(string city, Message msg, ITelegramBotClient botClient)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            Human user = new Human { ChatId = msg.Chat.Id, City = String.Empty, Autosend = false, IsAdmin = false };
            var findUser = db.Users.FirstOrDefault(u => u.ChatId == user.ChatId);
            if (findUser is not null)
            {
                findUser.City = city;
                await db.SaveChangesAsync();
            }
            else
            {
                await DbCheck(msg, botClient);
            }
        }
    }
}