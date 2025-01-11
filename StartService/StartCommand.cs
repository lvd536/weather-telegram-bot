using Telegram.Bot.Types.ReplyMarkups;
namespace TgBotPractice.StartService;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBotPractice.DataBase;

public class StartCommand
{
    private const string WelcomeMessage = """
    🌟 <b>Добро пожаловать в Weather Bot!</b>

    🤖 Я бот, который поможет вам быстро узнать погоду в любом городе мира.

    📋 <b>Основные команды:</b>
    • /weather - Погода в Самаре (по умолчанию)
    • /weather [город] - Погода в указанном городе (город писать на англ. языке)
    Например: <code>/weather Moscow</code>
    • /weather default [город] - Погода в указанном городе при исп. /weather
    Например: <code>/weather default Sochi</code> (город всегда писать на англ. языке)

    🔥 <b>Возможности бота:</b>
    • 🌡️ Текущая температура
    • 🤔 Ощущаемая температура
    • ☁️ Состояние погоды
    • 💧 Влажность воздуха
    • 💨 Скорость ветра
    • 🎈 Атмосферное давление

    ⚡️ <b>Быстрый доступ:</b>
    Используйте кнопку ниже для мгновенного получения погоды 👇
    
    🔜 <b>Скоро будет доступно:</b>
    • 📅 Ежедневные уведомления о погоде
    • 🌍 Сохранение нескольких городов
    • 📊 Прогноз на несколько дней

    💡 <i>Разработчик: @lvdshka
    Версия: 1.5.0</i>
    """;
    
    public async Task StartCmd(ITelegramBotClient botClient, Message msg, UpdateType type)
    {
        await DbMethods.DbCheck(msg, botClient);
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


        
        await botClient.SendMessage(msg.Chat.Id,WelcomeMessage, parseMode: ParseMode.Html, replyMarkup: keyboard );
    }
}