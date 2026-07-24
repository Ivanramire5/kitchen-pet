using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 200f;

    [Header("References")]
    [SerializeField] private Transform playerBody; // Arrastra aquí al jugador desde el Inspector

    private float xRotation = 0f;

    void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 1. Capturar el movimiento del mouse (multiplicado por Time.deltaTime para que sea independiente de los FPS)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 2. Acumular y limitar la rotación vertical (eje X local) para no doblar el cuello de más
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // 3. Aplicar la rotación vertical solo a la cámara
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 4. Rotar el cuerpo del jugador horizontalmente (eje Y del mundo)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
