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
        if (collision.gameObject.name.ToLower().Contains("cancha") || collision.gameObject.name.ToLower().Contains("floor") || collision.gameObject.name.ToLower().Contains("cube"))
        {
            // IMPORTANTE: Asegurate de saber si tu Player 1 está en el lado izquierdo (X negativo) o derecho (X positivo)
            // En este ejemplo: Lado Izquierdo = Player 1, Lado Derecho = Player 2
            if (transform.position.x < 0)
            {
                // Si pica del lado izquierdo, el punto es para el rival (Player 2)
                if (GestorPuntaje.instancia != null) GestorPuntaje.instancia.SumarPuntoPlayer2();
            }
            else
            {
                // Si pica del lado derecho, el punto es para el Player 1
                if (GestorPuntaje.instancia != null) GestorPuntaje.instancia.SumarPuntoPlayer1();
            }

            Invoke("SakarAleatorio", 1.5f);
        }
    }
}