using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
    public float damage = 10f;  // Dégâts infligés au joueur
    public float damageDelay = 2f;  // Délai avant que le monstre n'inflige des dégâts à nouveau
    private float nextDamageTime;  // Temps auquel le monstre peut infliger des dégâts
    private bool isTouchingPlayer;  // Si le monstre touche actuellement le joueur
    private GameObject player;  // Référence au joueur
    private PlayerHealth playerHealth;  // Référence au script PlayerHealth du joueur

    public float moveSpeed = 3f;  // Vitesse de déplacement du monstre
    private bool isFrozen;  // Si le monstre est gelé (immobile)

    void Start()
    {
        // Trouver le joueur dans la scène
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();  // Récupérer le script PlayerHealth du joueur
        }

        // S'assurer que le monstre est un trigger
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;  // Permet au monstre de traverser le joueur sans collision physique
        }
    }

    void Update()
    {
        if (player != null)
        {
            // Si le monstre n'est pas gelé, il peut se déplacer
            if (!isFrozen)
            {
                // Déplacer le monstre vers la position du joueur
                MoveTowardsPlayer();
            }

            // Si le monstre touche le joueur et que le délai est écoulé
            if (isTouchingPlayer && Time.time >= nextDamageTime && !isFrozen)
            {
                // Infliger des dégâts au joueur après le délai de 2 secondes
                playerHealth.TakeDamage(damage);
                nextDamageTime = Time.time + damageDelay;  // Réinitialiser le délai
                StartCoroutine(FreezeMonster());  // Geler le monstre pendant 2 secondes
            }
        }
    }

    void MoveTowardsPlayer()
    {
        // Calculer la direction vers le joueur
        Vector3 direction = (player.transform.position - transform.position).normalized;

        // Déplacer le monstre dans la direction du joueur
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Optionnel : Faire en sorte que le monstre regarde toujours le joueur
        transform.LookAt(player.transform);
    }

    void OnTriggerEnter(Collider other)
    {
        // Vérifier si le monstre touche le joueur
        if (other.CompareTag("Player"))
        {
            isTouchingPlayer = true;
            nextDamageTime = Time.time + damageDelay;  // Définir le temps de délai initial
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Si le monstre quitte la zone de contact avec le joueur, il arrête d'infliger des dégâts
        if (other.CompareTag("Player"))
        {
            isTouchingPlayer = false;
        }
    }

    // Coroutine pour geler le monstre pendant 2 secondes
    private IEnumerator FreezeMonster()
    {
        isFrozen = true;  // Geler le monstre
        yield return new WaitForSeconds(2f);  // Attendre 2 secondes
        isFrozen = false;  // Dé-geler le monstre
    }
}
