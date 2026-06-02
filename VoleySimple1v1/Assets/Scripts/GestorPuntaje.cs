using UnityEngine;
using TMPro; // IMPORTANTE: Para controlar el texto de TextMeshPro

public class GestorPuntaje : MonoBehaviour
{
    // Instancia estática para poder llamarlo desde el script de la pelota fácilmente
    public static GestorPuntaje instancia;

    [Header("Componente de Texto")]
    public TextMeshProUGUI textoPuntaje;

    private int puntosP1 = 0;
    private int puntosP2 = 0;

    void Awake()
    {
        instancia = this;
    }

    void Start()
    {
        ActualizarMarcador();
    }

    public void SumarPuntoPlayer1()
    {
        puntosP1++;
        ActualizarMarcador();
    }

    public void SumarPuntoPlayer2()
    {
        puntosP2++;
        ActualizarMarcador();
    }

    void ActualizarMarcador()
    {
        // Cambia el texto en pantalla (ej: "3 - 2")
        textoPuntaje.text = puntosP1 + " - " + puntosP2;
    }
}