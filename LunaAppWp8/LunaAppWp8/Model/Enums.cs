using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunaAppWp8.Model
{

    public enum ValidationEnum
    { 
        NoNeedForValidation=0,
        StartDateInFuture=1,
        EndDateTooCloseToStart =2,
        EndDateBeforeStart=3,
        DateOverlappsExistingPeriod=4,
        EndDateFarInTheFuture =5
    }
}
