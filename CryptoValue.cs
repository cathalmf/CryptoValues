using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptoValues
{
    public class CryptoObject
    {
        public string id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public string rank { get; set; }
        public string price_usd { get; set; }
        public string price_btc { get; set; }
        public string _24h_volume_usd { get; set; }
        public string market_cap_usd { get; set; }
        public string available_supply { get; set; }
        public string total_supply { get; set; }
        public string max_supply { get; set; }
        public string percent_change_1h { get; set; }
        public string percent_change_24h { get; set; }
        public string percent_change_7d { get; set; }
        public string last_updated { get; set; }

        public override string ToString()
        {
            return $"{symbol.PadRight(7, ' ')} {name.PadRight(20, ' ')} {price_usd.PadRight(10, ' ')} {percent_change_1h.PadRight(7, ' ')} {percent_change_24h.PadRight(7, ' ')} {percent_change_7d.PadRight(7, ' ')}";
        }

        public string ToString(string holding)
        {
            return $"{symbol.PadRight(7, ' ')} {name.PadRight(20, ' ')} {price_usd.PadRight(10, ' ')} {holding.PadRight(13, ' ')} {percent_change_1h.PadRight(7, ' ')} {percent_change_24h.PadRight(7, ' ')} {percent_change_7d.PadRight(7, ' ')}";
        }
    }
}
