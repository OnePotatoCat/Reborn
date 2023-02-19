using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Reborn
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private TMP_Text scoreText;

        public int score = 0;


        // Start is called before the first frame update
        void Start()
        {

        }

        public void AddScore(int point)
        {
            score += point;
            UpdateScoreText();
        }

        public bool SpendPoint(int point)
        {
            if (score < point)
            {
                return false;
            }

            score -= point;
            UpdateScoreText();
            return true;
        }

        private void UpdateScoreText()
        {
            scoreText.text = $"Score:  {score}";
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}