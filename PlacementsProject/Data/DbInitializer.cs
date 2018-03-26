using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PlacementsProject.Models;

namespace PlacementsProject.Data
{
    /// <summary>
    /// Seeds the database with sample data
    /// </summary>
    public class DbInitializer
    {
        /// <summary>
        /// Loads sample data json file and populates the database
        /// </summary>
        /// <param name="context">ApplicationDbContext</param>
        public static void Initialize(IServiceProvider service)
        {
            using (var serviceScope = service.CreateScope())
            {
                var scopedServiceProvider = serviceScope.ServiceProvider;
                var context = scopedServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();
                if (context.LineItems.Any())
                {
                    return;
                }

                Dictionary<int, Campaign> campaignDictionary = new Dictionary<int, Campaign>();

                using (StreamReader streamReader = new StreamReader(@"DbSeed\placements_teaser_data.json"))
                {
                    string jsonString = streamReader.ReadToEnd();
                    List<RawLineItem> rawLineItems = JsonConvert.DeserializeObject<List<RawLineItem>>(jsonString);
                    foreach (RawLineItem rawLineItem in rawLineItems)
                    {
                        LineItem lineItem = CreateLineItem(rawLineItem);
                        if (!campaignDictionary.ContainsKey(lineItem.CampaignId))
                        {
                            Campaign campaign = CreateCampaign(rawLineItem, lineItem);
                            lineItem.Campaign = campaign;
                            campaignDictionary.Add(lineItem.CampaignId, campaign);
                            context.Campaigns.Add(campaign);
                        }
                        else
                        {
                            campaignDictionary[lineItem.CampaignId].LineItems.Add(lineItem);
                            lineItem.Campaign = campaignDictionary[lineItem.CampaignId];
                        }
                        context.LineItems.Add(lineItem);
                    }
                }

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Creates a LineItem from a RawLineItem
        /// </summary>
        /// <param name="rawLineItem">RawLineItem from the sample data</param>
        /// <returns>LineItem</returns>
        private static LineItem CreateLineItem(RawLineItem rawLineItem)
        {
            return new LineItem()
            {
                Id = rawLineItem.Id,
                CampaignId = rawLineItem.CampaignId,
                Reviewed = false,
                BookedAmount = rawLineItem.BookedAmount,
                AdjustedAmount = rawLineItem.Adjustments,
                ActualAmount = rawLineItem.ActualAmount,
                Comments = new List<Comment>(),
                Adjustments = new List<Adjustment>()
            };
        }

        /// <summary>
        /// Creates a Campaign
        /// </summary>
        /// <param name="rawLineItem">RawLineItem from the sample data</param>
        /// <param name="lineItem">LineItem associated with this Campaign</param>
        /// <returns>Campign</returns>
        private static Campaign CreateCampaign(RawLineItem rawLineItem, LineItem lineItem)
        {
            return new Campaign()
            {
                Id = rawLineItem.CampaignId,
                Name = rawLineItem.CampaignName,
                Reviewed = false,
                LineItems = new List<LineItem>()
                {
                    lineItem
                }
            };
        }
    }
}
