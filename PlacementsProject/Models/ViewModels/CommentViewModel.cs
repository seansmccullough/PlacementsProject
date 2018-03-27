using System;
using System.ComponentModel.DataAnnotations;

namespace PlacementsProject.Models.ViewModels
{
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

        public CommentViewModel()
        {
        }

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
