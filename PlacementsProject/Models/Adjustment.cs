using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementsProject.Models
{
    /// <summary>
    /// Represents Adjustment User can make to a LineItem
    /// </summary>
    public class Adjustment
    {
        /// <summary>
        /// Adjustment Id, primary key.
        /// Actually a guid, but it is a string here to be consistent with ApplicationUserId,
        /// which was generated as part of the boilerplate project
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        /// <summary>
        /// LineItem Id, foreign key
        /// </summary>
        public int LineItemId { get; set; }

        /// <summary>
        /// User Id, foreign key
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Amount to adjust LineItem BookedAmount
        /// </summary>
        [Display(Name= "Adjustment Amount")]
        [Range(0, Double.MaxValue)]
        public double AdjustmentAmount { get; set; }

        /// <summary>
        /// DateTime when this Adjustment was created
        /// </summary>
        [Display(Name="Adjustment Time")]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// LineItem associated with Adjustment
        /// </summary>
        public LineItem LineItem { get; set; }

        /// <summary>
        /// ApplicationUser associated with this Adjustment
        /// </summary>
        public ApplicationUser User { get; set; }
    }
}
