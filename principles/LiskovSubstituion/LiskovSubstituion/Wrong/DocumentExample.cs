namespace LiskovSubstituion.Wrong
{
    public class Document
    {
        public string Title { get; }
        public string Content { get; private set; }

        public Document(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public void UpdateContent(string newContent)
        {
            Content = newContent;
        }
    }

    public abstract class UserRole
    {
        public abstract void Read(Document doc);
        public virtual void Edit(Document doc, string newContent)
        {
            throw new InvalidOperationException("This role cannot edit documents.");
        }
        public virtual void Delete(List<Document> docs, Document doc)
        {
            throw new InvalidOperationException("This role cannot delete documents.");
        }
    }

    public class User : UserRole
    {
        public override void Read(Document doc)
        {
            Console.WriteLine($"Reading: {doc.Title} - {doc.Content}");
        }
    }

    public class Editor : UserRole
    {
        public override void Read(Document doc)
        {
            Console.WriteLine($"Reading: {doc.Title} - {doc.Content}");
        }

        public override void Edit(Document doc, string newContent)
        {
            if (string.IsNullOrWhiteSpace(newContent))
                throw new ArgumentException("Content cannot be empty."); // предусловие
            doc.UpdateContent(newContent);
            Console.WriteLine($"Edited document {doc.Title}."); // постусловие
        }
    }

    public class Admin : Editor
    {
        public override void Delete(List<Document> docs, Document doc)
        {
            if (!docs.Contains(doc))
                throw new ArgumentException("Document not found."); // предусловие

            docs.Remove(doc);
            Console.WriteLine($"Deleted document {doc.Title}."); // постусловие
        }
    }
}
