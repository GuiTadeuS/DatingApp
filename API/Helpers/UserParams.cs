namespace API.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 50;
        
        private int _pageSize = 10;

        private int _minAge = 18;

        private int _maxAge = 99;
        public int PageNumber { get; set; } = 1; // By default, return the first page

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string? CurrentUsername { get; set; }

        public string? Gender { get; set; }
        public int MinAge 
        {
            get => _minAge;
            set => _minAge = value > 18 ? value : 18;
        }

        public int MaxAge 
        {
            get => _maxAge;
            set => _maxAge = value < 99 ? value : 99;
        }


    }
}
