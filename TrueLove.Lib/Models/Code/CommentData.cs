using System.Collections.ObjectModel;

namespace TrueLove.Lib.Models.Code
{
    public class CommentData : ObservableCollection<string>
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public string Date { get; set; }
    }

    public class UpdateData
    {

    }
}
