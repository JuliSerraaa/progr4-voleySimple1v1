using UnityEngine;

public class ControlPelota : MonoBehaviour
{
    [Header("Configuración de Saque")]
    public float fuerzaSaqueHorizontal = 5f;
    public float fuerzaSaqueVertical = 2f;

    private Rigidbody rb;
    private Vector3 posicionInicial;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        posicionInicial = transform.position; // Guardamos dónde empieza arriba de la red
        
        SakarAleatorio();
    }

    public void SakarAleatorio()
    {
        // Reseteamos velocidad por si venía moviéndose
        rb.linearVelocity = Vector3.zero; 
        rb.angularVelocity = Vector3.zero;
        transform.position = posicionInicial;

        // Elegimos una dirección al azar en el eje X (izquierda o derecha)
        // Si sale 0 va para un lado, si sale 1 va para el otro
        float direccionX = Random.Range(0, 2) == 0 ? -1f : 1f;

        // También le damos un toque aleatorio al eje Z para que no vaya siempre recto
        float direccionZ = Random.Range(-0.5f, 0.5f);

        // Creamos el vector de fuerza de saque
        Vector3 fuerzaSaque = new Vector3(direccionX * fuerzaSaqueHorizontal, fuerzaSaqueVertical, direccionZ);

        // Impulsamos la pelota
        rb.AddForce(fuerzaSaque, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Detectamos si tocó el suelo
        if (collision.gameObject.name.ToLower().Contains("cancha") || collision.gameObject.name.ToLower().Contains("floor") || collision.gameObject.name.ToLower().Contains("cube"))
        {
            // Guíate por la posición X de la pelota para saber de qué lado cayó
            // Asumiendo que la red está en la posición X = 0
            if (transform.position.x < 0)
            {
                // Cayó del lado izquierdo -> ¡Punto para el Jugador 2!
                Debug.Log("¡Punto para el Player 2!");
                // Aquí llamaremos al script del puntaje más adelante
            }
            else
            {
                // Cayó del lado derecho -> ¡Punto para el Jugador 1!
                Debug.Log("¡Punto para el Player 1!");
                // Aquí llamaremos al script del puntaje más adelante
            }

            // Re-sacamos la pelota después de un punto para seguir jugando
            Invoke("SakarAleatorio", 1.5f); // Espera 1.5 segundos antes de sacar otra vez
        }
    }
}