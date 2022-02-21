
namespace Garage_2._0
{
    public static class Global
    {
        public static int Garagecapacity;
        public static double HourlyRate;
        public static double BaseRate;

        public static string TimeAsString(TimeSpan timeSpan)
        {
            return $"{timeSpan.Days} days, {timeSpan.Hours} hours, {timeSpan.Minutes} minutes";
        }
    }
}
