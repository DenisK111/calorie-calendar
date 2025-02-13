using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounting.Models.Entities
{
    public class Entry
    {
        public int Id { get; set; }
        public int DayId { get; set; }
        public Day Day { get; set; } = null!; // Navigation Property
        public required int Calories { get; set; }
        public required Macros Macros { get; set; } = null!;
        public required TimeSpan Time { get; set; }       
    }
}
