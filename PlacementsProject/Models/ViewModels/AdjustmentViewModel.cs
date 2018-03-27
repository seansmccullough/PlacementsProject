using System;
using System.ComponentModel.DataAnnotations;

namespace PlacementsProject.Models.ViewModels
{
    public class AdjustmentViewModel
    {
        /// <summary>
        /// Adjustment Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// LineItem Id, foreign key
        /// </summary>
        public int LineItemId { get; set; }

        /// <summary>
        /// Email of User who created Adjustment
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// Amount to adjust LineItem BookedAmount
        /// </summary>
        [Display(Name = "Adjustment Amount")]
        [Range(0, Double.MaxValue)]
        public double AdjustmentAmount { get; set; }

        /// <summary>
        /// DateTime when this Adjustment was created
        /// </summary>
        [Display(Name = "Adjustment Time")]
        public DateTime DateTime { get; set; }

        public AdjustmentViewModel()
        {
        }

        public AdjustmentViewModel(Adjustment adjustment)
        {
            Id = adjustment.Id;
            LineItemId = adjustment.LineItemId;
            UserEmail = adjustment.User.Email;
            AdjustmentAmount = adjustment.AdjustmentAmount;
            DateTime = adjustment.DateTime;
        }
    }
}
