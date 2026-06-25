using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

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
    public string teclaPegar = "f";

    [Header("Mecanica de Salto y Slider")]
    public Slider sliderPotencia;
    public float fuerzaSaltoMaxima = 15f;
    public float velocidadBarra = 2f;

    [Header("Mecanica de Golpe Táctico")]
    public Transform canchaRival;
    public float distanciaParaPegar = 2.5f;
    public float fuerzaGolpeHorizontal = 12f;
    public float fuerzaGolpeVertical = 5f;
    public float largoCancha = 8f;

    private Rigidbody rb;
    private bool estaEnElSuelo = true;
    private float tiempoSlider = 0f;
    private GameObject pelota;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pelota = GameObject.Find("Pelota");

        if (sliderPotencia != null)
        {
            sliderPotencia.minValue = 0f;
            sliderPotencia.maxValue = 1f;
        }
    }

    void Update()
    {
       
        if (estaEnElSuelo && sliderPotencia != null)
        {
            tiempoSlider += Time.deltaTime * velocidadBarra;
            sliderPotencia.value = Mathf.PingPong(tiempoSlider, 1f);
        }

        if (Keyboard.current != null)
        {
            
            KeyControl botonSalto = Keyboard.current[teclaSaltar] as KeyControl;
            if (botonSalto != null && botonSalto.wasPressedThisFrame && estaEnElSuelo)
            {
                Saltar();
            }

            
            KeyControl botonPegar = Keyboard.current[teclaPegar] as KeyControl;
            if (botonPegar != null && botonPegar.wasPressedThisFrame)
            {
                PegarALaPelota();
            }
        }
    }

    void FixedUpdate()
    {
        if (Keyboard.current == null) return;

        Vector3 movimiento = Vector3.zero;

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
        if (sliderPotencia != null) porcentajePotencia = sliderPotencia.value;

        float fuerzaFinal = porcentajePotencia * fuerzaSaltoMaxima;
        rb.AddForce(Vector3.up * fuerzaFinal, ForceMode.Impulse);
        estaEnElSuelo = false;
    }

    void PegarALaPelota()
    {
        if (pelota == null || canchaRival == null) return;

        float distancia = Vector3.Distance(transform.position, pelota.transform.position);

        if (distancia <= distanciaParaPegar)
        {
            Rigidbody rbPelota = pelota.GetComponent<Rigidbody>();
            if (rbPelota != null)
            {
                rbPelota.linearVelocity = Vector3.zero;

                
                float destinoZ = transform.position.z;

                
                float distanciaALaRed = Mathf.Abs(transform.position.x);
                float profundidadX = largoCancha - distanciaALaRed;
                profundidadX = Mathf.Clamp(profundidadX, 1.5f, largoCancha);

                float signoX = (canchaRival.position.x > 0) ? 1f : -1f;
                float destinoX = (canchaRival.position.x) + (profundidadX * signoX);

                
                float dispersionX = Random.Range(-0.8f, 0.8f);
                float dispersionZ = Random.Range(-0.8f, 0.8f);

                Vector3 destinoInteligente = new Vector3(destinoX + dispersionX, canchaRival.position.y, destinoZ + dispersionZ);
                Vector3 direccionCancha = (destinoInteligente - pelota.transform.position).normalized;

                Vector3 fuerzaFinal = new Vector3(
                    direccionCancha.x * fuerzaGolpeHorizontal,
                    fuerzaGolpeVertical,
                    direccionCancha.z * fuerzaGolpeHorizontal
                );

                rbPelota.AddForce(fuerzaFinal, ForceMode.Impulse);
            }
        }
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