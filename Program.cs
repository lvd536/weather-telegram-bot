﻿using System.Runtime.InteropServices.JavaScript;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBotPractice;

using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient("7557004382:AAFSqf56fgYQWHvpg1VU6zGJxJ_mdaQnkTI", cancellationToken: cts.Token);
var me = await bot.GetMe();
WeatherCommand weatherCommand = new WeatherCommand();
StartCommand startCommand = new StartCommand();
bot.OnMessage += OnMessage;
bot.OnUpdate += OnCallbackQuery;
bot.OnError += OnError;
Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
Console.ReadLine();
cts.Cancel();

async Task OnMessage(Message msg, UpdateType type)
{
    if (msg.Text is null) return;
    var commandParts = msg.Text.Split(' ');
    var command = commandParts[0];
    var argument = commandParts.Length > 1 ? commandParts[1] : null;
    if (msg.Text.StartsWith('/'))
    {
        switch (command)
        {
            case "/start":
                await startCommand.StartCmd(bot, msg, type);
                break;
            
            case "/weather":
                if (argument is not null)
                {
                    await weatherCommand.WeatherCmd(bot, msg, type, argument);
                }
                else
                {
                    await weatherCommand.WeatherCmd(bot, msg, type);
                }
                break;
        }
    }
    Console.WriteLine($"[Debug] Received {type} '{msg.Text}' in {msg.Chat}");
}

async Task OnCallbackQuery(Update update)
{
    if (update.Type != UpdateType.CallbackQuery) return;
    switch (update.CallbackQuery?.Data)
    {
        case "weatherCall":
            await weatherCommand.WeatherCmd(bot, update.CallbackQuery.Message ?? new Message(), update.Type);
            break;
        case "startCall":
            await startCommand.StartCmd(bot, update.CallbackQuery.Message ?? new Message(), update.Type);
            break;
    }
}

async Task OnError(Exception exception, HandleErrorSource handler)
{
    Console.WriteLine(exception);
    await Task.Delay(2000, cts.Token);
}