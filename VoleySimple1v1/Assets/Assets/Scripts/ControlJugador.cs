using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls; // Agregamos esto para que entienda los controles de botones

public class ControlJugador : MonoBehaviour
{
    [Header("Configuracion de Movimiento")]
    public float velocidad = 10f;

    [Header("Configuracion de Teclas (Strings)")]
    public string teclaArriba = "w";
    public string teclaAbajo = "s";
    public string teclaIzquierda = "a";
    public string teclaDerecha = "d";
    public string teclaSaltar = "space";

    [Header("Mecanica de Salto y Slider")]
    public Slider sliderPotencia;
    public float fuerzaSaltoMaxima = 15f;
    public float velocidadBarra = 2f;

    private Rigidbody rb;
    private bool estaEnElSuelo = true;
    private float tiempoSlider = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (sliderPotencia != null)
        {
            sliderPotencia.minValue = 0f;
            sliderPotencia.maxValue = 1f;
        }
    }

    void Update()
    {
        // Movimiento del slider en vaivén
        if (estaEnElSuelo && sliderPotencia != null)
        {
            tiempoSlider += Time.deltaTime * velocidadBarra;
            sliderPotencia.value = Mathf.PingPong(tiempoSlider, 1f);
        }

        // Detectar el salto de forma segura
        if (Keyboard.current != null)
        {
            // Buscamos la tecla y la convertimos en un KeyControl válido
            KeyControl botonSalto = Keyboard.current[teclaSaltar] as KeyControl;
            if (botonSalto != null && botonSalto.wasPressedThisFrame && estaEnElSuelo)
            {
                Saltar();
            }
        }
    }

    void FixedUpdate()
    {
        if (Keyboard.current == null) return;

        Vector3 movimiento = Vector3.zero;

        // Buscamos cada tecla aclarándole a Unity que es un botón/tecla (KeyControl)
        KeyControl btnArriba = Keyboard.current[teclaArriba] as KeyControl;
        KeyControl btnAbajo = Keyboard.current[teclaAbajo] as KeyControl;
        KeyControl btnDerecha = Keyboard.current[teclaDerecha] as KeyControl;
        KeyControl btnIzquierda = Keyboard.current[teclaIzquierda] as KeyControl;

        if (btnArriba != null && btnArriba.isPressed) movimiento.z = 1f;
        if (btnAbajo != null && btnAbajo.isPressed) movimiento.z = -1f;
        if (btnDerecha != null && btnDerecha.isPressed) movimiento.x = 1f;
        if (btnIzquierda != null && btnIzquierda.isPressed) movimiento.x = -1f;

        Vector3 nuevaPos = transform.position + movimiento.normalized * velocidad * Time.fixedDeltaTime;
        rb.MovePosition(new Vector3(nuevaPos.x, rb.position.y, nuevaPos.z));
    }

    void Saltar()
    {
        float porcentajePotencia = 0.2f;

        if (sliderPotencia != null)
        {
            porcentajePotencia = sliderPotencia.value;
        }

        float fuerzaFinal = porcentajePotencia * fuerzaSaltoMaxima;
        rb.AddForce(Vector3.up * fuerzaFinal, ForceMode.Impulse);
        estaEnElSuelo = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        string nombreObjeto = collision.gameObject.name.ToLower();
        if (nombreObjeto.Contains("cancha") || nombreObjeto.Contains("floor") || nombreObjeto.Contains("cube") || nombreObjeto.Contains("player"))
        {
            estaEnElSuelo = true;
        }
    }
}