using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector3 movementInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // 1. Capturar la entrada del teclado (WASD o Flechas)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        movementInput = (transform.forward * vertical + transform.right * horizontal).normalized;

        
    }

    void FixedUpdate()
    {
        // 2. Aplicar el movimiento usando físicas para evitar tropiezos con colliders
       rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
    }
}
