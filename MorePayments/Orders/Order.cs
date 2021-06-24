using System;
using System.Collections.Generic;
using System.Text;

namespace MorePayments.Orders
{
#warning нуждается в докуменирование
    public class Order : IOrder
    {
        public int OrderID { get; set; }
        public string Number { get; set; }
        public float Sum { get; set; }
        public float ShippingCost { get; set; }
        public string StatusComment { get; set; }
        public bool IsDraft { get; set; }
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string userId { get; set; }
        public OrderCurrency OrderCurrency { get; set; }
    }
}
