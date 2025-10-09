using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4.Persistance
{
    public interface IGameTableDataAccess
    {
        Task<GameTable> LoadAsync(string path);

        Task SaveAsync(string path, GameTable gameTable);
    }
}
