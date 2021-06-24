using System;
using System.Collections.Generic;
using System.Text;

namespace MorePayments.Orders
{
    public interface IOrder
    {
        /// <summary>
        /// Платежный идентификатор
        /// </summary>
        int OrderID { get; set; }
        /// <summary>
        /// Платежный номер
        /// </summary>
        string Number { get; set; }
        /// <summary>
        /// Полная сумма
        /// </summary>
        float Sum { get; set; }
        /// <summary>
        /// Стоимость доставки
        /// </summary>
        float ShippingCost { get; set; }
        /// <summary>
        /// Коментарий
        /// </summary>
        string StatusComment { get; set; }
        /// <summary>
        /// Is Draft
        /// Это черновик
        /// </summary>
        bool IsDraft { get; set; }
    }
}
