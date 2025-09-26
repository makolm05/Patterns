namespace OpenClosed.Correct
{

    public interface IFileReader
    {
        public string ReadFileAsString(string filePath);
    }

    public class TxtFileReader : IFileReader
    {
        public string ReadFileAsString(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }

    public class JsonFileReader : IFileReader
    {
        public string ReadFileAsString(string filePath)
        {
            string content = File.ReadAllText(filePath);
            var data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(content);
            return "JSON keys: " + string.Join(", ", data.Keys);
        }
    }

    public class XmlFileReader : IFileReader
    {
        public string ReadFileAsString(string filePath)
        {
            string content = File.ReadAllText(filePath);
            var doc = new System.Xml.XmlDocument();
            doc.LoadXml(content);
            return "XML root: " + doc.DocumentElement.Name;
        }
    }

    public interface ILogger
    {
        public void Log(string message);
    }

    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class FileLogger : ILogger
    {
        public void Log(string message)
        {
            File.AppendAllText("log.txt", $"{DateTime.Now}: {message}\n");
        }
    }

    public class FileProcessor
    {
        private readonly IFileReader _fileReader;
        private readonly ILogger _logger;
        
        public FileProcessor(IFileReader fileReader, ILogger logger)
        {
            _fileReader = fileReader;
            _logger = logger;
        }

        public void Process(string filePath)
        {
            string content = _fileReader.ReadFileAsString(filePath);
            _logger.Log(content);
        }
    }
}
