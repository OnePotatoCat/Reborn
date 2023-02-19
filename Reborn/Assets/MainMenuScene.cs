using UnityEngine;
using UnityEngine.SceneManagement;

namespace Reborn
{
    public class MainMenuScene : MonoBehaviour
    {
        [SerializeField] MapGenerator mapGen;
        [SerializeField] GameObject GridSystem; 
        [SerializeField] Camera camera;
        [SerializeField] GameObject introMenu;

        float accTime = 0;
        Vector3 newCamPos;
        public float rotTime;
        public float speed = 1f;
        int i = 0;


        // Start is called before the first frame update
        void Start()
        {
            mapGen.GenerateMap();
            newCamPos = camera.transform.position;
        }

        private void Update()
        {
            accTime += Time.deltaTime;
            if (accTime >= 8)
            {
                int x = Random.Range(-5, 6);
                int z= Random.Range(-5, 6);
                newCamPos = new Vector3(camera.transform.position.x + x,
                                                camera.transform.position.y, 
                                                camera.transform.position.z + z);
                foreach (Transform child in GridSystem.transform)
                {
                    Destroy(child.gameObject);
                }
                mapGen.GenerateMap();
                i++;
                accTime = 0;
            }
            camera.transform.position = Vector3.Lerp(camera.transform.position, newCamPos, accTime/8f);
            camera.transform.rotation = Quaternion.Slerp(Quaternion.Euler(90, 0 , 45*i), Quaternion.Euler(90, 0, 45*(1+i)), accTime / 8f);
        }


        public void Instruction()
        {
            if (!introMenu.gameObject.activeSelf)
            {
                introMenu.gameObject.SetActive(true);
            }
            else
            {
                introMenu.gameObject.SetActive(false);
            }
            
        }

        public void LoadGameScene()
        {
            SceneManager.LoadScene(1);
        }

    }

}