namespace TgBotPractice;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

public class WeatherCommand
{
    private readonly WeatherService _weatherService;

    public WeatherCommand()
    {
        _weatherService = new WeatherService("3e9eae6efa142dac8de06fd29fffca12");
    }

    public async Task WeatherCmd(ITelegramBotClient botClient, Message msg, UpdateType type)
    {
        Console.WriteLine("[Weather] Начали получать данные погоды");
        try
        {
            var weatherData = await _weatherService.GetWeatherAsync("Samara");
            await botClient.SendMessage(msg.Chat, weatherData.GetFormattedWeather(), ParseMode.Html);
            Console.WriteLine("[Weather] Успешно!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("[Weather] Ошибка при получении погоды: " + ex.Message);
            await botClient.SendMessage(msg.Chat, $"Ошибка {ex.Message} при попытке получить погоду. Пытаемся получить тестовую погоды в городе Москва", ParseMode.Html);
            var weatherData = await _weatherService.GetWeatherAsync("Moscow");
            await botClient.SendMessage(msg.Chat, weatherData.GetFormattedWeather(), ParseMode.Html);
        }
    }
}