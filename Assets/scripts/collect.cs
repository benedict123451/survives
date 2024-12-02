using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ItemCollectingWithInput : MonoBehaviour
{
    // Dictionnaire pour stocker le nom de l'objet et sa quantité
    public Dictionary<string, int> collectedItems = new Dictionary<string, int>();
    public float collectDistance = 3f; // Distance à laquelle le joueur peut interagir avec l'objet
    public KeyCode collectKey = KeyCode.E;  // La touche pour collecter l'objet (par défaut E)
    public Text inventoryText;  // L'UI text pour afficher l'inventaire
    public GameObject promptUI;  // L'UI pour afficher le message d'invite

    private GameObject currentItem;  // L'objet collectable actuel à proximité

    void Start()
    {
        // Masquer le prompt au début
        promptUI.SetActive(false);
        // Réinitialiser l'affichage de l'inventaire
        UpdateInventoryDisplay();
    }

    void Update()
    {
        // Vérifier si le joueur est à proximité d'un objet collectable
        CheckForCollectable();

        // Si l'objet collectable est à proximité et que la touche est pressée
        if (currentItem != null && Input.GetKeyDown(collectKey))
        {
            CollectItem();
        }
    }

    // Vérifier les objets collectables proches
    void CheckForCollectable()
    {
        // Chercher tous les objets à proximité avec un Collider
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, collectDistance);

        bool foundItem = false;

        foreach (var hitCollider in hitColliders)
        {
            // Vérifier si l'objet a le tag "Collectible"
            if (hitCollider.CompareTag("Collectible"))
            {
                // Afficher le prompt à l'écran
                promptUI.SetActive(true);
                // Mettre à jour la position du prompt au-dessus de l'objet
                promptUI.transform.position = Camera.main.WorldToScreenPoint(hitCollider.transform.position + Vector3.up);

                // Enregistrer l'objet comme l'objet collectable actuel
                currentItem = hitCollider.gameObject;

                foundItem = true;
                break;
            }
        }

        // Si aucun objet n'est à proximité, cacher le prompt
        if (!foundItem)
        {
            promptUI.SetActive(false);
            currentItem = null;
        }
    }

    // Collecter l'objet et mettre à jour l'inventaire
    void CollectItem()
    {
        // Si l'objet est collecté
        if (currentItem != null)
        {
            string itemName = currentItem.name;  // Utilise le nom de l'objet comme identifiant

            // Si l'objet est déjà dans le dictionnaire, augmenter sa quantité
            if (collectedItems.ContainsKey(itemName))
            {
                collectedItems[itemName]++;
            }
            else
            {
                // Sinon, ajouter l'objet avec une quantité de 1
                collectedItems.Add(itemName, 1);
            }

            Debug.Log(itemName + " collected!");  // Afficher dans la console

            // Mettre à jour l'affichage de l'inventaire
            UpdateInventoryDisplay();

            // Désactiver l'objet collecté après l'ajout au dictionnaire
            currentItem.SetActive(false);

            // Réactiver l'objet après 10 secondes
            StartCoroutine(ReappearItem(currentItem, 10f));  // Utilisation d'une Coroutine pour réapparaître après 10 secondes

            currentItem = null;  // Réinitialiser currentItem après avoir collecté l'objet
        }
    }

    // Coroutine pour réactiver l'objet après un délai
    IEnumerator ReappearItem(GameObject item, float delay)
    {
        // Attendre le temps spécifié
        yield return new WaitForSeconds(delay);

        // Réactiver l'objet
        item.SetActive(true);

        // Optionnel : Vous pouvez remettre l'objet à sa position initiale ici si nécessaire
        // item.transform.position = originalPosition;
    }

    // Mettre à jour l'affichage de l'inventaire
    void UpdateInventoryDisplay()
    {
        // Si l'inventaire UI est assigné
        if (inventoryText != null)
        {
            // Réinitialiser le texte de l'inventaire
            string inventoryContent = "Inventaire: \n";  // Commencer avec l'en-tête

            // Ajouter chaque objet collecté avec sa quantité à l'affichage de l'inventaire
            foreach (KeyValuePair<string, int> item in collectedItems)
            {
                inventoryContent += item.Key + " x" + item.Value + "\n";  // Afficher l'objet et sa quantité
            }

            // Assigner le texte complet à l'UI
            inventoryText.text = inventoryContent;

            // Forcer la mise à jour de l'UI
            LayoutRebuilder.ForceRebuildLayoutImmediate(inventoryText.transform as RectTransform);
            Debug.Log("Inventory updated: " + inventoryText.text);  // Log pour vérifier l'affichage
        }
    }
}
