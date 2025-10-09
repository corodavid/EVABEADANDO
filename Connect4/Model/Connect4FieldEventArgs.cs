using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4.Model
{
    public class Connect4FieldEventArgs : EventArgs
    {
        private int _x;
        private int _y;

        public int X { get { return _x; } }
        public  int Y { get { return _y; } }
        
        public Connect4FieldEventArgs(int x, int y)
        {
            _x = x;
            _y = y;
        }

    }
}
