using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject startMenu;


    // Start is called before the first frame update
    void Start()
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

        // Unpause the game
        Time.timeScale = 1f;
    }
}
