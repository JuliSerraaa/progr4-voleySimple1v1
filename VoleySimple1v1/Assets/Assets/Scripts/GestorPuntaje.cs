using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GestorPuntaje : MonoBehaviour
{
    public static GestorPuntaje instancia;

    [Header("Componente de Texto")]
    public TextMeshProUGUI textoPuntaje;

    private int puntosP1 = 0;
    private int puntosP2 = 0;
    private bool juegoTerminado = false;

    void Awake()
    {
        instancia = this;
    }

    void Start()
    {
        Time.timeScale = 1f; 
        ActualizarMarcador();
    }

    void Update()
    {
        
        if (juegoTerminado && Keyboard.current != null && Keyboard.current[Key.R].wasPressedThisFrame)
        {
            ReiniciarJuego();
        }
    }

    public void SumarPuntoPlayer1()
    {
        if (juegoTerminado) return;
        puntosP1++;
        ActualizarMarcador();
        ChequearGanador();
    }

    public void SumarPuntoPlayer2()
    {
        if (juegoTerminado) return;
        puntosP2++;
        ActualizarMarcador();
        ChequearGanador();
    }

    void ActualizarMarcador()
    {
        if (textoPuntaje != null)
            textoPuntaje.text = puntosP1 + " - " + puntosP2;
    }

    void ChequearGanador()
    {
        if (puntosP1 >= 5) TerminarPartida("¡GANÓ EL JUGADOR 1!");
        else if (puntosP2 >= 5) TerminarPartida("¡GANÓ EL JUGADOR 2!");
    }

    void TerminarPartida(string mensajeGanador)
    {
        juegoTerminado = true;
        if (textoPuntaje != null)
            textoPuntaje.text = mensajeGanador + "\n<size=22>Presioná 'R' para revancha</size>";

        Time.timeScale = 0f;
    }

    public bool TextoContieneGanador()
    {
        return juegoTerminado;
    }

    public void ReiniciarJuego()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
