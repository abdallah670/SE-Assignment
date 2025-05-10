using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal_Bugeting
{
    internal class Budget
    {
        public int Id { get; set; }
        public string category { get; set; }
        public decimal spent { get; set; }
        public decimal limit { get; set; }
        public DateTime periodStart { get; set; }
        public DateTime periodEnd { get; set; }
        public double getprogress()
        {
            if (limit == 0)
            {
                return 0;
            }
            return (double)(spent / limit) * 100;
        }
        public bool isExceeded()
        {
            return spent > limit;
        }
    }
}
