using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace OESK
{
    public class TableAlgorithm
    {
        [Key]
        public int IDAlgorithm { get; set; }
        public string Name { get; set; }
    }
}
