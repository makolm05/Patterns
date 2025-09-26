namespace OpenClosed.Wrong
{
    public class FileProcessor
    {
        public string FilePath { get; set; }

        public void Process(string fileType)
        {
            if (fileType == "txt")
            {
                string content = File.ReadAllText(FilePath);
                Console.WriteLine($"TXT file content: {content}");
            }
            else if (fileType == "json")
            {
                string content = File.ReadAllText(FilePath);
                var data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(content);
                Console.WriteLine("JSON keys: " + string.Join(", ", data.Keys));
            }
            else if (fileType == "xml")
            {
                string content = File.ReadAllText(FilePath);
                var doc = new System.Xml.XmlDocument();
                doc.LoadXml(content);
                Console.WriteLine("XML root: " + doc.DocumentElement.Name);
            }
        }

        public void SaveLog(string message)
        {
            File.AppendAllText("log.txt", $"{DateTime.Now}: {message}\n");
        }
    }
}
