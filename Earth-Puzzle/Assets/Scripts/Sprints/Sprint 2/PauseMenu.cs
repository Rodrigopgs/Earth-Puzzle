using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// A simple pause menu class.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [Tooltip("The name of the scene to load as the main menu")]
    public string menuScene;

    [Tooltip("The UI element to enable when paused. Usually this would be an empty GameObject with children.")]
    public GameObject pauseMenu;

    bool paused;

    Inputs inputs;
    Inputs.MenuActions menuActions;
    InputAction escape;

    void Awake()
    {
        inputs = new Inputs();
        menuActions = inputs.Menu;
        escape = menuActions.Escape;
    }

    void OnEnable()
    {
        inputs.Enable();
        menuActions.Enable();
        escape.Enable();

        escape.performed += OnEscape;
    }

    private void OnEscape(InputAction.CallbackContext cb)
    {
        if (!paused)
        {
            paused = true;
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
        else
        {
            Resume();
        }
    }

    void Start()
    {
        pauseMenu.SetActive(false);
    }

    /// <summary>
    /// Reloads the current scene
    /// </summary>
    public void Restart()
    {
        paused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Reusmes the current scene
    /// </summary>
    public void Resume()
    {
        paused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    /// <summary>
    /// Loads the menu scene
    /// </summary>
    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuScene);
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void Quit() => Application.Quit();

    void OnDestroy()
    {
        escape.performed -= OnEscape;
    }
}