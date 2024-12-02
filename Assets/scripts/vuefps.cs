using UnityEngine;

public class CameraFollowBehind : MonoBehaviour
{
    public Transform player;    // La capsule ou le joueur
    public Vector3 offset;      // D�calage de la cam�ra par rapport � la capsule

    void Start()
    {
        // Initialiser l'offset pour une vue derri�re la capsule
        offset = new Vector3(0, 2f, -5f);  // La cam�ra est positionn�e 5 unit�s derri�re et 2 unit�s plus haut que la capsule
    }

    void Update()
    {
        // Positionner la cam�ra derri�re la capsule
        transform.position = player.position + offset;

        // Faire en sorte que la cam�ra regarde toujours la capsule
        transform.LookAt(player);
    }
}
