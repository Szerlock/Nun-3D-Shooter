using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject startMenu;

    private bool isPaused = false;


    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 0f;

    }

    // Update is called once per frame
    public void StartGame()
    {
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

    public bool IsGameStarted()
    {
        return isPaused;
    }
}

