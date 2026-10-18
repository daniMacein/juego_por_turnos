using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image BarraDeVida;
    public GameObject Fondo;
   
    public Personaje personaje;

    public TextMeshProUGUI textoVida;
    [SerializeField] private Color colorDanio = new Color(0.9f, 0.08f, 0.08f, 1f);
    [SerializeField] private float duracionDanio = 2f;

    private Image barraDanio;
    private Camera camara;
    private Coroutine corrutinaOcultarDanio;
    private float vidaAnteriorNormalizada;
    private float vidaRetrasadaNormalizada;
    private bool estadoInicializado;

    private void Start()
    {
        camara = Camera.main;

        if (personaje == null)
            personaje = GetComponentInParent<Personaje>();

        ModificarBarraVida();
    }

    public void ModificarBarraVida()
    {
        if (personaje == null)
            personaje = GetComponentInParent<Personaje>();

        if (BarraDeVida == null || personaje == null)
            return;

        float vidaMaxima = personaje.vidaMaxima;
        float vidaActual = personaje.vida;
        float porcentajeVida = vidaMaxima > 0f
            ? Mathf.Clamp01(vidaActual / vidaMaxima)
            : 0f;

        PrepararCapaDanio();

        BarraDeVida.fillAmount = porcentajeVida;
        if (textoVida != null)
            textoVida.text = Mathf.RoundToInt(vidaActual) + "/" + Mathf.RoundToInt(vidaMaxima);

        if (!estadoInicializado)
        {
            vidaAnteriorNormalizada = porcentajeVida;
            vidaRetrasadaNormalizada = porcentajeVida;
            estadoInicializado = true;
            barraDanio.fillAmount = 0f;
            return;
        }

        if (porcentajeVida < vidaAnteriorNormalizada)
        {
            // Si llegan varios golpes seguidos, la capa roja acumula el tramo perdido.
            vidaRetrasadaNormalizada = Mathf.Max(vidaRetrasadaNormalizada, vidaAnteriorNormalizada);
            barraDanio.fillAmount = vidaRetrasadaNormalizada;

            if (corrutinaOcultarDanio != null)
                StopCoroutine(corrutinaOcultarDanio);

            corrutinaOcultarDanio = StartCoroutine(OcultarDanioTrasUnTiempo());
        }
        else if (porcentajeVida > vidaAnteriorNormalizada)
        {
            // La curación limpia el tramo rojo para que no se superponga a la vida actual.
            if (corrutinaOcultarDanio != null)
            {
                StopCoroutine(corrutinaOcultarDanio);
                corrutinaOcultarDanio = null;
            }

            vidaRetrasadaNormalizada = porcentajeVida;
            barraDanio.fillAmount = 0f;
        }

        vidaAnteriorNormalizada = porcentajeVida;
    }

    private void PrepararCapaDanio()
    {
        if (barraDanio != null)
            return;

        RectTransform barraRect = BarraDeVida.rectTransform;
        GameObject objetoDanio = new GameObject(
            "TramoDanio",
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(Image));

        objetoDanio.transform.SetParent(barraRect.parent, false);
        barraDanio = objetoDanio.GetComponent<Image>();

        RectTransform rectDanio = barraDanio.rectTransform;
        rectDanio.anchorMin = barraRect.anchorMin;
        rectDanio.anchorMax = barraRect.anchorMax;
        rectDanio.pivot = barraRect.pivot;
        rectDanio.anchoredPosition = barraRect.anchoredPosition;
        rectDanio.sizeDelta = barraRect.sizeDelta;
        rectDanio.localRotation = barraRect.localRotation;
        rectDanio.localScale = barraRect.localScale;

        barraDanio.sprite = BarraDeVida.sprite;
        barraDanio.type = Image.Type.Filled;
        barraDanio.fillMethod = BarraDeVida.fillMethod;
        barraDanio.fillOrigin = BarraDeVida.fillOrigin;
        barraDanio.fillClockwise = BarraDeVida.fillClockwise;
        barraDanio.color = colorDanio;
        barraDanio.raycastTarget = false;
        barraDanio.maskable = BarraDeVida.maskable;
        barraDanio.fillAmount = 0f;

        // Se dibuja detrás de la vida verde y delante del fondo negro.
        rectDanio.SetAsFirstSibling();
    }

    private IEnumerator OcultarDanioTrasUnTiempo()
    {
        yield return new WaitForSeconds(duracionDanio);

        if (barraDanio != null)
            barraDanio.fillAmount = 0f;

        vidaRetrasadaNormalizada = vidaAnteriorNormalizada;
        corrutinaOcultarDanio = null;
    }

    private void LateUpdate()
    {
        OrientarHaciaCamara();
    }

    public void OrientarHaciaCamara()
    {
        if (camara == null)
            return;

        transform.LookAt(
            transform.position + camara.transform.rotation * Vector3.forward,
            camara.transform.rotation * Vector3.up);
    }
}
