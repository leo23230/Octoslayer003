using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    private float startingHealth;
    private float currentHealth;

    public Image healthBarImage;

    /// <summary>
    /// Set starting health 
    /// </summary>
    public void SetStartingHealth(int startingHealth)
    {
        this.startingHealth = startingHealth;
        currentHealth = startingHealth;
    }

    /// <summary>
    /// Get the starting health
    /// </summary>
    public float GetStartingHealth()
    {
        return startingHealth;
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public void SubtractHealth(float amt)
    {
        if(currentHealth - amt < 0)
        {
            currentHealth = -1;
        }
        else
        {
            StartCoroutine(barLerp(currentHealth, currentHealth - amt));
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
            currentHealth = newVal;

            UpdateHealthBar();

            // Yield here
            yield return null;
        }
    }


    public void AddHealth(float amt)
    {
        if(currentHealth <= startingHealth - amt)
        {
            currentHealth += amt;
        }
        else
        {
            currentHealth = startingHealth;
        }
        
        UpdateHealthBar();
    }

    public void UpdateHealthBar() 
    {
        if(healthBarImage != null) healthBarImage.fillAmount = (float)currentHealth / (float)startingHealth;
    }

}