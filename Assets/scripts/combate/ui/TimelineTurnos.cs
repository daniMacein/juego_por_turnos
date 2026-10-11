using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Banda superior con el orden de turnos de la ronda actual.
//Se construye sola (canvas + entradas), no hay que maquetar nada en el editor.
//Cada entrada lleva el icono del personaje con un marco del color de su equipo.
public class TimelineTurnos : MonoBehaviour
{
    [Header("Apariencia")]
    public float tamanoMarco = 84f;
    public float grosorMarco = 8f;
    public float espacioEntre = 10f;
    public float alturaBanda = 120f;
    public Color colorEquipoA = new Color(0.25f, 0.5f, 1f); //azul
    public Color colorEquipoB = new Color(1f, 0.3f, 0.25f); //rojo
    public Color colorFondoBanda = new Color(0f, 0f, 0f, 0.45f);
    [Range(0f, 1f)] public float atenuadoResto = 0.6f;
    public float escalaTurnoActual = 1.25f;

    private Transform contenedor;

    void Awake()
    {
        CrearInterfaz();
    }

    //Crea el canvas, la banda superior y el contenedor de entradas
    private void CrearInterfaz()
    {
        GameObject canvasGO = new GameObject("TimelineTurnosCanvas");
        canvasGO.transform.SetParent(transform, false);
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10;

        GameObject bandaGO = new GameObject("Banda");
        bandaGO.transform.SetParent(canvasGO.transform, false);
        RectTransform banda = bandaGO.AddComponent<RectTransform>();
        banda.anchorMin = new Vector2(0f, 1f);
        banda.anchorMax = new Vector2(1f, 1f);
        banda.pivot = new Vector2(0.5f, 1f);
        banda.anchoredPosition = Vector2.zero;
        banda.sizeDelta = new Vector2(0f, alturaBanda);

        Image fondo = bandaGO.AddComponent<Image>();
        fondo.color = colorFondoBanda;

        HorizontalLayoutGroup layout = bandaGO.AddComponent<HorizontalLayoutGroup>();
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.spacing = espacioEntre;
        layout.childControlWidth = false;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;

        contenedor = bandaGO.transform;
    }

    //Reconstruye la banda con la lista ordenada de la ronda
    public void MostrarOrden(List<PosicionPersonaje> orden)
    {
        if (contenedor == null)
        {
            return;
        }

        foreach (Transform hijo in contenedor)
        {
            Destroy(hijo.gameObject);
        }

        if (orden != null)
        {
            foreach (PosicionPersonaje pp in orden)
            {
                if (pp != null && pp.personaje != null)
                {
                    CrearEntrada(pp.personaje);
                }
            }
        }

        ActualizarResaltado();
    }

    //Quita el turno ya consumido (el primero) y resalta el siguiente
    public void AvanzarTurno()
    {
        if (contenedor == null || contenedor.childCount == 0)
        {
            return;
        }

        Transform primero = contenedor.GetChild(0);
        primero.SetParent(null);
        Destroy(primero.gameObject);

        ActualizarResaltado();
    }

    //Crea una entrada: marco del color del equipo + icono (o inicial si aún no hay sprite)
    private void CrearEntrada(Personaje p)
    {
        GameObject entradaGO = new GameObject("Turno_" + p.nombre);
        entradaGO.transform.SetParent(contenedor, false);
        RectTransform entrada = entradaGO.AddComponent<RectTransform>();
        entrada.sizeDelta = new Vector2(tamanoMarco, tamanoMarco);

        Image marco = entradaGO.AddComponent<Image>();
        marco.color = p.equipo == Equipo.equipoA ? colorEquipoA : colorEquipoB;

        entradaGO.AddComponent<CanvasGroup>();

        GameObject iconoGO = new GameObject("Icono");
        iconoGO.transform.SetParent(entradaGO.transform, false);
        RectTransform iconoRect = iconoGO.AddComponent<RectTransform>();
        iconoRect.anchorMin = Vector2.zero;
        iconoRect.anchorMax = Vector2.one;
        iconoRect.offsetMin = new Vector2(grosorMarco, grosorMarco);
        iconoRect.offsetMax = new Vector2(-grosorMarco, -grosorMarco);

        Image icono = iconoGO.AddComponent<Image>();
        if (p.icono != null)
        {
            icono.sprite = p.icono;
        }
        else
        {
            //sin sprite todavía: fondo oscuro con la inicial del nombre
            icono.color = new Color(0.12f, 0.12f, 0.12f, 1f);

            GameObject letraGO = new GameObject("Letra");
            letraGO.transform.SetParent(iconoGO.transform, false);
            RectTransform letraRect = letraGO.AddComponent<RectTransform>();
            letraRect.anchorMin = Vector2.zero;
            letraRect.anchorMax = Vector2.one;
            letraRect.offsetMin = Vector2.zero;
            letraRect.offsetMax = Vector2.zero;

            TextMeshProUGUI letra = letraGO.AddComponent<TextMeshProUGUI>();
            letra.text = string.IsNullOrEmpty(p.nombre) ? "?" : p.nombre.Substring(0, 1).ToUpper();
            letra.fontSize = tamanoMarco * 0.5f;
            letra.alignment = TextAlignmentOptions.Center;
            letra.color = Color.white;
        }
    }

    //El primero (turno actual) grande y opaco, el resto atenuados
    private void ActualizarResaltado()
    {
        for (int i = 0; i < contenedor.childCount; i++)
        {
            Transform hijo = contenedor.GetChild(i);
            bool esActual = i == 0;
            hijo.localScale = Vector3.one * (esActual ? escalaTurnoActual : 1f);
            CanvasGroup grupo = hijo.GetComponent<CanvasGroup>();
            if (grupo != null)
            {
                grupo.alpha = esActual ? 1f : atenuadoResto;
            }
        }
    }
}
