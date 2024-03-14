namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime? SetKindUtc(this DateTime? dateTime)
        {
            if (dateTime == null) return null;

            return DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc);
        }

        public static int CalculateAge(this DateOnly dateOfBirth)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var age = today.Year - dateOfBirth.Year;

            if (dateOfBirth > today.AddYears(age)) age--;

            return age;
        }
    }
}
