using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OESK
{
    public class TableTest
    {
        [Key]
        public int IDTest { get; set; }
        public string DateTime { get; set; }

        public virtual List<TableTestResult> TableTestResult { get; set; }
    }
}
