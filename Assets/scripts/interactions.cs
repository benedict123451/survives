using UnityEngine;
using UnityEngine.UI;

public class CraftingInteraction : MonoBehaviour
{
    public Text interactionText;  // R�f�rence au texte d'interaction
    public GameObject craftingPanel;  // R�f�rence au panneau de crafting
    public float interactDistance = 3f;  // Distance d'interaction avec la table de crafting
    public KeyCode interactKey = KeyCode.E;  // Touche pour interagir avec la table de crafting
    public KeyCode closePanelKey = KeyCode.F;  // Touche pour fermer le panneau de crafting

    private GameObject player;  // R�f�rence au joueur
    private bool isNearCraftingTable = false;  // V�rifie si le joueur est proche de la table de crafting

    void Start()
    {
        // Masquer le texte d'interaction au d�but
        interactionText.gameObject.SetActive(false);

        // Trouver le joueur dans la sc�ne en cherchant son tag
        player = GameObject.FindGameObjectWithTag("Player");

        // Masquer le panel de crafting au d�but
        //craftingPanel.SetActive(false);
    }

    void Update()
    {
        // V�rifier la proximit� du joueur par rapport � la table de crafting
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            isNearCraftingTable = distanceToPlayer <= interactDistance;

            // Debug: Affiche la distance entre la table et le joueur
        }

        // Si le joueur est � proximit�, afficher le texte d'interaction
        if (isNearCraftingTable)
        {
            interactionText.gameObject.SetActive(true);
            interactionText.text = "Appuyer sur E pour interagir avec la table de craft";

            // Mettre � jour la position du texte pour qu'il soit au-dessus de la table
            interactionText.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2);

            // Si le joueur appuie sur E, afficher le panneau de crafting
            if (Input.GetKeyDown(interactKey))
            {
                Debug.Log("Appuyez sur E : Ouverture du panneau de crafting");
                craftingPanel.SetActive(true);
            }
        }
        else
        {
            // Si le joueur s'�loigne de la table, masquer le texte d'interaction
            interactionText.gameObject.SetActive(false);
        }

        // Si le joueur appuie sur F, fermer le panneau de crafting
        if (Input.GetKeyDown(closePanelKey))
        {
            Debug.Log("F appuy� : Fermeture du panneau de crafting");
            craftingPanel.SetActive(false);
        }
    }
}
