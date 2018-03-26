using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementsProject.Models
{
    /// <summary>
    /// Represents a Campaign
    /// </summary>
    public class Campaign
    {
        /// <summary>
        /// Campaign Id, primary key.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Campaign name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// If this Campaign has been marked as reviewed, and it's LineItems can no longer be modified
        /// </summary>
        public bool Reviewed { get; set; }

        /// <summary>
        /// LineItems associated with this Campaign
        /// </summary>
        public ICollection<LineItem> LineItems { get; set; }
    }
}
