namespace InvoiceGenerator.Helper
{
    public class InvoiceCodeFormaterHelper
    {
        public static string GetInvoiceCode(int lastsalesinvoiceuniqueid)
        {
            return $"I{DateTime.Now.Year}{lastsalesinvoiceuniqueid++}";
        }
    }
}
