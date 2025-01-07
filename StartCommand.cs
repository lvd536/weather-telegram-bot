﻿using System.Windows.Input;
using Telegram.Bot.Types.ReplyMarkups;
namespace TgBotPractice;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
public class StartCommand
{
    public async Task StartCmd(ITelegramBotClient botClient, Message msg, UpdateType type)
    {
        var test = new InlineKeyboardMarkup().AddButton("Weather", "weatherCall");
        await botClient.SendMessage(msg.Chat, "Пока что в боте есть только 1 возможность - просмотр погоды", replyMarkup: test);
    }
}