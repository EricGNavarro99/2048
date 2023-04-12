using System;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Tile.Board;
using TMPro;

namespace Unity.Manager
{
    public class GameManager : MonoBehaviour
    {
        public TileBoard board;
        [Range(2, 14)] public byte tilesInBoard = 2;

        public CanvasGroup gameOverCanvasGroup;

        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI highScoreText;

        private int score;
        private int highScore;

        private void Start() => NewGame();

        public void NewGame()
        {
            SetScore(0);
            this.highScore = LoadHighScore();
            this.highScoreText.text = LoadHighScore().ToString();

            this.gameOverCanvasGroup.alpha = 0f;
            this.gameOverCanvasGroup.interactable = false;

            this.board.ClearBoard();

            for (byte a = 0; a < this.tilesInBoard; a++)
                this.board.CreateTile();

            this.board.enabled = true;
        }

        public void GameOver()
        {
            this.board.enabled = false;

            this.gameOverCanvasGroup.interactable = true;

            Fade(this.gameOverCanvasGroup, 1f, 1f);
        }

        public void ExitGame() => Application.Quit();

        private void SetScore(int score)
        {
            this.score = score;
            this.scoreText.text = score.ToString();

            if (this.score > this.highScore)
                this.highScoreText.text = this.score.ToString();

            SaveHighScore();
        }

        public void IncreaseScore(int points) => SetScore(this.score + points);

        private void SaveHighScore()
        {
            int highScore = LoadHighScore();

            if (this.score >  highScore)
                PlayerPrefs.SetInt("HighScore", this.score);
        }

        private int LoadHighScore() => PlayerPrefs.GetInt("HighScore", 0);

        private async void Fade(CanvasGroup canvasGroup, float to, float delay)
        {
            await Task.Delay(TimeSpan.FromSeconds(delay));

            float elapsed = 0f, duration = 0.5f;
            float from = canvasGroup.alpha;

            while (elapsed < duration) 
            {
                canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
                elapsed += Time.deltaTime;

                await Task.Delay(TimeSpan.FromMilliseconds(10));
            }

            canvasGroup.alpha = to;
        }
    }
}
