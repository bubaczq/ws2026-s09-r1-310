using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verseny
{
    internal class Error
    {
        public int RowNumber { get; set; }
        public string Description { get; set; }

        public Error(int rowNumber, string description)
        {
            RowNumber = rowNumber;
            Description = description;
        }
    }
}
