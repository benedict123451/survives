using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public InventoryManager inventoryManager;  // Référence à l'inventaire
    public Transform craftingPanel;  // Panneau contenant les boutons de crafting
    public GameObject buttonPrefab;  // Préfab de bouton à instancier
    public GameObject recipeTextPrefab;  // Préfab de texte à instancier
    public float interactDistance = 3f;  // Distance d'interaction avec la table de crafting
    public KeyCode interactKey = KeyCode.E;  // Touche pour interagir avec la table de crafting

    public Dictionary<string, Dictionary<string, int>> craftingRecipes = new Dictionary<string, Dictionary<string, int>>();

    private GameObject craftingTable;  // Référence à la table de crafting
    private bool isNearCraftingTable = false;  // Vérifie si le joueur est proche de la table de crafting

    void Start()
    {
        // Exemple de recettes : fabriquer une épée avec 2 bois et 1 fer
        craftingRecipes.Add("épée fruité", new Dictionary<string, int>
        {
            { "Palmier", 1 },
            { "peach", 2 }
        });

        // Exemple : fabriquer une hache avec 3 bois
        craftingRecipes.Add("gants fruités", new Dictionary<string, int>
        {
            { "peach", 1 },
            {"Pomme", 1},
            {"Pastèque",1 }
        });


        // Masquer le panneau au début
        //craftingPanel.gameObject.SetActive(false);

        // Trouver la table de crafting (si elle existe)
        craftingTable = GameObject.FindWithTag("CraftingTable");  // Assurez-vous que votre table de crafting a le tag "CraftingTable"

        // Créer les boutons et textes dynamiquement
        CreateCraftingUI();
    }

    void Update()
    {
        // Vérifier la proximité avec la table de crafting
        if (craftingTable != null)
        {
            float distanceToTable = Vector3.Distance(transform.position, craftingTable.transform.position);
            isNearCraftingTable = distanceToTable <= interactDistance;
        }

        // Afficher ou cacher le panneau de crafting selon l'interaction
        if (isNearCraftingTable && Input.GetKeyDown(interactKey))
        {
            // Afficher le panneau si le joueur est proche de la table de crafting
            craftingPanel.gameObject.SetActive(true);
        }
        else if (!isNearCraftingTable)
        {
            // Cacher le panneau si le joueur s'éloigne de la table
            craftingPanel.gameObject.SetActive(false);
        }
    }

    // Créer les boutons de crafting pour chaque recette
    void CreateCraftingUI()
    {
        // Pour chaque recette
        foreach (var recipe in craftingRecipes)
        {
            string itemName = recipe.Key;
            Dictionary<string, int> ingredients = recipe.Value;

            // Créer un bouton pour cette recette
            GameObject newButton = Instantiate(buttonPrefab, craftingPanel);
            newButton.GetComponentInChildren<Text>().text = "Craft " + itemName;

            // Créer un texte indiquant les ingrédients nécessaires
            GameObject newRecipeText = Instantiate(recipeTextPrefab, craftingPanel);
            string ingredientsText = "Ingredients: ";
            foreach (var ingredient in ingredients)
            {
                ingredientsText += ingredient.Key + " x" + ingredient.Value + ", ";
            }
            newRecipeText.GetComponent<Text>().text = ingredientsText;

            // Ajouter un événement au bouton pour faire le crafting
            Button button = newButton.GetComponent<Button>();
            button.onClick.AddListener(() => Craft(itemName));
        }
    }

    // Vérifier si le joueur peut fabriquer un objet
    public bool CanCraft(string itemName)
    {
        if (craftingRecipes.ContainsKey(itemName))
        {
            foreach (var material in craftingRecipes[itemName])
            {
                if (!inventoryManager.inventory.ContainsKey(material.Key) || inventoryManager.inventory[material.Key] < material.Value)
                {
                    return false;  // Pas assez de matériaux
                }
            }
            return true;  // Tous les matériaux nécessaires sont présents
        }
        return false;
    }

    // Fabriquer un objet et soustraire les matériaux nécessaires
    public void Craft(string itemName)
    {
        if (CanCraft(itemName))
        {
            // Soustraire les matériaux
            foreach (var material in craftingRecipes[itemName])
            {
                inventoryManager.RemoveItem(material.Key, material.Value);
            }

            // Ajouter l'objet fabriqué à l'inventaire
            inventoryManager.AddItem(itemName, 1);
            Debug.Log(itemName + " crafted successfully!");
        }
        else
        {
            Debug.Log("Not enough materials to craft " + itemName);
        }
    }
}
