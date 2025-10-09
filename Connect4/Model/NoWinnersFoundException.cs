using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4.Model
{
    public class NoWinnersFoundException : Exception
    {
        public NoWinnersFoundException() { }    
        public NoWinnersFoundException(string message) : base(message) { }
        public NoWinnersFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
