namespace Inventory.Presentation.Models.VM
{
    public class HomeIndexVM
    {
        public int? TotalCategories { get; set; }
        public int? TotalProducts { get; set; }
        public int? TotalSaleOrders { get; set; }
        public int? TotalPurchaseOrders { get; set; }


        public int? TodaySaleOrders { get; set; }
        public int? TodayPurchaseOrders { get; set; }

    }
}
