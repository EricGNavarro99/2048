using UnityEngine;

namespace UnityTileState
{
    [CreateAssetMenu(menuName = "Tile state")]
    public class TileState : ScriptableObject
    {
        public Color backgroundColor;
        public Color textColor;
    }
}
