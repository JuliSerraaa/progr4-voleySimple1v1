using UnityEngine;

public class ControlPelota : MonoBehaviour
{
    [Header("Configuración de Saque")]
    public float fuerzaSaqueHorizontal = 5f;
    public float fuerzaSaqueVertical = 2f;

    private Rigidbody rb;
    private Vector3 posicionInicial;
    private int ultimoEnTocar = 0;
    private bool procesandoPunto = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        posicionInicial = transform.position;
        SakarAleatorio();
    }

    // ESTA FUNCIÓN AHORA SÓLO APLICA EL IMPULSO FISICO
    public void LanzarSaque()
    {
        procesandoPunto = false; // Liberamos el candado para el nuevo punto
        rb.isKinematic = false;  // Habilitamos físicas para que caiga y rebote

        // Dirección aleatoria izquierda o derecha en X
        float direccionX = Random.Range(0, 2) == 0 ? -1f : 1f;
        float direccionZ = Random.Range(-0.5f, 0.5f);

        Vector3 fuerzaSaque = new Vector3(direccionX * fuerzaSaqueHorizontal, fuerzaSaqueVertical, direccionZ);
        rb.AddForce(fuerzaSaque, ForceMode.Impulse);
    }

    // ESTA FUNCIÓN RESETEA LA PELOTA ARRIBA AL INSTANTE
    public void SakarAleatorio()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true; // La congelamos arriba para que no se caiga sola durante la espera

        transform.position = posicionInicial; // TELETRANSPORTE INSTANTÁNEO
        ultimoEnTocar = 0;

        // Espera 1.5 segundos quieta arriba y recién ahí se ejecuta el saque físico
        Invoke("LanzarSaque", 1.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GestorPuntaje.instancia != null && GestorPuntaje.instancia.TextoContieneGanador()) return;
        if (procesandoPunto) return; // Candado de seguridad

        string nombreObjeto = collision.gameObject.name.ToLower();

        if (nombreObjeto.Contains("player1")) { ultimoEnTocar = 1; return; }
        if (nombreObjeto.Contains("player2")) { ultimoEnTocar = 2; return; }

        // DETECTAR PUNTO ADENTRO DE LA CANCHA
        if (nombreObjeto.Contains("cancha") || nombreObjeto.Contains("floor") || nombreObjeto.Contains("cube"))
        {
            procesandoPunto = true; // Activamos el candado al toque

            if (transform.position.x < 0)
            {
                if (GestorPuntaje.instancia != null) GestorPuntaje.instancia.SumarPuntoPlayer2();
            }
            else
            {
                if (GestorPuntaje.instancia != null) GestorPuntaje.instancia.SumarPuntoPlayer1();
            }

            // Desaparece del suelo INSTANTÁNEAMENTE y va arriba a esperar
            SakarAleatorio();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GestorPuntaje.instancia != null && GestorPuntaje.instancia.TextoContieneGanador()) return;
        if (procesandoPunto) return;

        // DETECTAR PUNTO AFUERA
        if (other.gameObject.name.ToLower().Contains("zonafuera"))
        {
            procesandoPunto = true; // Activamos el candado al toque

            if (ultimoEnTocar == 1)
            {
                if (GestorPuntaje.instancia != null) GestorPuntaje.instancia.SumarPuntoPlayer2();
            }
            else if (ultimoEnTocar == 2)
            {
                if (GestorPuntaje.instancia != null) GestorPuntaje.instancia.SumarPuntoPlayer1();
            }
            else
            {
                if (transform.position.x < 0)
                {
                    if (GestorPuntaje.instancia != null) GestorPuntaje.instancia.SumarPuntoPlayer2();
                }
                else
                {
                    if (GestorPuntaje.instancia != null) GestorPuntaje.instancia.SumarPuntoPlayer1();
                }
            }

            // Desaparece de la zona de afuera INSTANTÁNEAMENTE y va arriba a esperar
            SakarAleatorio();
        }
    }
}