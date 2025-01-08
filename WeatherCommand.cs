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
        try
        {
            var weatherData = await _weatherService.GetWeatherAsync("Samara"); // По умолчанию Москва
            var back = new InlineKeyboardMarkup().AddButton("Back", "startCall");
            await botClient.SendMessage(msg.Chat, weatherData.GetFormattedWeather(), replyMarkup: back);
        }
        catch (Exception ex)
        {
            await botClient.SendMessage(msg.Chat, $"Произошла ошибка при получении погоды: {ex.Message}");
        }
    }
}