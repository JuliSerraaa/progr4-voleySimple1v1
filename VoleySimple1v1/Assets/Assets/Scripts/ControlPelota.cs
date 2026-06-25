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

    public void LanzarSaque()
    {
        procesandoPunto = false; 
        rb.isKinematic = false;  

        
        float direccionX = Random.Range(0, 2) == 0 ? -1f : 1f;
        float direccionZ = Random.Range(-0.5f, 0.5f);

        Vector3 fuerzaSaque = new Vector3(direccionX * fuerzaSaqueHorizontal, fuerzaSaqueVertical, direccionZ);
        rb.AddForce(fuerzaSaque, ForceMode.Impulse);
    }

    
    public void SakarAleatorio()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true; 

        transform.position = posicionInicial; 
        ultimoEnTocar = 0;

       
        Invoke("LanzarSaque", 1.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GestorPuntaje.instancia != null && GestorPuntaje.instancia.TextoContieneGanador()) return;
        if (procesandoPunto) return; 

        string nombreObjeto = collision.gameObject.name.ToLower();

        if (nombreObjeto.Contains("player1")) { ultimoEnTocar = 1; return; }
        if (nombreObjeto.Contains("player2")) { ultimoEnTocar = 2; return; }

      
        if (nombreObjeto.Contains("cancha") || nombreObjeto.Contains("floor") || nombreObjeto.Contains("cube"))
        {
            procesandoPunto = true; 

            if (transform.position.x < 0)
            {
                if (GestorPuntaje.instancia != null) GestorPuntaje.instancia.SumarPuntoPlayer2();
            }
            else
            {
                if (GestorPuntaje.instancia != null) GestorPuntaje.instancia.SumarPuntoPlayer1();
            }

            
            SakarAleatorio();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GestorPuntaje.instancia != null && GestorPuntaje.instancia.TextoContieneGanador()) return;
        if (procesandoPunto) return;

        if (other.gameObject.name.ToLower().Contains("zonafuera"))
        {
            procesandoPunto = true; 

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

            SakarAleatorio();
        }
    }
}
