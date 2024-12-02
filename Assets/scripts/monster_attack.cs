using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
    public float damage = 10f;  // D�g�ts inflig�s au joueur
    public float damageDelay = 2f;  // D�lai avant que le monstre n'inflige des d�g�ts � nouveau
    private float nextDamageTime;  // Temps auquel le monstre peut infliger des d�g�ts
    private bool isTouchingPlayer;  // Si le monstre touche actuellement le joueur
    private GameObject player;  // R�f�rence au joueur
    private PlayerHealth playerHealth;  // R�f�rence au script PlayerHealth du joueur

    public float moveSpeed = 3f;  // Vitesse de d�placement du monstre
    private bool isFrozen;  // Si le monstre est gel� (immobile)

    void Start()
    {
        // Trouver le joueur dans la sc�ne
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();  // R�cup�rer le script PlayerHealth du joueur
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
            // Si le monstre n'est pas gel�, il peut se d�placer
            if (!isFrozen)
            {
                // D�placer le monstre vers la position du joueur
                MoveTowardsPlayer();
            }

            // Si le monstre touche le joueur et que le d�lai est �coul�
            if (isTouchingPlayer && Time.time >= nextDamageTime && !isFrozen)
            {
                // Infliger des d�g�ts au joueur apr�s le d�lai de 2 secondes
                playerHealth.TakeDamage(damage);
                nextDamageTime = Time.time + damageDelay;  // R�initialiser le d�lai
                StartCoroutine(FreezeMonster());  // Geler le monstre pendant 2 secondes
            }
        }
    }

    void MoveTowardsPlayer()
    {
        // Calculer la direction vers le joueur
        Vector3 direction = (player.transform.position - transform.position).normalized;

        // D�placer le monstre dans la direction du joueur
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Optionnel : Faire en sorte que le monstre regarde toujours le joueur
        transform.LookAt(player.transform);
    }

    void OnTriggerEnter(Collider other)
    {
        // V�rifier si le monstre touche le joueur
        if (other.CompareTag("Player"))
        {
            isTouchingPlayer = true;
            nextDamageTime = Time.time + damageDelay;  // D�finir le temps de d�lai initial
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Si le monstre quitte la zone de contact avec le joueur, il arr�te d'infliger des d�g�ts
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
        isFrozen = false;  // D�-geler le monstre
    }
}
