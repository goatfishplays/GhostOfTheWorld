using System;
using UnityEngine;

// ! Notes for Goatfish that nobody cares about
/*
 Think this is bad for modding if you do like this
 makes custom menus near impossible I think
 unless there is a way to add additional enum variants
 if building a new one later try to figure something else out for modding support
 probs like pass it something that inherrits a base menu class 
*/
public class MenuManager : MonoBehaviour
{
    public enum MenuState
    {
        None,
        Pause,
        Controls,
        Audio,
        Inventory,

    }

    public MenuState state { get; private set; }
    [SerializeField] private GameObject activeMenu = null;

    // [Header("Menu Holders")]
    [Header("Pause Menu")]
    public GameObject pauseHolder;
    [Header("Controls Menu")]
    public GameObject controlsHolder;
    [Header("Audio Menu")]
    public GameObject audioHolder;
    public AudioManager audioManager => AudioManager.instance;

    [Header("Inventory Menu")]
    public GameObject inventoryHolder;
    public ItemUIInventoryController inventoryUI;

    public static MenuManager instance;

    public void SetState(string stateName)
    {
        try
        {
            MenuState state = (MenuState)Enum.Parse(typeof(MenuState), stateName);
            SetState(state);
        }
        catch (ArgumentException)
        {
            Debug.LogError($"'{stateName}' could not be parsed as an Enum for MenuState");
        }

    }

    public void SetState(MenuState newState = MenuState.None)
    {
        // Reset State 
        if (activeMenu != null)
        {
            activeMenu.SetActive(false);
        }
        activeMenu = null;



        // Load New State
        switch (newState)
        {
            case MenuState.None:
                // Set game state
                PlayerManager.instance.SetLookState(true);
                PauseGame(false);

                // Activate Stuff
                break;

            case MenuState.Pause:
                // Set game state
                PlayerManager.instance.SetLookState(false);
                pauseHolder.SetActive(true);

                // Activate Stuff
                activeMenu = pauseHolder;
                PauseGame(true);
                break;

            case MenuState.Controls:
                // Set game state
                PlayerManager.instance.SetLookState(false);
                controlsHolder.SetActive(true);

                // Activate Stuff
                activeMenu = controlsHolder;
                PauseGame(true);
                break;

            case MenuState.Audio:
                // Set game state
                PlayerManager.instance.SetLookState(false);
                audioHolder.SetActive(true);

                // Activate Stuff
                activeMenu = audioHolder;
                PauseGame(true);
                break;

            case MenuState.Inventory:
                // Set game state
                PlayerManager.instance.SetLookState(false);
                PauseGame(false);

                // Activate Stuff
                inventoryHolder.SetActive(true);
                activeMenu = inventoryHolder;
                inventoryUI.OpenInventory();
                break;
        }
        state = newState;
    }

    // TODO: Would be better if we had a GameStateManager that knows if the game is paused or not to prevent double pausing and reissuing way too many coroutines 
    public void PauseGame(bool pause = true)
    {
        if (pause)
        {
            Time.timeScale = 0f;
            audioManager.PauseAllSFX();
        }
        else
        {
            Time.timeScale = 1f;
            audioManager.UnPauseAllSFX();
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Two MenuManagers detected, deleting second");
            Destroy(this);
        }
    }

}
