using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;

namespace TesteWiproClient
{
    class Program
    {
        private static Timer ProcessCurrenciesTimer;
        private const int RepeatIntervalTimer = 5000;//120000; //2 minutes
        private const string CurrencyAPIAddress = "https://localhost:44333/api/";
        private const string CurrencyDataFileHeader = "ID_MOEDA;DATA_REF";
        private const string CurrencyPriceFileHeader = "vlr_cotacao;cod_cotacao;dat_cotacao";
        private const string ResultingFileHeader = "ID_MOEDA;DATA_REF;VLR_COTACAO";

        static void Main(string[] args)
        {
            ProcessCurrenciesTimer = new Timer(ProcessCurrencyPrices, null, 0, RepeatIntervalTimer);

            Console.WriteLine("Press \'q\' to quit.");
            while (Console.Read() != 'q');
        }

        private async static void ProcessCurrencyPrices(Object state)
        {
            LogToConsole("Calling currency data API");

            Currency result = await CallCurrencyAPI();

            if (result != null) 
            {
                LogToConsole("API data found, processing files");

                List<CurrencyData> currencyDataResult = ProcessCurrencyDataFile(result);
                ProcessCurrencyPriceFile(currencyDataResult);
            }
            else
            {
                LogToConsole("No data returned from API");
            }
        }

        private async static Task<Currency> CallCurrencyAPI() 
        {
            using (HttpClient client = new HttpClient()) 
            {
                var responseString = await client.GetStringAsync(CurrencyAPIAddress + "currency/GetItemFila");

                if (!string.IsNullOrEmpty(responseString))
                {
                    Currency result = JsonConvert.DeserializeObject<Currency>(responseString);
                    return result;
                }
                else 
                {
                    return null;
                }
            }
        }

        private static List<CurrencyData> ProcessCurrencyDataFile(Currency currency)
        {
            LogToConsole("Processing currency data file");

            List<string> lines = File.ReadLines("DadosMoeda.csv").ToList();

            lines = lines.FindAll(line => {

                if (line == CurrencyDataFileHeader)
                    return false;

                CurrencyData data = SplitCurrencyDataFileLine(line);

                return currency.Name == data.Name && (data.Date >= currency.StartDate && data.Date <= currency.EndDate);

            });

            return lines.Select(line => SplitCurrencyDataFileLine(line)).ToList();
        }

        private static CurrencyData SplitCurrencyDataFileLine(string line) 
        {
            string[] splitLine = line.Split(";");

            string name = splitLine[0];
            DateTime date = DateTime.Parse(splitLine[1]);

            return new CurrencyData
            {
                Date = date,
                Name = name
            };
        }

        private static void ProcessCurrencyPriceFile(List<CurrencyData> currencyDataResult)
        {
            LogToConsole("Processing currency prices file");

            List<string> lines = File.ReadLines("DadosCotacao.csv").ToList();

            lines = lines.FindAll(line => {

                if (line == CurrencyPriceFileHeader)
                    return false;

                CurrencyPriceData data = SplitCurrencyPriceFileLine(line);

                return currencyDataResult.Find(x => x.Name == data.Type.ToString() && x.Date == data.Date) != null;
            });

            List<CurrencyPriceData> currencyPriceResult = lines.Select(currencyPriceLine => SplitCurrencyPriceFileLine(currencyPriceLine)).ToList();

            GenerateResultFile(currencyDataResult, currencyPriceResult);
        }

        private static CurrencyPriceData SplitCurrencyPriceFileLine(string line)
        {
            string[] splitLine = line.Split(";");

            decimal price = decimal.Parse(splitLine[0]);
            int currrencyId = int.Parse(splitLine[1]);
            DateTime date = DateTime.Parse(splitLine[2]);

            return new CurrencyPriceData
            {
                Date = date,
                Price = price,
                Type = (CurrencyType)currrencyId
            };
        }

        private static void GenerateResultFile(List<CurrencyData> currencyData, List<CurrencyPriceData> currencyPriceData)
        {
            LogToConsole($"Generating results file. { currencyPriceData.Count } results found");

            string fileName = $"Resultado_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";

            List<string> resultLines = new List<string>();

            resultLines.Add(ResultingFileHeader);
            resultLines.AddRange(currencyPriceData.Select(x => $"{ x.Type.ToString() };{ x.Date.ToString("yyyy-MM-dd") };{ x.Price.ToString(new CultureInfo("pt-BR")) }").ToList());

            File.WriteAllLines(fileName, resultLines);

            LogToConsole("Processing finished");
        }

        private static void LogToConsole(string message) 
        { 
            Console.WriteLine($"[{ DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") }] - { message }");
        }
    }
}
