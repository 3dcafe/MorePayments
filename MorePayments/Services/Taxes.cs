using System;
using System.Collections.Generic;
using System.Text;

namespace MorePayments.Services
{
    public enum TaxType
    {
        /// <summary>
        /// Другой
        /// </summary>
        Other = 0,

        /// <summary>
        /// Без НДС
        /// </summary>
        Without = 1,

        /// <summary>
        /// НДС по ставке 0%
        /// </summary>
        Zero = 2,

        /// <summary>
        /// НДС по ставке 10%
        /// </summary>
        Ten = 3,

        /// <summary>
        /// НДС по ставке 18%
        /// </summary>
        Eighteen = 4,
    }
}
