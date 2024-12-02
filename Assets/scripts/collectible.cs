using UnityEngine;
using UnityEngine.UI;

public class Collectible : MonoBehaviour
{
    public enum ObjectType
    {
        Fruit,
        Wood
    }

    public ObjectType objectType; // Le type de l'objet (Fruit, Wood, etc.)
    public TextMesh collectMessage; // Le message de collecte � afficher en 3D
    public float collectRange = 3f; // La distance � laquelle le joueur peut collecter l'objet

    private bool canCollect = false; // Pour savoir si le joueur peut collecter l'objet

    private void Start()
    {
        // Initialement, ne pas afficher le message
        collectMessage.gameObject.SetActive(false);
    }

    private void Update()
    {
        // V�rifier la distance entre le joueur et l'objet collectable
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            // Si le joueur est � port�e de collecte
            if (distance <= collectRange)
            {
                collectMessage.gameObject.SetActive(true); // Afficher le message
                canCollect = true; // Le joueur peut collecter l'objet

                // V�rifier si le joueur appuie sur E pour collecter
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Ajouter l'objet � l'inventaire
                    GameManager.Instance.AddToInventory(objectType.ToString());

                    // D�sactiver l'objet collectable
                    collectMessage.gameObject.SetActive(false); // Masquer le message
                    Destroy(gameObject); // D�truire l'objet collectable apr�s qu'il soit collect�
                }
            }
            else
            {
                // Si le joueur est trop loin, cacher le message
                collectMessage.gameObject.SetActive(false);
                canCollect = false;
            }
        }
    }
}
