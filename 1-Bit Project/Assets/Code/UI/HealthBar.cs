using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private float healthPercentage = 100f;
    public RectTransform healthBarFill; // Reference to the health bar fill RectTransform

    void Update()
    {
        UpdateHealthBar(); 
    }
    // Method to take damage

    // Method to update the health bar fill based on current health
    private void UpdateHealthBar()
    {
        // Calculate the health percentage
        if (TurretHealth.maxHealth == 0)
        {
            healthPercentage = 100f;
        }
        else
        {
            healthPercentage = Mathf.Clamp((float)TurretHealth.currentHealth / TurretHealth.maxHealth, 0f, 1f);
        }

        //Debug.Log($"Precentage: {healthPercentage}");

        // Adjust the size of the health bar fill RectTransform based on health percentage
        healthBarFill.localScale = new Vector3(healthPercentage, 1f, 1f);
    }
}
