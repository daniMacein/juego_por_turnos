using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraCombate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static CamaraCombate camaraCombate;

    [Header("Cámara (vacío = la del mismo objeto, o la MainCamera)")]
    public Camera camara;

    [Header("Acercamiento al empezar el ataque")]
    public float duracionAcercamiento = 0.8f;
    [Range(0f, 1f)] public float centradoEnAccion = 0.5f;
    public float zoomAtaque = 0.75f; //multiplicador: menor de 1 acerca

    [Header("Sacudida en el golpe")]
    public float intensidadSacudida = 0.35f;
    public float duracionSacudida = 0.35f;

    [Header("Vuelta a la posición original")]
    public float duracionRetorno = 0.6f;

    //estado original de la cámara
    private Vector3 posOriginal;
    private Quaternion rotOriginal;
    private float zoomOriginal;
    private bool esOrtografica;

    //estado dirigido (lo que la cámara debe tener sin contar la sacudida)
    private Vector3 posBase;
    private float zoomBase;

    private Coroutine animacionEnCurso;
    private float trauma; //1 = sacudida máxima, decae a 0

    void Awake()
    {
        camaraCombate=this;

        if (camara == null)
        {
            camara = GetComponent<Camera>();
        }
        if (camara == null)
        {
            camara = Camera.main;
        }
    }

    void Start()
    {
        posOriginal = camara.transform.position;
        rotOriginal = camara.transform.rotation;
        esOrtografica = camara.orthographic;
        //con cámara ortográfica el "zoom" es el size; con perspectiva, el fov
        zoomOriginal = esOrtografica ? camara.orthographicSize : camara.fieldOfView;

        posBase = posOriginal;
        zoomBase = zoomOriginal;
    }

    void LateUpdate()
    {
        if (trauma > 0f)
        {
            trauma = Mathf.Max(0f, trauma - Time.deltaTime / duracionSacudida);
        }

        //sacudida: desplazamiento aleatorio suave, fuerte al principio y decayendo
        float s = trauma * trauma * intensidadSacudida;
        Vector3 desp = new Vector3(
            (Mathf.PerlinNoise(Time.time * 55f, 0f) - 0.5f) * 2f * s,
            (Mathf.PerlinNoise(0f, Time.time * 55f) - 0.5f) * 2f * s,
            0f);

        camara.transform.position = posBase + rotOriginal * desp;
        camara.transform.rotation = rotOriginal * Quaternion.Euler(0f, 0f, desp.x * 2f);

        if (esOrtografica)
        {
            camara.orthographicSize = zoomBase;
        }
        else
        {
            camara.fieldOfView = zoomBase;
        }
    }


    //Al empezar el ataque: paneo lento hacia la acción + zoom de acercamiento
    public void moviminetoAtaque(Personaje atacante, List<Personaje> objetivos)
    {
        Vector3 medio = atacante != null ? atacante.transform.position : posOriginal;
        int n = 1;
        if (objetivos != null)
        {
            foreach (Personaje p in objetivos)
            {
                if (p != null && p != atacante)
                {
                    medio += p.transform.position;
                    n++;
                }
            }
        }
        medio /= n;

        Vector3 destino = posOriginal;
        destino.x = Mathf.Lerp(posOriginal.x, medio.x, centradoEnAccion);
        destino.z = Mathf.Lerp(posOriginal.z, medio.z, centradoEnAccion * 0.5f);

        ArrancarAnimacion(destino, zoomOriginal * zoomAtaque, duracionAcercamiento);
    }

    //En el momento justo del golpe: sacudida de cámara
    public void movimientoGolpe()
    {
        trauma = 1f;
    }

    //Al acabar el ataque: volver a como estaba
    public void restaurarOriginal()
    {
        trauma = 0f;
        ArrancarAnimacion(posOriginal, zoomOriginal, duracionRetorno);
    }


    private void ArrancarAnimacion(Vector3 destinoPos, float destinoZoom, float duracion)
    {
        if (animacionEnCurso != null)
        {
            StopCoroutine(animacionEnCurso);
        }
        animacionEnCurso = StartCoroutine(AnimarHacia(destinoPos, destinoZoom, duracion));
    }

    private IEnumerator AnimarHacia(Vector3 destinoPos, float destinoZoom, float duracion)
    {
        Vector3 inicioPos = posBase;
        float inicioZoom = zoomBase;

        float tiempo = 0f;
        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.Clamp01(tiempo / duracion);
            t = t * t * (3f - 2f * t); //suavizado
            posBase = Vector3.Lerp(inicioPos, destinoPos, t);
            zoomBase = Mathf.Lerp(inicioZoom, destinoZoom, t);
            yield return null;
        }

        posBase = destinoPos;
        zoomBase = destinoZoom;
        animacionEnCurso = null;
    }
}
