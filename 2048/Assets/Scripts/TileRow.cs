using UnityEngine;
using Unity.Tile.Cell;

namespace Unity.Tile.Row
{
    public class TileRow : MonoBehaviour
    {
        public TileCell[] cells { get; private set;}

        private void Awake() => this.cells = GetComponentsInChildren<TileCell>();
    }
}
