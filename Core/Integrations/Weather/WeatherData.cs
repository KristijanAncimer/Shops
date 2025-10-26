namespace Core.Integrations.Weather;

public class WeatherData
{
    public DailyWeather Daily { get; set; }
}

public class DailyWeather
{
    public List<string> Time { get; set; } = new();
    public List<decimal> Temperature_2m_Max { get; set; } = new();
    public List<decimal> Temperature_2m_Min { get; set; } = new();
    public List<decimal> Windspeed_10m_Max { get; set; } = new();
    public List<decimal> Precipitation_Sum { get; set; } = new();
}
