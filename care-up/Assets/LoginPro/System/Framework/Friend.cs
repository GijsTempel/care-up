namespace LoginProAsset
{
    public class Friend
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public Friend(string id, string name, string status)
        {
            this.Id = id;
            this.Name = name;
            this.Status = status;
        }
    }
}