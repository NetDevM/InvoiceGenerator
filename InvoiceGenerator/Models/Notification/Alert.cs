namespace InvoiceGenerator.Models.Notification
{
    public static class MyAlerts
    {
        public const string SUCCESS = "success";
        public const string ATTENTION = "attention";
        public const string ERROR = "error";
        public const string INFORMATION = "info";

        public static string[] ALL
        {
            get
            {
                return new[]
                           {
                           SUCCESS,
                           ATTENTION,
                           INFORMATION,
                           ERROR
                       };
            }
        }

    }
}
