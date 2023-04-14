namespace InvoiceGenerator.Helper
{
    /// <summary>
    /// Payment status for displaying the status later
    /// </summary>
    public class PaymentStatusConstants
    {

        public static string Due
        {
            get
            {
                return "due";
            }
        }

        public static string FullPayment
        {
            get
            {
                return "full payment";
            }
        }

        public static string PartialPayment
        {
            get
            {
                return "partial payment";
            }
        }
    }


    /// <summary>
    /// payment method for sales invoice
    /// </summary>
    public class PaymentMethodConstants
    {

        public static string Cash
        {
            get
            {
                return "cash";
            }
        }

        public static string Cheque
        {
            get
            {
                return "cheque";
            }
        }

        public static string Card
        {
            get
            {
                return "card";
            }
        }



    }
}
