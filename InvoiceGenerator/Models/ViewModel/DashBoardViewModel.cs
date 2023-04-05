namespace InvoiceGenerator.Models
{
    public class DashBoardViewModel
    {
        public List<SalesInvoice>? SalesInvoices { get; set; }

        public float TotalRevenue { get; set; } = 0;

        public float TotalSales { get; set; } = 0;

        public int TotalProducts { get; set; } = 0;

        public int TotalCustomers { get; set; } = 0;

        public StoreSettings? StoreSettings { get; set; }
    }
}
