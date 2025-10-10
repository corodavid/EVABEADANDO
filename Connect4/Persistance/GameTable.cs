using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Connect4.Persistance;

namespace Connect4.Persistance
{
    public class GameTable
    {
        #region Fields

        private readonly int _size;

        /// <summary>
        /// The board is flipped
        /// (0,0) (0,1) ... (0,n)
        /// .   .             .  
        /// .      .          .  
        /// .          .      .  
        /// (n,0) (n,1) ... (n,n)
        /// </summary>
        private FieldStatus[,] _fields;

        #endregion

        #region Properties

        public Boolean IsFilled
        {
            get
            {
                for(int i = 0; i < _size; i++) 
                {
                    for(int j = 0; j < _size; j++)
                    {
                        if (_fields[i, j] == FieldStatus.NONE)
                            return false;
                    }
                }
                return true;
            }
        }
        public FieldStatus this[int row, int col]
        {
            get
            {
                if (row < 0 || col < 0 || row >= _size || col >= _size)
                {
                    throw new IndexOutOfRangeException($"Getter wanted to access\n row:{row} col:{col}  range is 0..{_size-1}");
                }
                return _fields[row, col]; 
                
            }
            set 
            {
                if(row < 0 || col < 0 || row >= _size || col >= _size)
                {
                    throw new IndexOutOfRangeException($"Setter wanted to access\n row:{row} col:{col}  range is 0..{_size-1}");
                }
                if (_fields[row, col] != FieldStatus.NONE)
                    throw new FieldIsNotNoneException($"Setter tried to set not NONE field\n row:{row} col:{col}  range is 0..{_size - 1}");
                _fields[row, col] = value;
            }
        }

        public int Size { get { return _size; } }

        #endregion

        #region Constructors
        public GameTable(int n) 
        {
            if (n <= 0)
                throw new ArgumentOutOfRangeException($"{nameof(n)} is negative");
            _size = n;
            _fields = new FieldStatus[n,n]; //Automatically NONE as it is 0 in the enum.
        }



        public GameTable(int n, FieldStatus[,] fields)
        {
            if (n < 0)
                throw new ArgumentOutOfRangeException($"Size is negative: {n}");

            if (fields == null)
                throw new ArgumentNullException(nameof(fields));

            int rows = fields.GetLength(0);
            int cols = fields.GetLength(1);

            if (rows != cols)
                throw new ArgumentException($"Fields must be a square matrix, but got {rows}x{cols}.", nameof(fields));

            if (rows != n)
                throw new ArgumentException($"Field size ({rows}x{cols}) does not match n={n}.", nameof(fields));
            _size = n;
            _fields = fields;
        }




        #endregion

        #region Public Methdods
        
        public Boolean IsNONE(int row, int col)
        {
            if(row < 0 || row >= _size || col < 0 || col >= _size)
            {
                throw new ArgumentOutOfRangeException($"row:{row} col:{col}  range is 0..{_size - 1}");
            }
            return _fields[row, col] == FieldStatus.NONE;
        }
        

        public bool ColumnDoesntHaveGaps(int col)
        {
            bool alreadyHadGap = false;
            for(int i = _size -1; i >= 0; i--)
            {

                if (alreadyHadGap && _fields[i, col] != FieldStatus.NONE)
                    return false;
                else if (!alreadyHadGap && _fields[i,col] == FieldStatus.NONE)
                {
                    alreadyHadGap = true;
                }
            }
            return true;
        }


        public Boolean IsColumnFull(int col)
        {
            if (col < 0 || col >= _size)
                
                throw new ArgumentOutOfRangeException($"col:{col}  range is 0..{_size - 1}");
            for (int i = _size - 1; i >= 0; i--)
            {
                if (_fields[i, col] == FieldStatus.NONE)
                    return false;
            }
            return true;
        }

        public int FirstNoneFieldInColumn(int col)
        {
            if (col < 0 || col >= _size)
                throw new ArgumentOutOfRangeException($"col:{col}  range is 0..{_size - 1}");
            for (int i = _size - 1; i >= 0; i--)
            {
                if (_fields[i, col] == FieldStatus.NONE)
                    return i;
            }
            //throw new Exception($"Column is full : {col}");
            return -1;
        }


        #endregion

        #region Private Methods




        #endregion
    }
}
