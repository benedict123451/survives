using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ItemCollectingWithInput : MonoBehaviour
{
    // Dictionnaire pour stocker le nom de l'objet et sa quantit�
    public Dictionary<string, int> collectedItems = new Dictionary<string, int>();
    public float collectDistance = 3f; // Distance � laquelle le joueur peut interagir avec l'objet
    public KeyCode collectKey = KeyCode.E;  // La touche pour collecter l'objet (par d�faut E)
    public Text inventoryText;  // L'UI text pour afficher l'inventaire
    public GameObject promptUI;  // L'UI pour afficher le message d'invite

    private GameObject currentItem;  // L'objet collectable actuel � proximit�

    void Start()
    {
        // Masquer le prompt au d�but
        promptUI.SetActive(false);
        // R�initialiser l'affichage de l'inventaire
        UpdateInventoryDisplay();
    }

    void Update()
    {
        // V�rifier si le joueur est � proximit� d'un objet collectable
        CheckForCollectable();

        // Si l'objet collectable est � proximit� et que la touche est press�e
        if (currentItem != null && Input.GetKeyDown(collectKey))
        {
            CollectItem();
        }
    }

    // V�rifier les objets collectables proches
    void CheckForCollectable()
    {
        // Chercher tous les objets � proximit� avec un Collider
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, collectDistance);

        bool foundItem = false;

        foreach (var hitCollider in hitColliders)
        {
            // V�rifier si l'objet a le tag "Collectible"
            if (hitCollider.CompareTag("Collectible"))
            {
                // Afficher le prompt � l'�cran
                promptUI.SetActive(true);
                // Mettre � jour la position du prompt au-dessus de l'objet
                promptUI.transform.position = Camera.main.WorldToScreenPoint(hitCollider.transform.position + Vector3.up);

                // Enregistrer l'objet comme l'objet collectable actuel
                currentItem = hitCollider.gameObject;

                foundItem = true;
                break;
            }
        }

        // Si aucun objet n'est � proximit�, cacher le prompt
        if (!foundItem)
        {
            promptUI.SetActive(false);
            currentItem = null;
        }
    }

    // Collecter l'objet et mettre � jour l'inventaire
    void CollectItem()
    {
        // Si l'objet est collect�
        if (currentItem != null)
        {
            string itemName = currentItem.name;  // Utilise le nom de l'objet comme identifiant

            // Si l'objet est d�j� dans le dictionnaire, augmenter sa quantit�
            if (collectedItems.ContainsKey(itemName))
            {
                collectedItems[itemName]++;
            }
            else
            {
                // Sinon, ajouter l'objet avec une quantit� de 1
                collectedItems.Add(itemName, 1);
            }

            Debug.Log(itemName + " collected!");  // Afficher dans la console

            // Mettre � jour l'affichage de l'inventaire
            UpdateInventoryDisplay();

            // D�sactiver l'objet collect� apr�s l'ajout au dictionnaire
            currentItem.SetActive(false);

            // R�activer l'objet apr�s 10 secondes
            StartCoroutine(ReappearItem(currentItem, 10f));  // Utilisation d'une Coroutine pour r�appara�tre apr�s 10 secondes

            currentItem = null;  // R�initialiser currentItem apr�s avoir collect� l'objet
        }
    }

    // Coroutine pour r�activer l'objet apr�s un d�lai
    IEnumerator ReappearItem(GameObject item, float delay)
    {
        // Attendre le temps sp�cifi�
        yield return new WaitForSeconds(delay);

        // R�activer l'objet
        item.SetActive(true);

        // Optionnel : Vous pouvez remettre l'objet � sa position initiale ici si n�cessaire
        // item.transform.position = originalPosition;
    }

    // Mettre � jour l'affichage de l'inventaire
    void UpdateInventoryDisplay()
    {
        // Si l'inventaire UI est assign�
        if (inventoryText != null)
        {
            // R�initialiser le texte de l'inventaire
            string inventoryContent = "Inventaire: \n";  // Commencer avec l'en-t�te

            // Ajouter chaque objet collect� avec sa quantit� � l'affichage de l'inventaire
            foreach (KeyValuePair<string, int> item in collectedItems)
            {
                inventoryContent += item.Key + " x" + item.Value + "\n";  // Afficher l'objet et sa quantit�
            }

            // Assigner le texte complet � l'UI
            inventoryText.text = inventoryContent;

            // Forcer la mise � jour de l'UI
            LayoutRebuilder.ForceRebuildLayoutImmediate(inventoryText.transform as RectTransform);
            Debug.Log("Inventory updated: " + inventoryText.text);  // Log pour v�rifier l'affichage
        }
    }
}
