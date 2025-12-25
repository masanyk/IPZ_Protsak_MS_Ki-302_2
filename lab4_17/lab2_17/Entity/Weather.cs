namespace lab2_17.Entity
{
    public class Weather
    {
        public string Date { get; set; }
        public string Temperature { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"{Date}: {Temperature}, {Description}";
        }
    }
}