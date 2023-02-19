using UnityEngine;


namespace Reborn
{
    public class Atlas : MonoBehaviour
    {
        [SerializeField] public GameManager gameManager;
        [SerializeField] public Transform SpawnLocation;
        [SerializeField] public HealthBar healthBar;
        [SerializeField] public Camera SecondaryCamera;
        public GameObject player;
        public float health = 30;

        // Sound Effects
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip destructionSFX;
        [SerializeField] private float sfxVolume = 1f;

        public void Respawn()
        {
            player.transform.position = SpawnLocation.position;
            player.SetActive(true);
        }

        public void TakeDamage(float damage)
        {
            health -= damage;
            healthBar.SetHealth((int)health);
            if (health <= 0)
            {
                Debug.Log("Game Over");
                gameManager.GameOver();
            }
        }

        public void PlayDestructionClip()
        {
            audioSource.PlayOneShot(destructionSFX, sfxVolume);
        }

    }
}
