using System.Collections;

namespace True_Love.Dates
{
    class Comments : DictionaryBase
    {
        public void Add(Comment newComment) => Dictionary.Add(newComment.name, newComment);
        public void Remove(string name) => Dictionary.Remove(name);
        public Comments() { }
        public Comment this[string name]
        {
            get => (Comment)Dictionary[name];
            set => Dictionary[name] = value;
        }
        public override string ToString() => $"form {(Dictionary.Values as Comment).name.ToUpper()}\n{(Dictionary.Values as Comment).commentary}"; 
        public IEnumerable Name
        {
            get
            {
                foreach(object comment in Dictionary.Values)
                {
                    yield return (comment as Comment).name;
                }
            }
        } 
    }
}
