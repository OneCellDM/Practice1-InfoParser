using System.Text.RegularExpressions;

namespace ConnectionInfoParaser
{
   
    public class DataReader
    {
        private Regex _SectionsRegex = new Regex(@"^\r\n", RegexOptions.Multiline);

        private string _FilePath;
        public DataReader(string filePath)
        {
            _FilePath = filePath;
        }

        
        public List<Data> Read()
        {
          List<Data> res = new List<Data>();

          var data = File.ReadAllText(_FilePath).Trim();
          var sections = _SectionsRegex.Split(data);
         
          foreach (var section in sections)
          {
                string title = string.Empty;
                string connectionString = string.Empty;

                var lines = section.Split(Environment.NewLine);

                foreach (var line in lines)
                {
                    if (line.StartsWith('[') && line.EndsWith(']'))
                        title = line;

                    if (line.ToLower().StartsWith("connect"))
                        connectionString = line;
                }

                ReadValidOrException(title, connectionString);
                
                res.Add(new(title, connectionString));        
            }
            return res;
        }
        private void ReadValidOrException(string title, string connectionString)
        {

            if (title.Length == 0)
                throw new Exception("Неверный формат файла: Отсуствует заголовок данных");

            if (connectionString.Length == 0)
                throw new Exception("Неверный формат файла: Отсуствует обязательная строка подключения");


            var stringsections = connectionString.Split(";").Where(x => x.Trim().Length > 0).ToList();

            var first = stringsections.First();

            foreach (var stringSection in stringsections)
            {
                var sigments = stringSection.Split('=');

                if (sigments.Length == 1)
                    throw new Exception("Неверный формат файла: Отсуствует разделитель имени от значения");

                if (sigments.Count(x => x.Length == 0) > 0)
                    throw new Exception("Неверный формат файла: Отсутствует имя или значение параметра");
            }
        }
        
    }
  
}