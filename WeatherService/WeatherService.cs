using Newtonsoft.Json.Linq;

namespace TgBotPractice.WeatherService;

public class WeatherService
{
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://api.openweathermap.org/data/2.5/weather";

    public WeatherService(string apiKey)
    {
        _apiKey = apiKey;
        _httpClient = new HttpClient();
    }

    public async Task<WeatherData> GetWeatherAsync(string city)
    {
        try
        {
            var response = await _httpClient.GetStringAsync(
                $"{BaseUrl}?q={city}&appid={_apiKey}&units=metric&lang=ru");
            var json = JObject.Parse(response);

            return new WeatherData
            {
                Temperature = Math.Round(json["main"]["temp"]!.Value<double>(), 1),
                FeelsLike = Math.Round(json["main"]["feels_like"]!.Value<double>(), 1),
                Description = json["weather"][0]["description"]!.Value<string>()!,
                Humidity = json["main"]["humidity"]!.Value<int>(),
                WindSpeed = Math.Round(json["wind"]["speed"]!.Value<double>(), 1),
                Pressure = json["main"]["pressure"]!.Value<int>(),
                CityName = json["name"]!.Value<string>()!
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при получении погоды: {ex.Message}");
        }
    }
}

public class WeatherData
{
    public double Temperature { get; set; }
    public double FeelsLike { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Humidity { get; set; }
    public double WindSpeed { get; set; }
    public int Pressure { get; set; }
    public string CityName { get; set; } = string.Empty;

    public string GetFormattedWeather()
    {
        return $"""
                🌍 Погода в городе {CityName}:
                
                🌡 Температура: {Temperature}°C
                🤔 Ощущается как: {FeelsLike}°C
                ☁️ Состояние: {Description}
                💧 Влажность: {Humidity}%
                💨 Скорость ветра: {WindSpeed} м/с
                🎈 Давление: {Pressure} гПа
                """;
    }
} 