﻿using Telegram.Bot.Types.ReplyMarkups;
using TgBotPractice.DataBase;
namespace TgBotPractice;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

public class StartCommand
{
    private const string WelcomeMessage = """
    🌟 <b>Добро пожаловать в Weather Bot!</b>

    🤖 Я бот, который поможет вам быстро узнать погоду в любом городе мира.

    📋 <b>Основные команды:</b>
    • /weather - Погода в Самаре (по умолчанию)
    • /weather [город] - Погода в указанном городе (город писать на англ. языке)
    Например: <code>/weather Moscow</code>

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
    Версия: 1.0.2</i>
    """;

    public async Task DBCheck(Message msg)
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
    
    public async Task StartCmd(ITelegramBotClient botClient, Message msg, UpdateType type)
    {
        await DBCheck(msg);
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