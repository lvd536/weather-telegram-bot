﻿namespace TgBotPractice.WeatherService;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using DataBase;

public class WeatherCommand
{
    private readonly WeatherService _weatherService;
    private string _city;
    public WeatherCommand()
    {
        _weatherService = new WeatherService("3e9eae6efa142dac8de06fd29fffca12");
    }
    public async Task WeatherCmd(ITelegramBotClient botClient, Message msg, UpdateType type)
    {
        Console.WriteLine("[Weather] Начали получать данные погоды");
        try
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Human user = new Human { ChatId = msg.Chat.Id, City = String.Empty, Autosend = false, IsAdmin = false };
                var findUser = db.Users.FirstOrDefault(u => u.ChatId == user.ChatId);
                if (findUser is not null)
                {
                    _city = findUser.City;
                    var weatherData = await _weatherService.GetWeatherAsync(_city);
                    await botClient.SendMessage(msg.Chat, weatherData.GetFormattedWeather(), ParseMode.Html);
                    Console.WriteLine("[Weather] Успешно!");
                }
                else
                {
                    await DbMethods.DbCheck(msg, botClient);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[Weather] Ошибка при получении погоды: " + ex.Message);
            await botClient.SendMessage(msg.Chat, $"Ошибка при попытке получить погоду. Пытаемся получить тестовую погоду в городе Москва", ParseMode.Html);
            var weatherData = await _weatherService.GetWeatherAsync("Moscow");
            await botClient.SendMessage(msg.Chat, weatherData.GetFormattedWeather(), ParseMode.Html);
        }
    }
    
    public async Task WeatherCmd(ITelegramBotClient botClient, Message msg, UpdateType type, string arg)
    {
        Console.WriteLine("[Weather] Начали получать данные погоды");
        try
        {
            var weatherData = await _weatherService.GetWeatherAsync(arg);
            await botClient.SendMessage(msg.Chat, weatherData.GetFormattedWeather(), ParseMode.Html);
            Console.WriteLine("[Weather] Успешно!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("[Weather] " + ex.Message);
            await botClient.SendMessage(msg.Chat, $"Ошибка {ex.Message} при попытке получить погоду. Пытаемся получить тестовую погоду в городе Москва", ParseMode.Html);
            var weatherData = await _weatherService.GetWeatherAsync("Moscow");
            await botClient.SendMessage(msg.Chat, weatherData.GetFormattedWeather(), ParseMode.Html);
        }
    }
    
    public async Task WeatherAutoCmd(ITelegramBotClient botClient, Message msg, UpdateType type, string input)
    {   
        await using (ApplicationContext db = new ApplicationContext())
        {
            var findUserByAutosend = db.Users.FirstOrDefault(u => u.Autosend != false);
            if (findUserByAutosend is not null)
            {
                input = findUserByAutosend.AutoWeather.ToString();
                Console.WriteLine($"[Debug] успешно применено время с DB - {input}");
                
                if (TimeSpan.TryParse(input, out TimeSpan targetTime))
                {
                    await botClient.SendMessage(msg.Chat, $"Вы уже ранее установили время на {targetTime}. Запускаем таймер еще раз", ParseMode.Html);
                    Console.WriteLine("[Debug Timer] Таймер запущен. Рассылка будет происходить ежедневно в указанное время.");
                    Task backgroundTask = Task.Run(async () =>
                    {
                        while (true)
                        {
                            DateTime now = DateTime.Now;
                            Console.WriteLine($"[Debug] Начал создавать рассылку для чата {msg.Chat}! Текущее время: {now}");
                            DateTime nextCall = now.Date + targetTime;
                            if (nextCall <= now)
                            {
                                nextCall = nextCall.AddDays(1);
                            }
                            Console.WriteLine($"[Debug] Рассылка в чат {msg.Chat} будет отправлена в {nextCall}");
                            TimeSpan timeToWait = nextCall - now;
                    
                            if (timeToWait.TotalMilliseconds < 0)
                            {
                                Console.WriteLine("[Debug] Ошибка: отрицательное время ожидания. Проверьте логику расчета nextCall.");
                                break;
                            }
                            await Task.Delay(timeToWait);
                            await WeatherCmd(botClient, msg, type);
                            Console.WriteLine($"Рассылка для чата {msg.Chat} успешно выполнена в {DateTime.Now}");
                        }
                    });
                }
            }
            else
            {
                await DbMethods.DbCheck(msg, botClient);
                
                if (TimeSpan.TryParse(input, out TimeSpan targetTime))
                {
                    Human user = new Human { ChatId = msg.Chat.Id, City = String.Empty, Autosend = false, IsAdmin = false };
                    var findUserById = db.Users.FirstOrDefault(u => u.ChatId == user.ChatId);
                    if (findUserById is not null)
                    {
                        findUserById.Autosend = true;
                        findUserById.AutoWeather = targetTime;
                    }
                    Console.WriteLine("[Debug Timer] Таймер запущен. Рассылка будет происходить ежедневно в указанное время.");
                    Task backgroundTask = Task.Run(async () =>
                    {
                        while (true)
                        {
                            DateTime now = DateTime.Now;
                            Console.WriteLine($"[Debug] Начал создавать рассылку для чата {msg.Chat}! Текущее время: {now}");
                            DateTime nextCall = now.Date + targetTime;
                            if (nextCall <= now)
                            {
                                nextCall = nextCall.AddDays(1);
                            }
                            Console.WriteLine($"[Debug] Рассылка в чат {msg.Chat} будет отправлена в {nextCall}");
                            TimeSpan timeToWait = nextCall - now;
                    
                            if (timeToWait.TotalMilliseconds < 0)
                            {
                                Console.WriteLine("[Debug] Ошибка: отрицательное время ожидания. Проверьте логику расчета nextCall.");
                                break;
                            }
                            await Task.Delay(timeToWait);
                            await WeatherCmd(botClient, msg, type);
                            Console.WriteLine($"[Debug] Рассылка для чата {msg.Chat} успешно выполнена в {DateTime.Now}");
                        }
                    });
                }
                else
                {
                    await botClient.SendMessage(msg.Chat, "Неверно указано время! Пример: 8:00 | /weather auto 8:00", ParseMode.Html);
                }
            }
        }
        
    }
}