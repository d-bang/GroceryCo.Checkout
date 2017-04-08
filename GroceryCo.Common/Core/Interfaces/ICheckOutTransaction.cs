using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCo.Common.Core.Interfaces
{
    public interface ICheckOutTransaction
    {
        decimal AmountDue { get; }
        string PrintReceipt();
    }
}
