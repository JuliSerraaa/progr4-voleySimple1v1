using UnityEngine;

// Este ScriptableObject guarda el dato del volumen del juego.
// Al ser un asset (un archivo .asset dentro del proyecto), todos los
// scripts que lo referencien leen y escriben el MISMO valor, sin
// necesidad de singletons ni de pasar referencias entre escenas.
[CreateAssetMenu(fileName = "DatosVolumen", menuName = "Configuracion/Datos de Volumen")]
public class DatosVolumen : ScriptableObject
{
    [Range(0f, 1f)]
    public float volumen = 1f;

    // Usamos PlayerPrefs para que el valor sobreviva a cerrar el juego.
    // Un ScriptableObject, en una build, NO graba solo los cambios en
    // disco: si no usaramos PlayerPrefs aca, el volumen se reiniciaria
    // cada vez que se abre el juego de nuevo.
    private const string CLAVE_GUARDADO = "VolumenJuego";

    public void Guardar()
    {
        PlayerPrefs.SetFloat(CLAVE_GUARDADO, volumen);
        PlayerPrefs.Save();
    }

    public void Cargar()
    {
        volumen = PlayerPrefs.GetFloat(CLAVE_GUARDADO, 1f);
    }
}
