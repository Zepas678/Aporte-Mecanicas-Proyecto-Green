using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Menus")]

    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject menu;

    [Header("First option selected")]
    [SerializeField] GameObject settingsMenuOptions;

    [SerializeField] GameObject menuOptions;


    PlayerInput playerInput;

    InputAction a_cancel;

    void OnEnable()
    {
        a_cancel.started += closeSettings;
    }

    void OnDisable()
    {
        a_cancel.started -= closeSettings;
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        a_cancel = playerInput.actions["Cancel"];

        EventSystem.current.SetSelectedGameObject(menuOptions);
    }
    public void newGame()
    {
        SceneManager.LoadScene("Items_Test");
    }

    public void openSettings()
    {
        settingsMenu.SetActive(true);
        menu.SetActive(false);

        EventSystem.current.SetSelectedGameObject(settingsMenuOptions);
    }

    public void closeSettings(InputAction.CallbackContext context)
    {
        settingsMenu.SetActive(false);
        menu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(menuOptions);
    }

    public void onClickCloseSettings()
    {
        settingsMenu.SetActive(false);
        menu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(menuOptions);
    }
}
