using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tetronimo
{
    [System.Serializable]
    public class TetronimoData
    {
        //-1 -1 | 0 -1 | 1 -1
        //-1  0 | 0  0 | 1  0
        //-1  1 | 0  1 | 1  1
        private Dictionary<Vector2Int, int> cellWeightTable;
        private int[] weights;
        private Vector2Int[] directions;

        public int size;
        public int complexity; //Value from 0 to 10
        public Tile tile;

        public List<Vector2Int> cells { get; private set; }

        private Vector2Int lastDir;

        public TetronimoData(int size, int complexity, Tile tile)
        {
            this.size = size;
            this.complexity = complexity;
            this.tile = tile;

            //Modify cell weights based on the complexity of the tetronimo
            cellWeightTable = new Dictionary<Vector2Int, int>()
            {
                { new Vector2Int(-1, -1), 1 + complexity }, { new Vector2Int(0, -1), 20 - complexity }, { new Vector2Int(1, -1), 1 + complexity },
                { new Vector2Int(-1, 0), 20 - complexity },                                             { new Vector2Int(1, 0), 20 - complexity },
                { new Vector2Int(-1, 1), 1 + complexity },  { new Vector2Int(0, 1), 20 - complexity },  { new Vector2Int(1, 1), 1 + complexity }
            };

            //Generate arrays for cell weights and directions
            weights = cellWeightTable.Values.ToArray();
            directions = cellWeightTable.Keys.ToArray();

            //Create the Cells list
            cells = new List<Vector2Int>();

            //Generate the first cell at 0,0
            Vector2Int curCell = new Vector2Int(0, 0);
            lastDir = new Vector2Int(0, 0);
            cells.Add(curCell);

            //Create additional cells until we have a full tetronimo based on the required size
            for (int i = 1; i < size; i++)
            {
                cells.Add(RandomWeightedCell(curCell));
                curCell = cells[i];
            }
        }

        private Vector2Int RandomWeightedCell(Vector2Int curCell)
        {
            Vector2Int cell = curCell;
            int randomWeight = Random.Range(0, weights.Sum());

            for (int i = 0; i < weights.Length; i++)
            {
                randomWeight -= weights[i];
                if (randomWeight < 0)
                {
                    cell = curCell + directions[i];
                    if (cells.Contains(cell))
                    {
                        int n = i;
                        while (cells.Contains(cell))
                        {
                            n++;
                            if(n >= directions.Length)
                            {
                                n = 0;
                            }
                            else if(n == i)
                            {
                                Debug.LogError("Unable to find valid position for cell");
                                break;
                            }

                            cell = curCell + directions[n];
                        }
                    }
                    break;
                }
            }

            return cell;
        }
    }
}
