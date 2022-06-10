Console.WriteLine("Hello, World! Start pay:");
Console.WriteLine("Enter login:");
string loginYookassa = Console.ReadLine();
Console.WriteLine("Enter Passoword");
string passwordYookassa = Console.ReadLine();
MorePayments.Payment.Yookassa.YookassaService service = new MorePayments.Payment.Yookassa.YookassaService(int.Parse(loginYookassa), passwordYookassa);

var data = await service.CreatePaymentAsync
    (
    new MorePayments.Payment.Yookassa.YookassaPayment() 
    { 
        amount = new MorePayments.Payment.Yookassa.YookassaAmount { currency = "RUB", value = "2.00" },
        confirmation = new MorePayments.Payment.Yookassa.YookassaConfirmation()
        {
            return_url = "https://3dcafe.ru",
            type = "redirect"
        },
        description = Guid.NewGuid().ToString(),
        payment_method_data = new MorePayments.Payment.Yookassa.YookassaPaymentMethodData()
        {
             type = "bank_card"
        } 
    }, Guid.NewGuid().ToString()
);
Console.WriteLine("Payment Created");
var info = await service.GetPaymentAsync(data.id);
if(info.paid)
{
    Console.WriteLine(info.paid);
}
else
{
    Console.WriteLine("pls set breakpoint for paid");
}