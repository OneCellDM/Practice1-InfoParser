using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using static System.Collections.Specialized.BitVector32;

namespace ConnectionInfoParaser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string filePath = string.Empty;

            if(args.Length == 0)
            {
                Console.WriteLine("Список аргументов коммандной строки пуст");
                Environment.Exit(-1);
            }

            filePath = args.Last();

            var fileInfo = new FileInfo(filePath);

            if (fileInfo.Exists == false) 
            {
                Console.WriteLine("Файл не существует");
                Environment.Exit(-1);
            }

            if (fileInfo.Length == 0)
            {
                Console.WriteLine("Файл пуст");
                Environment.Exit(0);
            }

            try
            {
                DataReader dataReader = new DataReader(filePath);

                DataSplitter dataSplitter = new DataSplitter(dataReader.Read());

                var badData = dataSplitter.GetBadData();

               

                StringBuilder stringBuilder = new StringBuilder();

                foreach (var data in badData)
                {
                    stringBuilder.AppendLine(CreateStringFromData(data));
                }

                File.WriteAllTextAsync($"bad_data.txt", stringBuilder.ToString());


                var goodData = dataSplitter.GetGoodData();

                var chunks = goodData.Chunk(5).ToList();

                for (var j = 0; j < 5; j++)
                {
                    stringBuilder = new StringBuilder();
                    for (int i = 0; i < chunks.Count(); i++)
                    {
                        if (j < chunks[i].Count())
                            stringBuilder.AppendLine(CreateStringFromData(chunks[i][j]));
                        
                    }
                      

                    File.WriteAllTextAsync($"base_{j}.txt", stringBuilder.ToString());
                }

                Console.WriteLine("Работа завершена");

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(-1);
            }

            
            
        }
        public static string CreateStringFromData(Data data) => 
            $"{data.Title}\n{data.ConnectionString}\n";


    }
   
    public record Data(string Title, string ConnectionString);
  
}