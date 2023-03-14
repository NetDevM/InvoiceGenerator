namespace InvoiceGenerator.Models
{
    public class DashBoardViewModel
    {
        public List<SalesInvoice>? SalesInvoices { get; set; }

        public float TotalRevenue { get; set; }

        public float TotalSales { get; set; }

        public int TotalProducts { get; set; }

        public int TotalCustomers { get; set; }

        public StoreSettings StoreSettings { get; set; }
    }
}
