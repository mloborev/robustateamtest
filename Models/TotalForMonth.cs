using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskRobusta.Models
{
    class TotalForMonth
    {
        public int Id { get; set; }
        public int Clicks { get; set; }
        public int Deps { get; set; }
        public int Regs { get; set; }
        public DateTime DateOfParse { get; set; }
    }
}
