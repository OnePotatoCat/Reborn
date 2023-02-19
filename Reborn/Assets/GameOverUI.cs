using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace Reborn
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        public void UpdateFinalScore(int score)
        {
            text.text = $"Final Score: {score.ToString()}";
        }

        public void MainMenu()
        {
            SceneManager.LoadScene(0);
        }


    }
}