using UnityEngine;
using UnityTileCell;

namespace UnityTileRow
{
    public class TileRow : MonoBehaviour
    {
        public TileCell[] cells { get; private set;}

        private void Awake()
        {
            cells = GetComponentsInChildren<TileCell>();
        }
    }
}
