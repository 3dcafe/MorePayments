namespace MorePayments.Payment.Yookassa
{
    public class YookassaConfirmation
    {
        public string type { get; set; }
        public string return_url { get; set; }
        public string confirmation_url { get; set; }
    }
}
