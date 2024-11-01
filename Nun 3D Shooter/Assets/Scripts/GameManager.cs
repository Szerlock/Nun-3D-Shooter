using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    CharacterScriptableObject characterData;
    public static GameManager instance { get; private set; }
    [SerializeField]
    private GameObject startMenu;
    [SerializeField]
    private GameObject RestartMenu;
    [SerializeField]
    private Corpse corpse;

    private bool isPaused = false;  


    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 0f;
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        corpse.SpawnCorpse();
        // Hide the start menu
        if (startMenu != null)
        {
            startMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        StartCoroutine(UnpauseAfterDelay(2f));
    }

    // Coroutine that waits for the specified delay before unpausing the game
    private IEnumerator UnpauseAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);  // Use WaitForSecondsRealtime so it respects Time.timeScale = 0
        Time.timeScale = 1f;  // Unpause the game
        isPaused = true;
    }

    public void OpenRestartMenu()
    {
        // Show the restart menu
        if (RestartMenu != null)
        {
            Time.timeScale = 0f;
            RestartMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        StartCoroutine(UnpauseAfterDelay(2f));
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool IsGameStarted()
    {
        return isPaused;
    }
}

