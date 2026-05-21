using UnityEngine;

public class ControlJugador : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 10f;

    [Header("Configuración de Teclas")]
    public KeyCode teclaArriba;
    public KeyCode teclaAbajo;
    public KeyCode teclaIzquierda;
    public KeyCode teclaDerecha;

    private Rigidbody rb;

    void Start()
    {
        // Agarramos el Rigidbody del personaje para moverlo con físicas
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Creamos un vector de movimiento en base a qué tecla se presione
        Vector3 movimiento = Vector3.zero;

        if (Input.GetKey(teclaArriba))    movimiento.z = 1f;
        if (Input.GetKey(teclaAbajo))     movimiento.z = -1f;
        if (Input.GetKey(teclaDerecha))   movimiento.x = 1f;
        if (Input.GetKey(teclaIzquierda)) movimiento.x = -1f;

        // Movemos al jugador usando el Rigidbody para que no atraviese cosas
        rb.MovePosition(transform.position + movimiento.normalized * velocidad * Time.deltaTime);
    }
}