using UnityEngine;

namespace Unity.Tile.Cell
{
    public class TileCell : MonoBehaviour
    {
        public Vector2Int coordinates { get; set; }
        public Tile tile { get; set; }

        private bool empty => this.tile == null;
        public bool occupied => this.tile != null;
    }
}
