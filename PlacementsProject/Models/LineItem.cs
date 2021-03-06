﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementsProject.Models
{
    /// <summary>
    /// Represents a LineItem
    /// </summary>
    public class LineItem
    {
        /// <summary>
        /// Id, primary key
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// CampaignId, foreign key
        /// </summary>
        [Display(Name = "Campaign Id")]
        public int CampaignId { get; set; }

        /// <summary>
        /// If this LineItem has been marked as Reviewed, and can no longer be modified
        /// </summary>
        public bool Reviewed { get; set; }

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
        /// Campaign associated with this LineItem
        /// </summary>
        public Campaign Campaign { get; set; }

        /// <summary>
        /// Comments associated with this LineItem
        /// </summary>
        public ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// Adjustments associated with this LineItem
        /// </summary>
        public ICollection<Adjustment> Adjustments { get; set; }
    }
}
