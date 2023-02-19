using UnityEngine;
using TMPro;


namespace Reborn
{
    public class StartText : MonoBehaviour
    {
        [SerializeField] private GameObject startUI;
        [SerializeField] private TMP_Text startText;

        float accTime = 0;
        float currentTime = 0;
        float previousTime = 0;
        private void Start()
        {
            currentTime = Time.realtimeSinceStartup;
        }
        private void Update()
        {
            if (accTime <= 3)
            {
                previousTime = currentTime;
                currentTime = Time.realtimeSinceStartup;
                accTime += currentTime - previousTime;
                startText.text = $"Start in {3 - (int)accTime}";
            }
            else
            {
                startUI.SetActive(false);
            }
        }
    }
}
