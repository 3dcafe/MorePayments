// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
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


#warning Check payment