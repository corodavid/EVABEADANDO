using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4.Persistance
{
    public class GameTableDataAccess : IGameTableDataAccess
    {
        private readonly GameTable _table = null;
        public GameTableDataAccess() {}

        public async Task<GameTable> LoadAsync(string path)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync(string path, GameTable gameTable)
        {
            throw new NotImplementedException();
        }
    }
}
