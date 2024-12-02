using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Vitesse de déplacement
    public float rotationSpeed = 700f; // Vitesse de rotation du personnage

    private Rigidbody rb;

    void Start()
    {
        // On récupère le Rigidbody du personnage
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Récupérer les entrées de l'utilisateur (flèches directionnelles ou WASD)
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculer le mouvement du personnage
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);

        // Appliquer le mouvement au Rigidbody pour déplacer le personnage
        rb.MovePosition(transform.position + movement * speed * Time.deltaTime);

        // Rotation du personnage pour faire face à la direction du mouvement
        if (movement.magnitude > 0)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
