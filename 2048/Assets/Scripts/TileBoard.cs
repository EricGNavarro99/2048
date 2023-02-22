using System.Collections.Generic;
using UnityEngine;
using UnityTile;
using UnityTileState;
using UnityTileGrid;

namespace UnityTileBoard
{    
    public class TileBoard : MonoBehaviour
    {
        public Tile tilePrefab;
        [Range(2, 14)] public byte tilesInBoard = 2;
        public TileState[] tileStates;

        private TileGrid grid;
        private List<Tile> tiles;

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
    }
}
