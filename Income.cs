﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal_Bugeting
{
    [Serializable]
    public class Income
    {
       public IncomeDTO income { get; set; }
    
        public void MarknonRecurring()
        {
            income.isRecurring = false;
        }
    }
}
