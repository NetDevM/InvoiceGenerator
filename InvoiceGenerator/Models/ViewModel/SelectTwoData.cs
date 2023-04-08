namespace InvoiceGenerator.Models.ViewModel
{
    
    public class Result
    {
        public int id { get; set; }
        public string text { get; set; }
        public bool? selected { get; set; }
        public bool? disabled { get; set; }
    }

    public class SelectTwoData
    {
        public List<Result> results { get; set; } 
    }

}
