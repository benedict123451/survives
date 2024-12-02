using UnityEngine;

public class CameraFollowBehind : MonoBehaviour
{
    public Transform player;    // La capsule ou le joueur
    public Vector3 offset;      // Décalage de la caméra par rapport à la capsule

    void Start()
    {
        // Initialiser l'offset pour une vue derrière la capsule
        offset = new Vector3(0, 2f, -5f);  // La caméra est positionnée 5 unités derrière et 2 unités plus haut que la capsule
    }

    void Update()
    {
        // Positionner la caméra derrière la capsule
        transform.position = player.position + offset;

        // Faire en sorte que la caméra regarde toujours la capsule
        transform.LookAt(player);
    }
}
