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
                using (StreamReader reader = new (path))
                {
                    string line = await reader.ReadLineAsync() ?? String.Empty;
                    string[] split = line.TrimEnd().Split(' ');
                    if (split.Length != 3 ||
                    !int.TryParse(split[0], out int size) ||
                    !int.TryParse(split[1], out int firstRemainingTime) ||
                    !int.TryParse(split[2], out int secondRemainingTime))
                        throw new Exception();
                    GameTable table = new (size);
                    for(int i = 0; i < size; i++)
                    {
                        line = await reader.ReadLineAsync() ?? String.Empty;
                        split = line.TrimEnd().Split(' ');
                        for(int j = 0; j <  size; j++)
                        {
                            table[i, j] = split[j] switch
                            {
                                "X" => FieldStatus.X,
                                "O" => FieldStatus.O,
                                "NONE" => FieldStatus.NONE,
                                _ => throw new Exception()
                            };
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
                throw new ArgumentNullException(nameof(path));
            try
            {
                await using (StreamWriter writer = new (path))
                {
                    await writer.WriteLineAsync($"{gameTable.Size} {firstRemainingTime} {secondRemainingTime}");
                    for(int i = 0; i < gameTable.Size; i++)
                    {
                        for(int j = 0; j < gameTable.Size; j++)
                        {
                            await writer.WriteAsync($"{gameTable[i, j]} ");
                        }
                        await writer.WriteAsync("\b\n");
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
