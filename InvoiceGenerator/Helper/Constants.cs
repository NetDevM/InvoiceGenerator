namespace InvoiceGenerator.Helper
{
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
