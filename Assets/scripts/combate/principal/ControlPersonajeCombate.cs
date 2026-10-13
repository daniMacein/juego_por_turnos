using System.Collections;
using UnityEngine;

public class ControlPersonajeCombate : MonoBehaviour
{

    [Header("Tiempos (segundos)")]
    public float duracionIda = 0.35f;
    public float duracionVuelta = 0.45f;

    [Header("Mini salto")]
    public float alturaSalto = 1.2f;

    //punto de partida, se guarda solo la primera vez que sales
    private Vector3 posicionOrigen;
    private Quaternion rotacionOrigen;
    private bool origenGuardado = false;


    //Te desplazas enfrente del objetivo con un mini salto, mirando hacia él.
    //ubicacion: punto de destino (el empty frente al objetivo).
    //mirarA: a quién mirar al llegar (normalmente el objetivo); si es null se usa la rotación de ubicacion.
    public IEnumerator MoverseAobjetivo(Transform ubicacion, Transform mirarA = null)
    {
        GuardarOrigen();

        Quaternion rotFinal = ubicacion.rotation;
        if (mirarA != null)
        {
            rotFinal = MirarHacia(transform.position, mirarA.position);
        }

        yield return Desplazar(ubicacion.position, rotFinal, duracionIda);
    }

    //Vuelves al punto de partida con otro mini salto, recuperando tu orientación
    public IEnumerator VolverAOrigen()
    {
        if (!origenGuardado)
        {
            yield break;
        }

        yield return Desplazar(posicionOrigen, rotacionOrigen, duracionVuelta);
    }

    //Vuelve a guardar el origen (por si el personaje ha cambiado de sitio entre rondas)
    public void ReiniciarOrigen()
    {
        posicionOrigen = transform.position;
        rotacionOrigen = transform.rotation;
        origenGuardado = true;
    }


    private void GuardarOrigen()
    {
        if (!origenGuardado)
        {
            ReiniciarOrigen();
        }
    }

    //Desplazamiento rápido en línea recta con arco de salto y giro suavizado
    private IEnumerator Desplazar(Vector3 destino, Quaternion rotDestino, float duracion)
    {
        Vector3 inicio = transform.position;
        Quaternion rotInicio = transform.rotation;

        float tiempo = 0f;
        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.Clamp01(tiempo / duracion);
            float suave = t * t * (3f - 2f * t);

            Vector3 pos = Vector3.Lerp(inicio, destino, suave);
            pos.y += Mathf.Sin(t * Mathf.PI) * alturaSalto;
            transform.position = pos;

            transform.rotation = Quaternion.Slerp(rotInicio, rotDestino, suave);

            yield return null;
        }

        transform.position = destino;
        transform.rotation = rotDestino;
    }

    //Rotación horizontal hacia un punto (sin inclinar arriba/abajo)
    private Quaternion MirarHacia(Vector3 desde, Vector3 hacia)
    {
        Vector3 direccion = hacia - desde;
        direccion.y = 0f;
        if (direccion.sqrMagnitude < 0.0001f)
        {
            return transform.rotation;
        }
        return Quaternion.LookRotation(direccion);
    }

    
}
