using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlacementsProject.Models.ViewModels
{
    public class CampaignViewModel
    {
        /// <summary>
        /// Campaign Id
        /// </summary>
        [Display(Name = "Campaign Id")]
        public int Id { get; set; }

        /// <summary>
        /// Campaign name
        /// </summary>
        [Display(Name = "Campaign Name")]
        public string Name { get; set; }

        /// <summary>
        /// If this Campaign has been marked as reviewed, and it's LineItems can no longer be modified
        /// </summary>
        public bool Reviewed { get; set; }

        /// <summary>
        /// LineItems associated with this Campaign
        /// </summary>
        public ICollection<LineItemViewModel> LineItems { get; set; }

        public CampaignViewModel()
        {
        }

        public CampaignViewModel(Campaign campaign)
        {
            Id = campaign.Id;
            Name = campaign.Name;
            Reviewed = campaign.Reviewed;
            if (campaign.LineItems != null)
            {
                LineItems = new List<LineItemViewModel>();
                foreach (var campaignLineItem in campaign.LineItems)
                {
                    LineItems.Add(new LineItemViewModel(campaignLineItem));
                }
            }
        }
    }
}
