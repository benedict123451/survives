using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;  // Vie maximale du joueur
    private float currentHealth;  // Vie actuelle du joueur
    public Slider healthSlider;  // Slider de la barre de vie
    public Text healthText;  // Texte pour afficher la vie actuelle du joueur

    void Start()
    {
        currentHealth = maxHealth;  // Initialiser la vie actuelle à la vie maximale
        UpdateHealthUI();  // Mettre à jour l'UI dès le début
    }

    void UpdateHealthUI()
    {
        // Mettre à jour la valeur du slider
        healthSlider.value = currentHealth / maxHealth;

        // Afficher la vie actuelle sous forme de texte
        healthText.text = "HP: " + Mathf.RoundToInt(currentHealth) + "/" + Mathf.RoundToInt(maxHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        UpdateHealthUI();
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateHealthUI();
    }
}
