using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Common;
namespace StockStrategy.Common
{
   public class Mapping
    {
        public static List<DataModel.Common.DayOfWeek> DayOfWeekList = new List<DataModel.Common.DayOfWeek>
        {
            new DataModel.Common.DayOfWeek {Day="Monday",Show="一" },
            new DataModel.Common.DayOfWeek {Day="Tuesday",Show="二" },
            new DataModel.Common.DayOfWeek {Day="Wednesday",Show="三" },
            new DataModel.Common.DayOfWeek {Day="Thursday",Show="四" },
            new DataModel.Common.DayOfWeek {Day="Friday",Show="五" },
            new DataModel.Common.DayOfWeek {Day="Saturday",Show="六" },
            new DataModel.Common.DayOfWeek {Day="Sunday",Show="日" }  
        };
    }
}
