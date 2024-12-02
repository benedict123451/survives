using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<string, int> inventory = new Dictionary<string, int>();  // Dictionnaire de l'inventaire

    public Text inventoryText;  // Pour afficher l'inventaire � l'�cran

    // Ajouter un �l�ment � l'inventaire
    public void AddItem(string itemName, int amount)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName] += amount;  // Ajouter � l'existant
        }
        else
        {
            inventory.Add(itemName, amount);  // Ajouter le nouvel �l�ment
        }

        UpdateInventoryDisplay();
    }

    // Retirer un �l�ment de l'inventaire
    public void RemoveItem(string itemName, int amount)
    {
        if (inventory.ContainsKey(itemName) && inventory[itemName] >= amount)
        {
            inventory[itemName] -= amount;  // Retirer l'�l�ment
            if (inventory[itemName] == 0)
            {
                inventory.Remove(itemName);  // Supprimer l'�l�ment si la quantit� est 0
            }
            UpdateInventoryDisplay();
        }
    }

    // Mettre � jour l'affichage de l'inventaire
    void UpdateInventoryDisplay()
    {
        string inventoryContent = "Inventaire: \n";
        foreach (var item in inventory)
        {
            inventoryContent += item.Key + " x" + item.Value + "\n";
        }
        inventoryText.text = inventoryContent;
    }
}
