using UnityEngine;

public class PlayerController3D : MonoBehaviour
{
    public float moveSpeed = 5f;    // Velocidad de movimiento del jugador
    public float jumpForce = 10f;   // Fuerza de salto del jugador
    private Rigidbody rb;
    private bool isGrounded;
    private Vector3 originalGravity;
    private bool isPoweredUp = false;
    private float powerUpDuration = 4f;
    private float powerUpTimer = 0f;
    private Vector3 respawnPosition = new Vector3(0f, 3.45f, 0f); // Posición de reaparición predeterminada

    public GameObject spherePrefab; // Prefab de la esfera que quieres instanciar

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalGravity = Physics.gravity;

        // Invocar la función para instanciar esferas después de 2 segundos
        Invoke("SpawnSpheres", 2f);
    }

    void Update()
    {
        // Movimiento horizontal
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontalInput * moveSpeed * Time.deltaTime, 0f, 0f);

        // Interpolar suavemente entre la posición actual y la nueva posición deseada
        rb.MovePosition(Vector3.Lerp(transform.position, transform.position + movement, 0.5f));

        // Verificar la altura del jugador
        if (transform.position.y < -20f)
        {
            RespawnPlayer(); // Reaparecer si el jugador cae por debajo de cierta altura
        }

        // Verificar el temporizador del Power Up
        if (isPoweredUp)
        {
            powerUpTimer -= Time.deltaTime;
            if (powerUpTimer <= 0)
            {
                // Restablecer el tamaño del jugador cuando el Power Up expire
                transform.localScale /= 3.05f;
                isPoweredUp = false;
            }
        }

        // Saltar
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        // Modificar la gravedad cuando el jugador no esté en el suelo
        if (!isGrounded)
        {
            Physics.gravity = originalGravity * 2f; // Duplicar la gravedad
        }
        else
        {
            Physics.gravity = originalGravity; // Restaurar la gravedad original
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Verificar si el jugador está en el suelo
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        // Verificar si el jugador colisiona con el Power Up
        if (collision.gameObject.CompareTag("PowerUp"))
        {
            // Incrementar el tamaño del jugador
            transform.localScale *= 3.5f;
            isPoweredUp = true;
            powerUpTimer = powerUpDuration;

            // Desactivar el Power Up (destruirlo)
            Destroy(collision.gameObject);
        }
    }

    void RespawnPlayer()
    {
        // Reaparecer al jugador en la posición predeterminada
        transform.position = respawnPosition;
        rb.velocity = Vector3.zero; // Reiniciar la velocidad del jugador
    }

    void SpawnSpheres()
    {
        // Definir las posiciones donde quieres instanciar las esferas
        Vector3 spawnPosition = new Vector3(-10.04f, 4f, 0f);
        Vector3 spawnPosition2 = new Vector3(8.04f, 4f, 0f);
        Vector3 spawnPosition3 = new Vector3(24.13f, 1.74f, 0f);

        // Instanciar las esferas en las posiciones definidas
        Instantiate(spherePrefab, spawnPosition, Quaternion.identity);
        Instantiate(spherePrefab, spawnPosition2, Quaternion.identity);
        Instantiate(spherePrefab, spawnPosition3, Quaternion.identity);
    }
}
