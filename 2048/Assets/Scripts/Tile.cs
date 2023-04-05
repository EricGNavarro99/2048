using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Tile.State;
using Unity.Tile.Cell;

namespace Unity.Tile
{
    public class Tile : MonoBehaviour
    {
        public TileState state { get; private set; }
        public TileCell cell { get; private set; }
        public int number { get; private set; }

        public bool locked { get; set; }

        private Image background;
        private TextMeshProUGUI text;

        private void Awake()
        {
            background = GetComponent<Image>();
            text = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void SetState(TileState state, int number)
        {
            this.state = state;
            this.number = number;

            background.color = state.backgroundColor;
            text.color = state.textColor;
            text.text = number.ToString();
        }

        public void Spawn (TileCell cell)
        {
            if (this.cell != null) this.cell.tile = null;

            this.cell = cell;
            this.cell.tile = this;

            transform.position = cell.transform.position;
        }

        public void MoveTo(TileCell cell) 
        {
            if (this.cell != null) this.cell.tile = null;

            this.cell = cell;
            this.cell.tile = this;

            Animate(cell.transform.position, false);
        }

        public void Merge(TileCell cell)
        {
            if (this.cell != null ) this.cell.tile = null;

            this.cell = null;

            cell.tile.locked = true;

            Animate(cell.transform.position, true);
        }

        private async void Animate(Vector3 to, bool merging)
        {
            float elapsed = 0f;
            float duration = 0.1f;

            Vector3 from = transform.position;

            while (elapsed < duration)
            {
                transform.position = Vector3.Lerp(from, to, elapsed / duration);
                elapsed += Time.deltaTime;
                await Task.Delay(TimeSpan.FromMilliseconds(1));
            }

            transform.position = to;

            if (merging) Destroy(gameObject);
        }
    }
}