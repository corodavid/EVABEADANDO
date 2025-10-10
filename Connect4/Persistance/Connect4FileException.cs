using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4.Persistance
{
    public class Connect4FileException : Exception
    {
        public Connect4FileException() { }
        public Connect4FileException(string message) : base(message) { }
    }
}
