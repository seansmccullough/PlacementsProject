using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementsProject.Models
{
    /// <summary>
    /// Represents a Comment a User can leave on a LineItem
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// CommentId, primary key.
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
        /// Text of Comment
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// DateTime when Comment was created
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// LineItem associated with Comment
        /// </summary>
        public LineItem LineItem { get; set; }

        /// <summary>
        /// ApplicationUser who created Comment
        /// </summary>
        public ApplicationUser User { get; set; }
    }
}
