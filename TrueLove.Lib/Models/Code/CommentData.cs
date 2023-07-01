using System.Collections.ObjectModel;

namespace TrueLove.Lib.Models.Code
{
    public class CommentData : ObservableCollection<string>
    {
        public string name { get; set; }
        public string comment { get; set; }
        public string date { get; set; }
    }

    public class UpdateItem
    {

    }
}
