using System.Collections.Generic;

namespace PlacementsProject.Models.ViewModels
{
    /// <summary>
    /// View model for 
    /// </summary>
    public class LineItemIndexViewModel : PaginatedList<LineItemViewModel>
    {
        public double TotalBookedAmount { get; set; }
        public double TotalAdjustedAmount { get; set; }
        public double TotalActualAmount { get; set; }

        public LineItemIndexViewModel(List<LineItemViewModel> items, int count, int pageIndex, int pageSize,
            double totalBookedAmount, double totalAdjustedAmount, double totalActualAmount)
            : base(items, count, pageIndex, pageSize)
        {
            TotalBookedAmount = totalBookedAmount;
            TotalAdjustedAmount = totalAdjustedAmount;
            TotalActualAmount = totalActualAmount;
        }
    }
}
