namespace TgBotPractice.WeatherService;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using DataBase;

public class NotesCommand
{
    public async Task NotesCmd(ITelegramBotClient botClient, Message msg, UpdateType type)
    {
        await botClient.SendMessage(msg.Chat, "Notes");
        // TODO: Добавить функционал для заметок
    }
}
