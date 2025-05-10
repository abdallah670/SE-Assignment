using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal_Bugeting
{
    internal class Goal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal targetAmount { get; set; }
        public DateTime targetDate { get; set; }
        public decimal currentAmount { get; set; }
        public Dictionary<DateTime, decimal> CalculateTimeLine()
        {
            Dictionary<DateTime, decimal> timeline = new Dictionary<DateTime, decimal>();
            DateTime currentDate = DateTime.Now;
            TimeSpan timeRemaining = targetDate - currentDate;
            decimal amountPerDay = (targetAmount - currentAmount) / (decimal)timeRemaining.TotalDays;
            for (int i = 0; i <= timeRemaining.Days; i++)
            {
                DateTime date = currentDate.AddDays(i);
                decimal amount = currentAmount + (amountPerDay * i);
                timeline[date] = amount;
            }
            return timeline;

        }
        public void AddToGoal(decimal amount)
        {
            currentAmount += amount;
        }
    }
}
