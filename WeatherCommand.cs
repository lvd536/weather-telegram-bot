using Telegram.Bot.Types.ReplyMarkups;
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
        /*await botClient.SendMessage(msg.Chat.Id, "Напишите название города на английском. \nПример: Samara || Moscow || Berlin || Sochi", ParseMode.Html);
        await Task.Delay(2500);
        string city = msg.Text ?? "Samara";
        try
        {
            var weatherData = await _weatherService.GetWeatherAsync(city);
            var back = new InlineKeyboardMarkup().AddButton("Back", "startCall");
            await botClient.SendMessage(msg.Chat, weatherData.GetFormattedWeather(), replyMarkup: back);
        }
        catch (Exception ex)
        {
            await botClient.SendMessage(msg.Chat, $"Произошла ошибка при получении погоды: {ex.Message}. Получаем погоду в городе *Samara*");
            var weatherData = await _weatherService.GetWeatherAsync("Samara");
            await botClient.SendMessage(msg.Chat, weatherData.GetFormattedWeather(), ParseMode.Html);
        }*/
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
        }
    }
}