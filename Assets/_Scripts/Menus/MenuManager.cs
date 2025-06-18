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
        Death,
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

    [Header("Death Menu")]
    public GameObject deathHolder;

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
                PlayerManager.instance.LockInputs(false, false);
                PauseGame(false);

                // Activate Stuff
                break;

            case MenuState.Pause:
                // Set game state
                PlayerManager.instance.LockInputs(true, true);
                PauseGame(true);

                // Activate Stuff
                pauseHolder.SetActive(true);
                activeMenu = pauseHolder;
                break;

            case MenuState.Controls:
                // Set game state
                PlayerManager.instance.LockInputs(true, true);
                PauseGame(true);

                // Activate Stuff
                controlsHolder.SetActive(true);
                activeMenu = controlsHolder;
                break;

            case MenuState.Audio:
                // Set game state
                PlayerManager.instance.LockInputs(true, true);
                PauseGame(true);

                // Activate Stuff
                audioHolder.SetActive(true);
                activeMenu = audioHolder;
                break;

            case MenuState.Inventory:
                // Set game state
                PlayerManager.instance.LockInputs(true, true);
                PauseGame(false);

                // Activate Stuff
                inventoryHolder.SetActive(true);
                activeMenu = inventoryHolder;
                inventoryUI.OpenInventory();
                break;

            case MenuState.Death:
                // Set game state
                PlayerManager.instance.LockInputs(true, true, true);
                PauseGame(false);

                // Activate Stuff 
                deathHolder.SetActive(true);
                activeMenu = deathHolder;
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
