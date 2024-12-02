using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton pour acc�der facilement � ce script
    public Text inventoryText; // Affichage de l'inventaire dans l'UI

    private Dictionary<string, int> inventory = new Dictionary<string, int>(); // Dictionnaire pour stocker les objets et leur quantit�

    void Awake()
    {
        // Assurer qu'il n'y a qu'une seule instance de GameManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Ajouter un �l�ment � l'inventaire
    public void AddToInventory(string itemType)
    {
        if (inventory.ContainsKey(itemType))
        {
            inventory[itemType]++;
        }
        else
        {
            inventory.Add(itemType, 1);
        }

        // Mettre � jour l'UI de l'inventaire
        UpdateInventoryUI();
    }

    // Mettre � jour l'UI de l'inventaire
    private void UpdateInventoryUI()
    {
        inventoryText.text = ""; // Vider l'affichage actuel de l'inventaire

        // Parcourir tous les �l�ments de l'inventaire et les afficher
        foreach (KeyValuePair<string, int> item in inventory)
        {
            inventoryText.text += $"{item.Key}: {item.Value}\n";
        }
    }
}
