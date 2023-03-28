using System.Text.RegularExpressions;

namespace ConnectionInfoParaser
{
    public class DataSplitter
    {
        private List<Data> _Data;
        public DataSplitter(List<Data> data) => _Data = data;       
        
        public bool DataIsValid(Data data)
        {
            var str = data.ConnectionString;
            if (str.ToLower().Contains("file="))
            {
                var pathMatch = Regex.Match(str, @"""[^""]*""");

                if (pathMatch.Success) 
                    return PathIsValid(pathMatch.Value.Replace('"',' ').Trim());
              
            }
            else if (str.ToLower().Contains("srvr="))
            {

                var refHostMatch = Regex.Match(str, @"(srvr=)(""[^""]*"")", RegexOptions.IgnoreCase);
                var refNameMatch = Regex.Match(str, @"(ref=)(""[^""]*"")", RegexOptions.IgnoreCase);

                return refHostMatch.Success && refNameMatch.Success; 
            }
            return false;

            
        }

        private bool PathIsValid(string path) =>
            Regex.IsMatch(path, @"^(?:[a-zA-Z]\:|\\\\[\w\.]+\\[\w.$]+)\\(?:[\w]+\\)*\w([\w.])+$", RegexOptions.IgnoreCase);
        
     
        public List<Data> GetGoodData() => _Data.Where(x => DataIsValid(x) == true).ToList();
        
        public List<Data> GetBadData() => _Data.Where(x => DataIsValid(x) == false).ToList();



    }
  
}