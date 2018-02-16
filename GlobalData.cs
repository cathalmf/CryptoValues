using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CryptoValues
{
    public class GlobalData
    {
        public float total_market_cap_usd { get; set; }
        public float total_24h_volume_usd { get; set; }
        public float bitcoin_percentage_of_market_cap { get; set; }
        public int active_currencies { get; set; }
        public int active_assets { get; set; }
        public int active_markets { get; set; }
        public int last_updated { get; set; }

        public override string ToString()
        {
            return $"Total Mark Cap: {(total_market_cap_usd.ToString("#,##0,,,B", CultureInfo.InvariantCulture))}";
        }
    }

}
