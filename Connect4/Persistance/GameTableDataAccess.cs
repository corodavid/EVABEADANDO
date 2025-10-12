using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Connect4.Persistance
{
    public class GameTableDataAccess : IGameTableDataAccess
    {
        public GameTableDataAccess() {}

        public async Task<(int, int, GameTable)> LoadAsync(string path)
        { 
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string line = await reader.ReadLineAsync() ?? String.Empty;
                    string[] split = line.TrimEnd().Split(' ');
                    int size;
                    int firstRemainingTime, secondRemainingTime;
                    int.TryParse(split[0], out size);
                    int.TryParse(split[1], out firstRemainingTime);
                    int.TryParse(split[2], out secondRemainingTime);
                    GameTable table = new GameTable(size);
                    for(int i = 0; i < size; i++)
                    {
                        line = await reader.ReadLineAsync() ?? String.Empty;
                        split = line.TrimEnd().Split(' ');
                        for(int j = 0; j <  size; j++)
                        {
                            switch(split[j])
                            {
                                case "X":
                                    table[i, j] = FieldStatus.X;
                                    break;
                                case "O":
                                    table[i, j] = FieldStatus.O;
                                    break;
                                case "NONE":
                                    table[i, j] = FieldStatus.NONE;
                                    break;
                                default:
                                    throw new Connect4FileException();
                            }
                        }
                    }

                    return (firstRemainingTime, secondRemainingTime, table);
                }
            }
            catch(Exception e) 
            {
                throw new Connect4FileException("",e);
            }
        }

        public async Task SaveAsync(string path, int firstRemainingTime, int secondRemainingTime, GameTable gameTable)
        {
            if(path == null)
                throw new ArgumentNullException("path");
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.WriteLineAsync($"{gameTable.Size} {firstRemainingTime} {secondRemainingTime}");
                    for(int i = 0; i < gameTable.Size; i++)
                    {
                        for(int j = 0; j < gameTable.Size; j++)
                        {
                            writer.WriteAsync($"{gameTable[i, j].ToString()} ");
                        }
                        writer.WriteAsync("\b\n");
                    }
                }
            }
            catch(Exception e) 
            {
                throw new Connect4FileException(e.Message, e);
            }
        }
    }
}
