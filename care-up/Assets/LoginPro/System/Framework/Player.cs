namespace LoginProAsset
{
    public class Player
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Score { get; set; }

        public Player(string id, string name, string score)
        {
            Id = id;
            Name = name;
            Score = score;
        }
    }
}
