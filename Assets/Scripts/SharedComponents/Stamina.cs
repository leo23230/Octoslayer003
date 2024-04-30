using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    private float startingStamina;
    private float currentStamina;
    [Tooltip("Rate in 10ths of a second")]
    public float staminaRefillRate = 5f;

    public Image staminaBarImage;
    private void Start()
    {
        StartCoroutine(PassiveStaminaIncrease());
    }
    public void SetStartingStamina(int startingStamina)
    {
        this.startingStamina = startingStamina;
        currentStamina = startingStamina;
    }

    public float GetStartingStamina()
    {
        return startingStamina;
    }

    public float GetStamina()
    {
        return currentStamina;
    }

    public void SubtractStamina(float amt)
    {
        if (currentStamina - amt < 0)
        {
            currentStamina = -1;
        }
        else
        {
            StartCoroutine(barLerp(currentStamina, currentStamina -= amt));
        }
        
    }

    private IEnumerator barLerp(float _currentStamina, float _newStamina)
    {
        float elapsedTime = 0;
        float waitTime = 0.1f;
        while (elapsedTime < waitTime)
        {
            float newVal = Mathf.Lerp(_currentStamina, _newStamina, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            currentStamina = newVal;

            UpdateStaminaBar();
            Debug.Log("Stamina: " + currentStamina);

            // Yield here
            yield return null;
        }
    }

    public void AddStamina(float amt)
    {
        if (currentStamina <= startingStamina - amt)
        {
            currentStamina += amt;
        }
        else
        {
            currentStamina = startingStamina;
        }

        UpdateStaminaBar();
    }

    public IEnumerator PassiveStaminaIncrease()
    {
        while(true)
        {
            if(currentStamina < startingStamina)
            {
                currentStamina += 1;
                UpdateStaminaBar();
                yield return new WaitForSeconds(staminaRefillRate / 10);
            }
            yield return null;
        }
    }

    public void UpdateStaminaBar()
    {
        if (staminaBarImage != null) staminaBarImage.fillAmount = (float)currentStamina / (float)startingStamina;
    }
}
