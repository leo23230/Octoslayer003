using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionHint : MonoBehaviour
{
    public static InteractionHint instance;
    private TextMeshProUGUI textComponent;
    private string buttonPressInstruction;
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
    private void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        textComponent.text = "";
    }
    private void Update()
    {
        var controllers = Input.GetJoystickNames();

        if (controllers.Length <= 0)
        {
            buttonPressInstruction = "Press E to ";
        }
        else
        {
            buttonPressInstruction = "Press B to ";
        }
    }
    public void DisplayHint(string _hint)
    {
        textComponent.text = buttonPressInstruction + _hint;
    }
    public void DisableHint()
    {
        textComponent.text = "";
    }
}
