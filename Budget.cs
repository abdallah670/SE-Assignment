using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Personal_Bugeting
{
    [Serializable]
    public class Budget
    {
       public BudgetDTO _budgetDTO {  get; set; }
       public UserDTO User { get; set; }
        public decimal getprogress()
        {
            if ( _budgetDTO.spent== 0)
            {
                return 0;
            }
            return (decimal)(_budgetDTO.spent / _budgetDTO.limit) * 100;
        }
        public bool isExceeded()
        {
            return _budgetDTO.spent > _budgetDTO.limit;
        }
    }
}
