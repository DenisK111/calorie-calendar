using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounting.Models.Entities
{
    public class Day
    {
        public int Id { get; set; }
        public required DateOnly Date { get; set; }
        public required decimal? Weight { get; set; }
        public List<Entry> Entries { get; set; } = new();
    }
}
