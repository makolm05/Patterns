using LiskovSubstituion.Wrong;

namespace LiskovSubstituion.Correct
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
            if (string.IsNullOrWhiteSpace(newContent))
                throw new ArgumentException("Content cannot be empty."); // предусловие
            Content = newContent;
        }
    }

    public interface IReadable
    {
        public void Read(Document doc);
    }

    public interface IEditable
    {
        public void Edit(Document doc, string newContent);
    }

    public interface IDeletable
    {
        public void Delete(List<Document> docs, Document doc);
    }

    public class User : IReadable
    {
        public void Read(Document doc)
        {
            Console.WriteLine($"Reading: {doc.Title} - {doc.Content}");
        }
    }

    public class Editor : IReadable, IEditable
    {
        public void Read(Document doc)
        {
            Console.WriteLine($"Reading: {doc.Title} - {doc.Content}");
        }

        public void Edit(Document doc, string newContent)
        {
            doc.UpdateContent(newContent);
            Console.WriteLine($"Edited document {doc.Title}."); // постусловие
        }
    }

    public class Admin : IReadable, IEditable, IDeletable
    {
        public void Read(Document doc)
        {
            Console.WriteLine($"Reading: {doc.Title} - {doc.Content}");
        }

        public void Edit(Document doc, string newContent)
        {
            doc.UpdateContent(newContent);
            Console.WriteLine($"Edited document {doc.Title}."); // постусловие
        }
        public void Delete(List<Document> docs, Document doc)
        {
            if (!docs.Contains(doc))
                throw new ArgumentException("Document not found."); // предусловие

            docs.Remove(doc);
            Console.WriteLine($"Deleted document {doc.Title}."); // постусловие
        }
    }
}
