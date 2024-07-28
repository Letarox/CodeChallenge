using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError(typeof(GameManager).ToString() + " is NULL");

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    #endregion

    private ActionState _currentActionState;

    public ActionState CurrentActionState => _currentActionState;

    void Update()
    {
        //if the player presses ESC and the ActionState is None, it quits the application
        if (Input.GetKeyDown(KeyCode.Escape) && _currentActionState == ActionState.None)
        {
            Application.Quit();
        }
    }

    public void SetActionState(ActionState newState)
    {
        _currentActionState = newState;
    }
}

public enum ActionState
{
    None,
    Inventory
}

public enum EquipmentType
{
    Sword,
    Shield,
    Axe
}