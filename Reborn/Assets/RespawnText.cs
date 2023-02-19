using UnityEngine;
using TMPro;

namespace Reborn
{
    public class RespawnText : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameObject respawnUI;
        [SerializeField] private TMP_Text respawnText;

        public bool start = false;
        float accTime = 0;
        private void Update()
        {
            if (start)
            {
                
                accTime += Time.deltaTime;
                respawnText.text = $"Respawn in {(gameManager.respawnTimer - (int)accTime).ToString("0.0")}";
            }
            if (accTime >= gameManager.respawnTimer)
            {
                accTime = 0;
                start = false;
                respawnUI.SetActive(false);
            }
        }
    }
}
