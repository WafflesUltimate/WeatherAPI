using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml.Schema;
using static System.Net.WebRequestMethods;
using Newtonsoft.Json.Linq;

namespace WeatherAPI
{
    public class Program
    {

        public static string graffitti = " __          __        _   _                 _____        __     __                 _____ _ _           _______        _               \r\n \\ \\        / /       | | | |               |_   _|       \\ \\   / /                / ____(_) |         |__   __|      | |            _ \r\n  \\ \\  /\\  / /__  __ _| |_| |__   ___ _ __    | |  _ __    \\ \\_/ /__  _   _ _ __  | |     _| |_ _   _     | | ___   __| | __ _ _   _(_)\r\n   \\ \\/  \\/ / _ \\/ _` | __| '_ \\ / _ \\ '__|   | | | '_ \\    \\   / _ \\| | | | '__| | |    | | __| | | |    | |/ _ \\ / _` |/ _` | | | |  \r\n    \\  /\\  /  __/ (_| | |_| | | |  __/ |     _| |_| | | |    | | (_) | |_| | |    | |____| | |_| |_| |    | | (_) | (_| | (_| | |_| |_ \r\n     \\/  \\/ \\___|\\__,_|\\__|_| |_|\\___|_|    |_____|_| |_|    |_|\\___/ \\__,_|_|     \\_____|_|\\__|\\__, |    |_|\\___/ \\__,_|\\__,_|\\__, (_)\r\n                                                                                                 __/ |                          __/ |  \r\n                                                                                                |___/                          |___/ ";
        public static string graffitti2 = "                  ██████                \r\n                ██      ██              \r\n              ██          ████          \r\n            ██              ▒▒██        \r\n        ████▒▒                ██        \r\n  ██████      ▒▒            ▒▒▒▒████    \r\n██▒▒            ▒▒        ▒▒      ▒▒██  \r\n██▒▒▒▒        ▒▒▒▒▒▒▒▒▒▒▒▒          ▒▒██\r\n  ██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██\r\n    ████████▓▓████████████████████████  ";

        public static double[] GetGeoCoordinates() 
        {
            var client = new HttpClient();
            string cityName = null;
            string stateCode = null;

            while (cityName == null || cityName.Length < 2)
            {
                Console.WriteLine("Please enter your city name to proceed.");
                cityName = Console.ReadLine();
            }

            while (stateCode == null || stateCode.Length < 2)
            {
                Console.WriteLine("Please enter your state abbreviation to proceed.");
                stateCode = Console.ReadLine();
            }

            var geocodeURL = $"https://api.openweathermap.org/geo/1.0/direct?q={cityName},{stateCode},USA&limit=5&appid=f05f3c5f17273fd2d51fd7c3def4b328";
            
            string geoInfo = client.GetStringAsync(geocodeURL).Result;
            geoInfo = geoInfo.Replace('[', ' ').Replace(']', ' ');

            string lat = JObject.Parse(geoInfo).GetValue("lat").ToString();
            string lon = JObject.Parse(geoInfo).GetValue("lon").ToString();

            double[] geoCoordinates = new double[2];

            geoCoordinates[0] = double.Parse(lat);
            geoCoordinates[1] = double.Parse(lon);

            return geoCoordinates;
        }


        public static string KelvinToFahrenheit(string kelvin)
        {
            var a = double.Parse(kelvin);
            double result = (a - 273.15) * 9 / 5 + 32;
            int truncResult = (int)result;

            string strResult = "°F" + " " + truncResult.ToString();
            return strResult;
        }

        static void Main(string[] args)
        {
            var client = new HttpClient();

            var geoCoordinates = GetGeoCoordinates();

            var weatherURL = $"https://api.openweathermap.org/data/2.5/weather?lat={geoCoordinates[0]}&lon={geoCoordinates[1]}&appid=f05f3c5f17273fd2d51fd7c3def4b328";

            var weatherInfo = client.GetStringAsync(weatherURL).Result;
            weatherInfo = weatherInfo.Replace('[', ' ').Replace(']', ' ');

            var a = JObject.Parse(weatherInfo).GetValue("weather").ToString();
            var b = JObject.Parse(weatherInfo).GetValue("main").ToString();

            string weatherDep = JObject.Parse(a).GetValue("description").ToString();
            string temp = JObject.Parse(b).GetValue("temp").ToString();
            string feelsLike = JObject.Parse(b).GetValue("feels_like").ToString();
            string humidity = JObject.Parse(b).GetValue("humidity").ToString();
            string minTemp = JObject.Parse(b).GetValue("temp_min").ToString();
            string maxTemp = JObject.Parse(b).GetValue("temp_max").ToString();

            humidity += "%";
            var fahrTemp = KelvinToFahrenheit(temp);
            var fahrTemp2 = KelvinToFahrenheit(feelsLike);
            var fahrTemp3 = KelvinToFahrenheit(minTemp);
            var fahrTemp4 = KelvinToFahrenheit(maxTemp);


            //Console.WriteLine(weatherInfo);

            Console.WriteLine(graffitti);
            Console.WriteLine($"{graffitti2}");
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine($"The forecast is {weatherDep}. The temparture is {fahrTemp}, but it feels like {fahrTemp2}." +
            $" The minimum temparture expected is {fahrTemp3} and the maximum temparture expected is {fahrTemp4}." +
            $" The humidity is {humidity}. Have a nice Day!");

        }
    }
}
