namespace TrueLove.Lib.Datebase
{
    class Comment 
    {
        private Comment() { }
        private string _name;
        public string name
        {
            get => _name;
            set => _name = value;
        }
        private string _commentary;
        public string commentary
        {
            get => _commentary;
            set => _commentary = value;
        }
        public Comment(string newName, string newCommentary)
        {
            name = newName;
            commentary = newCommentary;
        }
        //public override string ToString() => $"form {name.ToUpper()}\n{commentary}";
    }
}
