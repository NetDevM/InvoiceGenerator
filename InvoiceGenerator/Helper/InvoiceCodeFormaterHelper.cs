namespace InvoiceGenerator.Helper
{
    /// <summary>
    /// helper for invoicecode generator
    /// </summary>
    public class InvoiceCodeFormaterHelper
    {

        /// <summary>
        /// method to format the Invoice number by interpolating with the unique salesinvoice id
        /// </summary>
        /// <param name="lastsalesinvoiceuniqueid"></param>
        /// <returns></returns>
        public static string GetInvoiceCode(int lastsalesinvoiceuniqueid)
        {
            return $"I{DateTime.Now.Year}{lastsalesinvoiceuniqueid++}";
        }
    }
}
