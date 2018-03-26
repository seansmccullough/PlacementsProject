using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PlacementsProject.Data
{
    /// <summary>
    /// Represents in a LineItem from the sample data.
    /// Used by DbInitializer to parse sample data.
    /// </summary>
    public class RawLineItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("campaign_id")]
        public int CampaignId { get; set; }

        [JsonProperty("campaign_name")]
        public string CampaignName { get; set; }

        [JsonProperty("line_item_name")]
        public string LineItemName { get; set; }

        [JsonProperty("booked_amount")]
        public double BookedAmount { get; set; }

        [JsonProperty("actual_amount")]
        public double ActualAmount { get; set; }

        [JsonProperty("adjustments")]
        public double Adjustments { get; set; }
    }
}
