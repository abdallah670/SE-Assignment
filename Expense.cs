using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal_Bugeting
{
    [Serializable]
    public class Expense
    {
        public ExpenseDTO expense { get; set; }

        public void MarknonRecurring()
        {
            expense.isRecurring = false;
        }
    }
}
