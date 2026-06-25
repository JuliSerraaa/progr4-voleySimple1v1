using UnityEngine;
using UnityEngine.UI;

// Script para el slider del menu de opciones.
// Lee y escribe el volumen en el MISMO asset DatosVolumen que usa
// GestorSonido, asi se mantienen siempre sincronizados.
public class ConfiguradorSonido : MonoBehaviour
{
    [Header("Referencias")]
    public Slider sliderVolumen;
    public DatosVolumen datosVolumen;

    void Start()
    {
        if (datosVolumen == null)
        {
            Debug.LogWarning("ConfiguradorSonido: falta asignar DatosVolumen en el Inspector.");
            return;
        }

        datosVolumen.Cargar();

        if (sliderVolumen != null)
        {
            sliderVolumen.minValue = 0f;
            sliderVolumen.maxValue = 1f;
            sliderVolumen.value = datosVolumen.volumen;
            sliderVolumen.onValueChanged.AddListener(CambiarVolumen);
        }

        AudioListener.volume = datosVolumen.volumen;
    }

    public void CambiarVolumen(float nuevoValor)
    {
        datosVolumen.volumen = nuevoValor;
        AudioListener.volume = nuevoValor;
        datosVolumen.Guardar();
    }

    void OnDestroy()
    {
        if (sliderVolumen != null)
            sliderVolumen.onValueChanged.RemoveListener(CambiarVolumen);
    }
}
