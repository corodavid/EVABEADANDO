using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4.Persistance
{
    public class FieldIsNotNoneException : Exception
    {
        public FieldIsNotNoneException() : base() { }

        public FieldIsNotNoneException(string message) : base(message) { }

        public FieldIsNotNoneException(string message, Exception e) : base(message, e) { }

    }
}
