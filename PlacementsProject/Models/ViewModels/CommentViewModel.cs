using System;
using System.ComponentModel.DataAnnotations;

namespace PlacementsProject.Models.ViewModels
{
    /// <summary>
    /// View model for Comment
    /// </summary>
    public class CommentViewModel
    {
        /// <summary>
        /// Comment Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Id of LineItem associated with this Comment
        /// </summary>
        [Display(Name = "Line Item Id")]
        public int LineItemId { get; set; }

        /// <summary>
        /// Id of User who created this Comment
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// User Email
        /// </summary>
        [Display(Name = "User")]
        public string UserEmail { get; set; }

        /// <summary>
        /// Text of Comment
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// DateTime when Comment was created
        /// </summary>
        [Display(Name = "Last Modified Time")]
        public DateTime ModifiedDateTime { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CommentViewModel()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="comment">Comment</param>
        public CommentViewModel(Comment comment)
        {
            Id = comment.Id;
            LineItemId = comment.LineItemId;
            UserEmail = comment.User.Email;
            UserId = comment.UserId;
            Text = comment.Text;
            ModifiedDateTime = comment.ModifiedDateTime;
        }
    }
}
