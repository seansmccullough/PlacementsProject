using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlacementsProject.Models.ViewModels
{
    public class LineItemViewModel
    {
        /// <summary>
        /// LineItem Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id of Campaign associated with LineItem
        /// </summary>
        [Display(Name = "Campaign Id")]
        public int CampaignId { get; set; }

        /// <summary>
        /// Name of Campaign associated with LineItem
        /// </summary>
        public string CampaignName { get; set; }

        /// <summary>
        /// If this LineItem has been marked as Reviewed, and can no longer be modified
        /// </summary>
        public bool Reviewed { get; set; }

        /// <summary>
        /// If the Campaign associated with this LineItem has been reviewed
        /// </summary>
        public bool CampaignReviewed { get; set; }

        /// <summary>
        /// Booked Amount
        /// </summary>
        [Display(Name = "Booked Amount")]
        public double BookedAmount { get; set; }

        /// <summary>
        /// Adjusted Amount
        /// </summary>
        [Display(Name = "Adjusted Amount")]
        public double AdjustedAmount { get; set; }

        /// <summary>
        /// Booked Amount - Adjusted Amount
        /// </summary>
        [Display(Name = "Actual Amount")]
        public double ActualAmount { get; set; }

        /// <summary>
        /// Comments associated with this LineItem
        /// </summary>
        public ICollection<CommentViewModel> Comments { get; set; }

        /// <summary>
        /// Adjustments associated with this LineItem
        /// </summary>
        public ICollection<AdjustmentViewModel> Adjustments { get; set; }

        public LineItemViewModel()
        {
        }

        public LineItemViewModel(LineItem lineItem)
        {
            Id = lineItem.Id;
            CampaignId = lineItem.CampaignId;
            CampaignName = lineItem.Campaign.Name;
            Reviewed = lineItem.Reviewed;
            CampaignReviewed = lineItem.Campaign.Reviewed;
            BookedAmount = lineItem.BookedAmount;
            AdjustedAmount = lineItem.AdjustedAmount;
            ActualAmount = lineItem.ActualAmount;
            if (lineItem.Comments != null)
            {
                Comments = new List<CommentViewModel>();
                foreach (var lineItemComment in lineItem.Comments)
                {
                    Comments.Add(new CommentViewModel(lineItemComment));
                }
            }
            if (lineItem.Adjustments != null)
            {
                Adjustments = new List<AdjustmentViewModel>();
                foreach (var lineItemAdjustment in lineItem.Adjustments)
                {
                    Adjustments.Add(new AdjustmentViewModel(lineItemAdjustment));
                }
            }

        }
    }
}
