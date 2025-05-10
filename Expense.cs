using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal_Bugeting
{
    internal class Expense
    {
        int Id { get; set; }
        decimal amount { get; set; }
        DateTime date { get; set; }
        bool isRecurring { get; set; }
        public void MarknonRecurring()
        {
            isRecurring = false;
        }
    }
}
