using UnityEngine;
using UnityTile;

namespace UnityTileCell
{
    public class TileCell : MonoBehaviour
    {
        public Vector2Int coordinates { get; set; }
        public Tile tile { get; set; }

        private bool empty => tile == null;
        public bool occupied => tile != null;
    }
}
