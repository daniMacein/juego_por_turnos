using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InterfazCombate : MonoBehaviour
{
 public TextMeshProUGUI    nombre;

    public botonInfoAtaque   textoBoton1;
     public botonInfoAtaque   textoBoton2;
      public botonInfoAtaque   textoBoton3;

       public botonInfoAtaque   textoBoton4;


      public static InterfazCombate interfazCombate;

      private GameObject menuCombate;

    //*** Animación de mostrar/ocultar (los elementos se deslizan por el borde inferior)
    public float duracionAnimacion = 0.3f;

    private RectTransform rectMenu;
    private List<RectTransform> elementos = new List<RectTransform>();
    private List<Vector2> posicionesOriginales = new List<Vector2>();
    private Coroutine animacionEnCurso;
    private float desplazamientoActual = 0f; //0 = visible, >0 = hundido hacia abajo


    void Awake()
    {
        interfazCombate=this;
    }

    void Start()
    {
        menuCombate = transform.GetChild(0).gameObject;
        rectMenu = menuCombate.GetComponent<RectTransform>();

        //guardamos la posición de cada hijo directo para desplazarlos en bloque
        //(el Canvas raíz lo controla Unity y no se puede deslizar él mismo)
        foreach (Transform hijo in menuCombate.transform)
        {
            RectTransform rt = hijo as RectTransform;
            if (rt != null)
            {
                elementos.Add(rt);
                posicionesOriginales.Add(rt.anchoredPosition);
            }
        }

        menuCombate.SetActive(false);
    }


    public void ResetBotonesPersonaje()
    {
        personaje=null;
        OcultarConAnimacion();

    }
    public void MostrarMenuPersonaje(Personaje p)
        {

            if (p == null)
            {
                Debug.LogError("ERROR: Se llamó a MostrarMenuPersonaje con p = NULL");
                return;
            }

            nombre.text=p.nombre;

            textoBoton1.MostrarPersonaje(p,0);
            textoBoton2.MostrarPersonaje(p,1);
            textoBoton3.MostrarPersonaje(p,2);
            textoBoton4.MostrarPersonaje(p,3);
            personaje=p;

            MostrarConAnimacion();


        }

    private Personaje personaje;

    //Muestra el menú con animación: los elementos suben desde el borde inferior
    public void MostrarConAnimacion()
    {
        if (animacionEnCurso != null)
        {
            StopCoroutine(animacionEnCurso);
        }

        //si estaba desactivado, empezar fuera de pantalla para que se vea la entrada
        //(la primera vez no hay un ocultado anterior que lo haya dejado abajo)
        bool estabaOculto = !menuCombate.activeSelf;
        menuCombate.SetActive(true);
        if (estabaOculto)
        {
            AplicarDesplazamiento(DistanciaOculta());
        }

        animacionEnCurso = StartCoroutine(Animar(0f));
    }

    //Oculta el menú con animación: los elementos se deslizan hacia el borde
    //inferior y al terminar se desactiva la interfaz
    public void OcultarConAnimacion()
    {
        if (!menuCombate.activeSelf)
        {
            return;
        }

        if (animacionEnCurso != null)
        {
            StopCoroutine(animacionEnCurso);
        }

        animacionEnCurso = StartCoroutine(Animar(DistanciaOculta()));
    }

    //Cuánto hay que bajar para que todo quede fuera de pantalla
    private float DistanciaOculta()
    {
        return rectMenu.rect.height + 200f;
    }

    //Aplica el desplazamiento a todos los elementos manteniendo su colocación
    private void AplicarDesplazamiento(float d)
    {
        desplazamientoActual = d;
        Vector2 off = new Vector2(0f, -d);
        for (int i = 0; i < elementos.Count; i++)
        {
            elementos[i].anchoredPosition = posicionesOriginales[i] + off;
        }
    }

    private IEnumerator Animar(float destino)
    {
        float inicio = desplazamientoActual;

        float tiempo = 0f;
        while (tiempo < duracionAnimacion)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.Clamp01(tiempo / duracionAnimacion);
            t = t * t * (3f - 2f * t); //suavizado
            AplicarDesplazamiento(Mathf.Lerp(inicio, destino, t));
            yield return null;
        }

        AplicarDesplazamiento(destino);

        if (destino > 0f)
        {
            menuCombate.SetActive(false);
        }

        animacionEnCurso = null;
    }

    public void AlpulsarBoton1()
    {
        if (personaje == null)
        {
            Debug.LogError("Se pulsó el botón sin personaje seleccionado");
            return;
        }
        personaje.Ataque1();

    }

    public void AlpulsarBoton2()
    {
        if (personaje == null)
        {
            Debug.LogError("Se pulsó el botón sin personaje seleccionado");
            return;
        }
        personaje.Ataque2();
    }

    public void AlpulsarBoton3()
    {
        if (personaje == null)
        {
            Debug.LogError("Se pulsó el botón sin personaje seleccionado");
            return;
        }
        personaje.Ataque3();
    }

    public void AlpulsarBoton4()
    {
        if (personaje == null)
        {
            Debug.LogError("Se pulsó el botón sin personaje seleccionado");
            return;
        }
        personaje.Ataque4();
    }
}
