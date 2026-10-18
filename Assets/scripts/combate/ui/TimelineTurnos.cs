using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Banda superior con el orden de turnos de la ronda actual.
//Se construye sola (canvas + entradas), no hay que maquetar nada en el editor.
//Estructura: panel redondeado con los iconos + linea tipo timeline, y el recuadro
//"Ronda X" anclado por debajo solapándose un poco con el panel.
public class TimelineTurnos : MonoBehaviour
{
    [Header("Apariencia")]
    public float tamanoMarco = 84f;
    public float grosorMarco = 8f;
    public float espacioEntre = 14f;
    public float paddingPanel = 18f;      //aire vertical del panel respecto a los iconos
    public float margenLados = 48f;       //aire horizontal extra: la linea sobresale a los lados de los iconos
    public float margenLinea = 18f;       //distancia del borde del panel al extremo/punto de la linea
    public float solapeRonda = 16f;       //cuanto se mete el recuadro de ronda dentro del panel
    public float anchoRonda = 130f;
    public float altoRonda = 32f;
    public float margenSuperior = 10f;

    [Header("Colores")]
    public Color colorEquipoA = new Color(0.25f, 0.5f, 1f);   //azul
    public Color colorEquipoB = new Color(1f, 0.3f, 0.25f);   //rojo
    public Color colorFondoPanel = new Color(0.07f, 0.08f, 0.12f, 0.97f);
    public Color colorFondoRonda = new Color(0.04f, 0.05f, 0.09f, 0.98f);
    public Color colorTextoRonda = new Color(1f, 0.92f, 0.7f);
    [Range(0f, 1f)] public float opacidadIcono = 0.85f;   //1 = opaco, menos = más transparente
    [Range(0f, 1f)] public float atenuadoResto = 0.6f;
    public float escalaTurnoActual = 1.25f;

    [Header("Decoracion")]
    public float grosorLinea = 6f;
    //0 = centrada tras los iconos; negativa = como un raíl por debajo de ellos
    public float desplazamientoLineaY = -48f;
    public Color colorLinea = new Color(0.85f, 0.88f, 0.95f, 0.45f);
    public Color colorBordeActual = new Color(1f, 1f, 1f, 0.95f);
    public float grosorBordeActual = 3f;

    private Transform contenedor;
    private TextMeshProUGUI textoRonda;
    private ControlCombate controlCombate;
    private Sprite spriteRedondeado;
    private Sprite spriteCirculo;

    void Awake()
    {
        spriteRedondeado = CrearSpriteRedondeado(12);
        spriteCirculo = CrearSpriteCirculo(48);
        CrearInterfaz();
    }

    void Start()
    {
        controlCombate = ControlCombate.controlCombate;
        ActualizarRonda();
    }

    //Crea el canvas y el panel (que ya lleva dentro la linea, los puntos y el recuadro de ronda)
    private void CrearInterfaz()
    {
        GameObject canvasGO = new GameObject("TimelineTurnosCanvas");
        canvasGO.transform.SetParent(transform, false);
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10;

        GameObject panelGO = new GameObject("PanelIconos", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        panelGO.transform.SetParent(canvasGO.transform, false);
        RectTransform panel = panelGO.GetComponent<RectTransform>();
        panel.anchorMin = new Vector2(0.5f, 1f);
        panel.anchorMax = new Vector2(0.5f, 1f);
        panel.pivot = new Vector2(0.5f, 1f);
        panel.anchoredPosition = new Vector2(0f, -margenSuperior);

        Image fondo = panelGO.GetComponent<Image>();
        fondo.sprite = spriteRedondeado;
        fondo.type = Image.Type.Sliced;
        fondo.color = colorFondoPanel;

        HorizontalLayoutGroup fila = panelGO.AddComponent<HorizontalLayoutGroup>();
        fila.childAlignment = TextAnchor.MiddleCenter;
        fila.spacing = espacioEntre;
        fila.padding = new RectOffset((int)margenLados, (int)margenLados, (int)paddingPanel, (int)paddingPanel);
        fila.childControlWidth = false;
        fila.childControlHeight = false;
        fila.childForceExpandWidth = false;
        fila.childForceExpandHeight = false;

        ContentSizeFitter ajuste = panelGO.AddComponent<ContentSizeFitter>();
        ajuste.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        ajuste.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        contenedor = panelGO.transform;

        CrearDecoracionLinea(panelGO.transform);
        CrearRecuadroRonda(panelGO.transform);
    }

    //Linea que conecta los iconos (sobresale por los lados) + puntos en los extremos
    private void CrearDecoracionLinea(Transform panel)
    {
        GameObject lineaGO = new GameObject("Linea", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        lineaGO.transform.SetParent(panel, false);
        lineaGO.transform.SetAsFirstSibling(); //detrás de los iconos

        LayoutElement ignoreLinea = lineaGO.AddComponent<LayoutElement>();
        ignoreLinea.ignoreLayout = true;

        RectTransform linea = lineaGO.GetComponent<RectTransform>();
        linea.anchorMin = new Vector2(0f, 0.5f);
        linea.anchorMax = new Vector2(1f, 0.5f);
        linea.offsetMin = new Vector2(margenLinea, desplazamientoLineaY - grosorLinea * 0.5f);
        linea.offsetMax = new Vector2(-margenLinea, desplazamientoLineaY + grosorLinea * 0.5f);

        Image lineaImagen = lineaGO.GetComponent<Image>();
        lineaImagen.sprite = spriteRedondeado;
        lineaImagen.type = Image.Type.Sliced;
        lineaImagen.color = colorLinea;

        CrearPuntoLinea(panel, 0f);
        CrearPuntoLinea(panel, 1f);
    }

    //Punto decorativo en un extremo de la linea (0 = izquierda, 1 = derecha)
    private void CrearPuntoLinea(Transform panel, float lado)
    {
        GameObject puntoGO = new GameObject("Punto", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        puntoGO.transform.SetParent(panel, false);

        LayoutElement ignorePunto = puntoGO.AddComponent<LayoutElement>();
        ignorePunto.ignoreLayout = true;

        RectTransform punto = puntoGO.GetComponent<RectTransform>();
        punto.anchorMin = new Vector2(lado, 0.5f);
        punto.anchorMax = new Vector2(lado, 0.5f);
        punto.pivot = new Vector2(0.5f, 0.5f);
        punto.sizeDelta = Vector2.one * (grosorLinea * 2.2f);

        float offset = lado == 0f ? margenLinea : -margenLinea;
        punto.anchoredPosition = new Vector2(offset, desplazamientoLineaY);

        Image puntoImagen = puntoGO.GetComponent<Image>();
        puntoImagen.sprite = spriteCirculo;
        puntoImagen.color = colorLinea;
    }

    //Mini recuadro "Ronda X": anclado al borde inferior del panel, solapándose un poco
    private void CrearRecuadroRonda(Transform panel)
    {
        GameObject rondaGO = new GameObject("Ronda", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        rondaGO.transform.SetParent(panel, false);
        rondaGO.transform.SetAsLastSibling(); //por delante del panel

        LayoutElement ignoreRonda = rondaGO.AddComponent<LayoutElement>();
        ignoreRonda.ignoreLayout = true;

        RectTransform rondaRect = rondaGO.GetComponent<RectTransform>();
        rondaRect.anchorMin = new Vector2(0.5f, 0f);
        rondaRect.anchorMax = new Vector2(0.5f, 0f);
        rondaRect.pivot = new Vector2(0.5f, 1f);
        rondaRect.sizeDelta = new Vector2(anchoRonda, altoRonda);
        //positivo hacia arriba: el borde superior del recuadro queda dentro del panel
        rondaRect.anchoredPosition = new Vector2(0f, solapeRonda);

        Image rondaFondo = rondaGO.GetComponent<Image>();
        rondaFondo.sprite = spriteRedondeado;
        rondaFondo.type = Image.Type.Sliced;
        rondaFondo.color = colorFondoRonda;

        GameObject textoGO = new GameObject("TextoRonda", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        textoGO.transform.SetParent(rondaGO.transform, false);
        RectTransform textoRect = textoGO.GetComponent<RectTransform>();
        textoRect.anchorMin = Vector2.zero;
        textoRect.anchorMax = Vector2.one;
        textoRect.offsetMin = Vector2.zero;
        textoRect.offsetMax = Vector2.zero;

        textoRonda = textoGO.GetComponent<TextMeshProUGUI>();
        textoRonda.font = TMP_Settings.defaultFontAsset;
        if (textoRonda.font == null)
            textoRonda.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");

        textoRonda.fontStyle = FontStyles.Bold;
        textoRonda.alignment = TextAlignmentOptions.Center;
        textoRonda.color = colorTextoRonda;
        textoRonda.enableAutoSizing = true;
        textoRonda.fontSizeMin = 10f;
        textoRonda.fontSizeMax = 20f;
        textoRonda.text = "Ronda 1";
    }

    //Reconstruye la banda con la lista ordenada de la ronda
    public void MostrarOrden(List<PosicionPersonaje> orden)
    {
        if (contenedor == null)
        {
            return;
        }

        //Se quitan solo las entradas; la linea, los puntos y el recuadro de ronda se conservan.
        for (int i = contenedor.childCount - 1; i >= 0; i--)
        {
            Transform hijo = contenedor.GetChild(i);
            if (EsDecoracion(hijo))
            {
                continue;
            }

            //Se desenganchan antes de destruir para que los nuevos iconos no queden
            //detrás de los viejos (Destroy no es inmediato).
            hijo.SetParent(null, false);
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
        ActualizarRonda();
    }

    //Quita el turno ya consumido (el primero) y resalta el siguiente
    public void AvanzarTurno()
    {
        Transform primero = PrimeraEntrada();
        if (primero == null)
        {
            return;
        }

        primero.SetParent(null);
        Destroy(primero.gameObject);

        ActualizarResaltado();
    }

    //Primer hijo que es una entrada de personaje (ignora la decoracion)
    private Transform PrimeraEntrada()
    {
        if (contenedor == null)
        {
            return null;
        }

        for (int i = 0; i < contenedor.childCount; i++)
        {
            Transform hijo = contenedor.GetChild(i);
            if (!EsDecoracion(hijo))
            {
                return hijo;
            }
        }

        return null;
    }

    //La decoracion se marca con LayoutElement.ignoreLayout para que el layout la ignore
    private bool EsDecoracion(Transform hijo)
    {
        LayoutElement ajuste = hijo.GetComponent<LayoutElement>();
        return ajuste != null && ajuste.ignoreLayout;
    }

    //Pone "Ronda X" con el número actual de ControlCombate
    public void ActualizarRonda()
    {
        if (textoRonda == null)
        {
            return;
        }

        if (controlCombate == null)
        {
            controlCombate = ControlCombate.controlCombate;
        }

        int ronda = controlCombate != null ? controlCombate.numeroRonda : 1;
        textoRonda.text = "Ronda " + ronda;
    }

    //Crea una entrada: marco del color del equipo + icono (o inicial si aún no hay sprite)
    private void CrearEntrada(Personaje p)
    {
        GameObject entradaGO = new GameObject("Turno_" + p.nombre);
        entradaGO.transform.SetParent(contenedor, false);
        RectTransform entrada = entradaGO.AddComponent<RectTransform>();
        entrada.sizeDelta = new Vector2(tamanoMarco, tamanoMarco);

        //Tamaño preferido para que el layout y el ajuste de tamaño se calculen bien.
        LayoutElement ajusteEntrada = entradaGO.AddComponent<LayoutElement>();
        ajusteEntrada.preferredWidth = tamanoMarco;
        ajusteEntrada.preferredHeight = tamanoMarco;

        Image marco = entradaGO.AddComponent<Image>();
        marco.color = p.equipo == Equipo.equipoA ? colorEquipoA : colorEquipoB;

        //Contorno decorativo; solo se activa en el turno actual (ver ActualizarResaltado).
        Outline contorno = entradaGO.AddComponent<Outline>();
        contorno.effectColor = colorBordeActual;
        contorno.effectDistance = new Vector2(grosorBordeActual, grosorBordeActual);
        contorno.enabled = false;

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
            icono.color = new Color(1f, 1f, 1f, opacidadIcono);
        }
        else
        {
            //sin sprite todavía: fondo oscuro con la inicial del nombre
            icono.color = new Color(0.12f, 0.12f, 0.12f, opacidadIcono);

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

    //El primero (turno actual) grande, opaco y con contorno; el resto atenuados
    private void ActualizarResaltado()
    {
        Transform actual = PrimeraEntrada();

        for (int i = 0; i < contenedor.childCount; i++)
        {
            Transform hijo = contenedor.GetChild(i);
            if (EsDecoracion(hijo))
            {
                continue;
            }

            bool esActual = hijo == actual;
            hijo.localScale = Vector3.one * (esActual ? escalaTurnoActual : 1f);

            CanvasGroup grupo = hijo.GetComponent<CanvasGroup>();
            if (grupo != null)
            {
                grupo.alpha = esActual ? 1f : atenuadoResto;
            }

            Outline contorno = hijo.GetComponent<Outline>();
            if (contorno != null)
            {
                contorno.enabled = esActual;
            }
        }
    }

    //Genera un sprite blanco de esquinas redondeadas con borde para 9-slice,
    //así sirve para el panel y para el recuadro a cualquier tamaño.
    private Sprite CrearSpriteRedondeado(int radio)
    {
        int tam = radio * 2 + 4;
        Texture2D textura = new Texture2D(tam, tam, TextureFormat.RGBA32, false);
        textura.hideFlags = HideFlags.HideAndDontSave;
        textura.wrapMode = TextureWrapMode.Clamp;
        textura.filterMode = FilterMode.Bilinear;

        for (int y = 0; y < tam; y++)
        {
            for (int x = 0; x < tam; x++)
            {
                int dx = Mathf.Min(x, tam - 1 - x);
                int dy = Mathf.Min(y, tam - 1 - y);

                float alpha = 1f;
                if (dx < radio && dy < radio)
                {
                    float ax = radio - dx - 0.5f;
                    float ay = radio - dy - 0.5f;
                    float distancia = Mathf.Sqrt(ax * ax + ay * ay);
                    alpha = Mathf.Clamp01(radio - distancia + 0.5f);
                }

                textura.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
            }
        }

        textura.Apply();

        return Sprite.Create(
            textura,
            new Rect(0f, 0f, tam, tam),
            new Vector2(0.5f, 0.5f),
            100f,
            0,
            SpriteMeshType.FullRect,
            new Vector4(radio, radio, radio, radio));
    }

    //Genera un sprite circular blanco (para los puntos de la linea)
    private Sprite CrearSpriteCirculo(int diametro)
    {
        Texture2D textura = new Texture2D(diametro, diametro, TextureFormat.RGBA32, false);
        textura.hideFlags = HideFlags.HideAndDontSave;
        textura.wrapMode = TextureWrapMode.Clamp;
        textura.filterMode = FilterMode.Bilinear;

        float radio = diametro * 0.5f;
        for (int y = 0; y < diametro; y++)
        {
            for (int x = 0; x < diametro; x++)
            {
                float dx = x + 0.5f - radio;
                float dy = y + 0.5f - radio;
                float distancia = Mathf.Sqrt(dx * dx + dy * dy);
                float alpha = Mathf.Clamp01(radio - distancia + 0.5f);
                textura.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
            }
        }

        textura.Apply();

        return Sprite.Create(textura, new Rect(0f, 0f, diametro, diametro), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect);
    }

    private void OnDestroy()
    {
        DestruirSprite(spriteRedondeado);
        DestruirSprite(spriteCirculo);
    }

    private void DestruirSprite(Sprite sprite)
    {
        if (sprite == null)
        {
            return;
        }

        if (sprite.texture != null)
        {
            Destroy(sprite.texture);
        }

        Destroy(sprite);
    }
}
