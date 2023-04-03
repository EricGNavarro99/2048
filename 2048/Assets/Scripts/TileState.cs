using UnityEngine;

namespace Unity.Tile.State
{
    [CreateAssetMenu(menuName = "Tile state")]
    public class TileState : ScriptableObject
    {
        public Color backgroundColor;
        public Color textColor;
    }
}
