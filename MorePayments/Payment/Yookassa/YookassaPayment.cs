namespace MorePayments.Payment.Yookassa
{
    public class YookassaPaymentMethodData
    {
        public string type { get; set; }
    }

    public class YookassaPayment
    {
        public YookassaAmount amount { get; set; }
        public YookassaPaymentMethodData payment_method_data { get; set; }
        public YookassaConfirmation confirmation { get; set; }
        public string description { get; set; }
    }
}
