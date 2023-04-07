using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Tile.State;
using Unity.Tile.Grid;
using Unity.Tile.Cell;
using Unity.Manager;

namespace Unity.Tile.Board
{    
    public class TileBoard : MonoBehaviour
    {
        public GameManager gameManager;
        [Space]
        public Tile tilePrefab;
        public TileState[] tileStates;

        private TileGrid grid;
        private List<Tile> tiles;

        private bool waiting;

        private void Awake()
        {
            this.grid = GetComponentInChildren<TileGrid>();
            this.tiles = new List<Tile>(16);            
        }

        public void CreateTile()
        {
            Tile tile = Instantiate(this.tilePrefab, this.grid.transform);            
            tile.SetState(this.tileStates[0], 2);
            tile.Spawn(this.grid.GetRandomEmptyCell());
            this.tiles.Add(tile);
        }

        public void ClearBoard() 
        {
            foreach (var cell in this.grid.cells)
                cell.tile = null;

            foreach (var tile in this.tiles)
                Destroy(tile.gameObject);

            this.tiles.Clear();
        }

        private void Move(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
        {
            bool changed = false;

            for (int x = startX; x >= 0 && x < this.grid.width; x += incrementX)
            {
                for (int y = startY; y >= 0 && y < this.grid.height; y += incrementY)
                {
                    TileCell cell = this.grid.GetCell(x, y);

                    if (cell.occupied) changed |= MoveTile(cell.tile, direction);
                }
            }

            if (changed) WaitingForChanges();
        }
        
        private bool MoveTile(Tile tile, Vector2Int direction)
        {
            TileCell newCell = null;
            TileCell adjacent = this.grid.GetAdjacentCell(tile.cell, direction);

            while (adjacent != null) 
            {
                if (adjacent.occupied)
                {
                    if (CanMerge(tile, adjacent.tile))
                    {
                        Merge(tile, adjacent.tile);
                        return true;
                    }

                    break;
                }

                newCell = adjacent;
                adjacent = this.grid.GetAdjacentCell(adjacent, direction);
            }

            if (newCell != null)
            {
                tile.MoveTo(newCell);
                return true;
            }

            return false;
        }

        private bool CanMerge(Tile tileA, Tile tileB) => tileA.number == tileB.number && !tileB.locked;

        private void Merge(Tile tileA, Tile tileB)
        {
            this.tiles.Remove(tileA);

            tileA.Merge(tileB.cell);

            int index = Mathf.Clamp(IndexOf(tileB.state) + 1, 0, this.tileStates.Length - 1);
            int number = tileB.number * 2;

            tileB.SetState(this.tileStates[index], number);

            this.gameManager.IncreaseScore(number);
        }

        private int IndexOf(TileState state)
        {
            for (int a = 0; a < this.tileStates.Length; a++)
                if (state == this.tileStates[a]) return a;

            return -1;
        }

        private async void WaitingForChanges()
        {
            this.waiting = true;

            await Task.Delay(TimeSpan.FromSeconds(0.1));

            this.waiting = false;

            foreach (var tile in this.tiles) tile.locked = false;

            if (this.tiles.Count != this.grid.size) CreateTile();

            if (CheckForGameOver()) this.gameManager.GameOver();
        }

        private bool CheckForGameOver()
        {
            if (this.tiles.Count != this.grid.size) return false;

            foreach (var tile in this.tiles)
            {
                TileCell up = this.grid.GetAdjacentCell(tile.cell, Vector2Int.up);
                TileCell down = this.grid.GetAdjacentCell(tile.cell, Vector2Int.down);
                TileCell left = this.grid.GetAdjacentCell(tile.cell, Vector2Int.left);
                TileCell right = this.grid.GetAdjacentCell(tile.cell, Vector2Int.right);

                if (up != null && CanMerge(tile, up.tile)) return false;
                if (down != null && CanMerge(tile, down.tile)) return false;
                if (left != null && CanMerge(tile, left.tile)) return false;
                if (right != null && CanMerge(tile, right.tile)) return false;
            }

            return true;
        }

        public void MoveUp()
        {
            if (this.waiting) return;

            Move(Vector2Int.up, 0, 1, 1, 1);
        }

        public void MoveDown()
        {
            if (this.waiting) return;

            Move(Vector2Int.down, 0, 1, this.grid.height - 2, -1);
        }

        public void MoveLeft()
        {
            if (this.waiting) return;

            Move(Vector2Int.left, 1, 1, 0, 1);
        }

        public void MoveRight()
        {
            if (this.waiting) return;

            Move(Vector2Int.right, this.grid.width - 2, -1, 0, 1);
        }
    }
}
