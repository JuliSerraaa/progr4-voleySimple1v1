using UnityEngine;

// Maneja la musica de fondo y los efectos de sonido del juego.
// Al arrancar, carga el volumen guardado (via DatosVolumen) y lo
// aplica con AudioListener.volume, que es el "volumen maestro":
// afecta a TODOS los AudioSource de la escena a la vez.
public class GestorSonido : MonoBehaviour
{
    public static GestorSonido instancia;

    [Header("Datos de Volumen (ScriptableObject)")]
    public DatosVolumen datosVolumen;

    [Header("Audio Sources")]
    public AudioSource fuenteMusica;
    public AudioSource fuenteEfectos;

    [Header("Clips de Efectos")]
    public AudioClip clipSalto;
    public AudioClip clipGolpe;
    public AudioClip clipPunto;
    public AudioClip clipVictoria;

    void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        instancia = this;
        DontDestroyOnLoad(gameObject);

        if (datosVolumen != null)
        {
            datosVolumen.Cargar();
            AplicarVolumen(datosVolumen.volumen);
        }
    }

    public void AplicarVolumen(float valor)
    {
        AudioListener.volume = valor;
    }

    public void ReproducirEfecto(AudioClip clip)
    {
        if (fuenteEfectos != null && clip != null)
            fuenteEfectos.PlayOneShot(clip);
    }

    public void ReproducirSalto() => ReproducirEfecto(clipSalto);
    public void ReproducirGolpe() => ReproducirEfecto(clipGolpe);
    public void ReproducirPunto() => ReproducirEfecto(clipPunto);
    public void ReproducirVictoria() => ReproducirEfecto(clipVictoria);
}
