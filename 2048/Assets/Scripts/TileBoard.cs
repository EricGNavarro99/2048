using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Tile.State;
using Unity.Tile.Grid;
using Unity.Tile.Cell;

namespace Unity.Tile.Board
{    
    public class TileBoard : MonoBehaviour
    {
        public Tile tilePrefab;
        [Range(2, 14)] public byte tilesInBoard = 2;
        public TileState[] tileStates;

        private TileGrid grid;
        private List<Tile> tiles;

        private bool waiting;

        private void Awake()
        {
            grid = GetComponentInChildren<TileGrid>();
            tiles = new List<Tile>(16);            
        }

        private void Start()
        {
            for (byte a = 0; a < tilesInBoard; a++) CreateTile();
        }

        private void CreateTile()
        {
            Tile tile = Instantiate(tilePrefab, grid.transform);            
            tile.SetState(tileStates[0], 2);
            tile.Spawn(grid.GetRandomEmptyCell());
            tiles.Add(tile);
        }

        private void Move(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
        {
            bool changed = false;

            for (int x = startX; x >= 0 && x < grid.width; x += incrementX)
            {
                for (int y = startY; y >= 0 && y < grid.height; y += incrementY)
                {
                    TileCell cell = grid.GetCell(x, y);

                    if (cell.occupied) changed |= MoveTile(cell.tile, direction);
                }
            }

            if (changed) WaitingForChanges();
        }
        
        private bool MoveTile(Tile tile, Vector2Int direction)
        {
            TileCell newCell = null;
            TileCell adjacent = grid.GetAdjacentCell(tile.cell, direction);

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
                adjacent = grid.GetAdjacentCell(adjacent, direction);
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
            tiles.Remove(tileA);

            tileA.Merge(tileB.cell);

            int index = Mathf.Clamp(IndexOf(tileB.state) + 1, 0, tileStates.Length - 1);
            int number = tileB.number * 2;

            tileB.SetState(tileStates[index], number);
        }

        private int IndexOf(TileState state)
        {
            for (int a = 0; a < tileStates.Length; a++)
                if (state == tileStates[a]) return a;

            return -1;
        }

        private async void WaitingForChanges()
        {
            waiting = true;

            await Task.Delay(TimeSpan.FromSeconds(0.1));

            waiting = false;

            foreach (var tile in tiles) tile.locked = false;

            if (tiles.Count != grid.size) CreateTile();
        }

        public void MoveUp()
        {
            if (waiting) return;

            Move(Vector2Int.up, 0, 1, 1, 1);
        }

        public void MoveDown()
        {
            if (waiting) return;

            Move(Vector2Int.down, 0, 1, grid.height - 2, -1);
        }

        public void MoveLeft()
        {
            if (waiting) return;

            Move(Vector2Int.left, 1, 1, 0, 1);
        }

        public void MoveRight()
        {
            if (waiting) return;

            Move(Vector2Int.right, grid.width - 2, -1, 0, 1);
        }
    }
}
