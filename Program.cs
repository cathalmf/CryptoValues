using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace CryptoValues
{
    class Program
    {
        static void Main(string[] args)
        {
            var Portfolio = ParseTickers();

            decimal LastPortfolioValue = 0;
            string changestr = string.Empty;

            while(true)
            {
                var tickers = Portfolio.Select(k => k.Key).ToList();   

                string ticketjson = string.Empty;
                string globaljson = string.Empty;

                try
                {
                    ticketjson = new WebClient().DownloadString("https://api.coinmarketcap.com/v1/ticker/?limit=0");
                    globaljson = new WebClient().DownloadString("https://api.coinmarketcap.com/v1/global/");

                }
                catch (Exception)
                {
                    Thread.Sleep(1000);
                    continue;                    
                }

                var ticketresult = JsonConvert.DeserializeObject<List<CryptoObject>>(ticketjson);
                var globalresult = JsonConvert.DeserializeObject<GlobalData>(globaljson);

                var wanted = ticketresult.Where(t => tickers.Contains(t.symbol)).ToList();

                if(wanted.Count == 0)
                {
                    Console.WriteLine("No Tickers configured. See the CryptoValues.exe.config file");
                    Console.ReadLine();
                }

                ClearLines(wanted.Count + 1 + 1);

                Console.WriteLine($"{"Symbol".PadRight(7, ' ')} {"Name".PadRight(20, ' ')} {"Price USD".PadRight(10, ' ')} {"Holding".PadRight(13, ' ')} {"% 1H".PadRight(7, ' ')} {"% 24H".PadRight(7, ' ')} {"% 7D".PadRight(7, ' ')}");
                decimal total1h = 0;
                decimal total24h = 0;
                decimal total7d = 0;
                
                foreach(var w in wanted)
                {
                    Console.WriteLine(w.ToString(Portfolio.Where(p => p.Key == w.symbol).FirstOrDefault().Value.ToString()));
                    total1h += decimal.Parse(w.percent_change_1h);
                    total24h += decimal.Parse(w.percent_change_24h);
                    total7d += decimal.Parse(w.percent_change_7d);
                }

                total1h = total1h / wanted.Count;
                total24h = total24h / wanted.Count;
                total7d = total7d / wanted.Count;

                Console.WriteLine($"{"".PadRight(7, ' ')} {"".PadRight(20, ' ')} {"".PadRight(10, ' ')} {"".PadRight(13, ' ')} {total1h.ToString("#.##").PadRight(7, ' ')} {total24h.ToString("#.##").PadRight(7, ' ')} {total7d.ToString("#.##").PadRight(7, ' ')}");

                Console.WriteLine($"{globalresult.ToString()}");

                decimal currentPortfolioValue = PortfolioValue(wanted, Portfolio);

                Console.WriteLine($"Portfolio Value: {currentPortfolioValue.ToString("#.##")}");
                if(currentPortfolioValue > LastPortfolioValue)
                {
                    changestr += "+";
                }
                else if(currentPortfolioValue < LastPortfolioValue)
                {
                    changestr += "-";
                }
                else
                {
                    changestr += "0";
                }

                Console.WriteLine(changestr);

                LastPortfolioValue = currentPortfolioValue;

                Thread.Sleep((int)(2.5 * 60 * 1000));
                         
            }

        }

        

        // Clears the line count from (0,0) and returns to (0,0)
        private static void ClearLines(int count)
        {
            Console.SetCursorPosition(0, 0);
            for(int i= 0; i< count; i++)
            {
                Console.WriteLine();
            }

            Console.SetCursorPosition(0, 0);
        }

        private static decimal PortfolioValue(List<CryptoObject> objects, Dictionary<string, decimal> portfolio)
        {
            decimal total = 0;

            foreach(var cry in objects)
            {
                decimal tickervalue = decimal.Parse(cry.price_usd);

                total += tickervalue * portfolio[cry.symbol];
            }

            return total;
        }

        private static Dictionary<string, decimal> ParseTickers()
        {
            Dictionary<string, decimal> Mapping = new Dictionary<string, decimal>();

            var settings = System.Configuration.ConfigurationManager.GetSection("appSettings") as NameValueCollection;

            foreach(string key in settings)
            {
                string ticker = key.ToUpper();
                decimal holding = 0; 
                try
                {
                    holding = decimal.Parse(settings[key]);
                }
                catch (Exception ex)
                {
                    holding = 0;
                }

                Mapping[ticker] = holding;
            }

            return Mapping;
        }


    }
}
