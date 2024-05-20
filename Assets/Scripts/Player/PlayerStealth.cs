using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class PlayerStealth : MonoBehaviour
{
    public static PlayerStealth instance;

    private int startingStealth = 100;
    private int currentStealth = 100;
    public bool playerIsVisible = true;
    public bool bossIsActive = false;
    public bool inKillZone = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public int GetStealth()
    {
        return currentStealth;
    }

    public void SetStealth(int amt)
    {
        currentStealth = amt;
        UpdateLuaVariable();
    }
    public void SubtractStealth(int amt)
    {
        if (currentStealth - amt < 0)
        {
            currentStealth = -1;
        }
        else
        {
            currentStealth -= amt;
        }
        //currentStealth = 0;
        UpdateLuaVariable();
    }

    public void AddStealth(int amt)
    {
        if (currentStealth <= startingStealth - amt)
        {
            currentStealth += amt;
        }
        else
        {
            currentStealth = startingStealth;
        }
        UpdateLuaVariable();
    }

    private void UpdateLuaVariable()
    {
        DialogueLua.SetVariable("playerStealthScore", currentStealth);
        Debug.Log("Lua Stealth Score: " + DialogueLua.GetVariable("playerStealthScore").asString);
    }
}
