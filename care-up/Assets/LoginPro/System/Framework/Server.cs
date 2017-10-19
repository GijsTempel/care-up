namespace LoginProAsset
{
    public class Server
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public Server(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
