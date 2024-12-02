using UnityEngine;

public class PlayerMovementWithDynamicLimits : MonoBehaviour
{
    public float moveSpeed = 5f;   // Vitesse de déplacement
    public float rotationSpeed = 700f;  // Vitesse de rotation

    public Collider limitArea; // L'objet qui définit les limites (par exemple, un Plane ou un autre Collider)

    private Vector3 minBounds;
    private Vector3 maxBounds;

    void Start()
    {
        // Vérifier si limitArea a été assigné, sinon afficher un message d'erreur
        if (limitArea == null)
        {
            Debug.LogError("Aucun Collider pour définir les limites n'a été assigné !");
            return;
        }

        // Calculer les limites en fonction de l'objet limitArea (ici on utilise le collider)
        CalculateBounds();
    }

    void Update()
    {
        if (limitArea == null)
            return;  // Si limitArea n'est pas assigné, ne rien faire

        // Récupérer les entrées de l'utilisateur pour le mouvement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculer la direction du mouvement
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // Déplacer la capsule
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // Faire tourner la capsule pour qu'elle regarde la direction du mouvement
        if (moveDirection.magnitude > 0f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // Limiter le mouvement du personnage pour qu'il ne dépasse pas les limites
        RestrictMovement();
    }

    // Fonction pour calculer dynamiquement les limites en fonction du Collider de l'objet limitArea
    void CalculateBounds()
    {
        // Si limitArea est un BoxCollider, on peut directement utiliser sa taille et sa position
        if (limitArea is BoxCollider boxCollider)
        {
            minBounds = boxCollider.bounds.min;
            maxBounds = boxCollider.bounds.max;
        }
        // Si limitArea est un MeshCollider (par exemple pour un terrain), on fait un calcul similaire
        else if (limitArea is MeshCollider meshCollider)
        {
            minBounds = meshCollider.bounds.min;
            maxBounds = meshCollider.bounds.max;
        }
        else
        {
            Debug.LogError("Le collider n'est ni un BoxCollider ni un MeshCollider !");
        }
    }

    // Fonction pour limiter la position du personnage à l'intérieur des limites calculées
    void RestrictMovement()
    {
        // Récupérer la position actuelle du personnage
        Vector3 currentPosition = transform.position;

        // Limiter la position X et Z pour qu'elle soit dans les bornes spécifiées
        currentPosition.x = Mathf.Clamp(currentPosition.x, minBounds.x, maxBounds.x);
        currentPosition.z = Mathf.Clamp(currentPosition.z, minBounds.z, maxBounds.z);

        // Appliquer la nouvelle position (la position Y reste inchangée)
        transform.position = currentPosition;
    }
}
