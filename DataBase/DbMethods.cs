namespace TgBotPractice.DataBase;
using Telegram.Bot.Types.ReplyMarkups;
using TgBotPractice.DataBase;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

public class DbMethods
{
    public static async Task DBCheck(Message msg)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            Human user = new Human { ChatId = msg.Chat.Id, City = String.Empty, Autosend = false, IsAdmin = false };
            if (db.Users.Any(u => u.ChatId == user.ChatId))
            {
                Console.WriteLine($"ChatId: {user.ChatId} already exists.");
            }
            else
            {
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
}