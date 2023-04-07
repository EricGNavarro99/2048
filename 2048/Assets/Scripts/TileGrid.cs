using UnityEngine;
using Unity.Tile.Row;
using Unity.Tile.Cell;

namespace Unity.Tile.Grid
{
    public class TileGrid : MonoBehaviour
    {
        public TileRow[] rows { get; private set; }
        public TileCell[] cells { get; private set; }

        public int size => this.cells.Length;
        public int height => this.rows.Length;
        public int width => this.size / this.height;

        private void Awake()
        {
            this.rows = GetComponentsInChildren<TileRow>();
            this.cells = GetComponentsInChildren<TileCell>();
        }

        private void Start()
        {
            for (int y = 0; y < this.rows.Length; y++)
                for (int x = 0; x < this.rows[y].cells.Length; x++)
                    this.rows[y].cells[x].coordinates = new Vector2Int(x, y);
        }

        public TileCell GetCell(int x, int y)
        {
            if (x >= 0 && x < this.width && y >= 0 && y < this.height) 
                return this.rows[y].cells[x];
            else 
                return null;
        }

        public TileCell GetCell(Vector2Int coordinates) => GetCell(coordinates.x, coordinates.y);

        public TileCell GetAdjacentCell(TileCell cell, Vector2Int direction)
        {
            Vector2Int coordinates = cell.coordinates;
            coordinates.x += direction.x;
            coordinates.y -= direction.y;

            return GetCell(coordinates);
        }

        public TileCell GetRandomEmptyCell()
        {
            int index = Random.Range(0, this.cells.Length);
            int startingIndex = index;

            while (this.cells[index].occupied)
            {
                index++;

                if (index >= this.cells.Length) index = 0;

                if (index == startingIndex) return null;
            }

            return this.cells[index];
        }
    }
}
