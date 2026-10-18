using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectorPersonaje : MonoBehaviour
{
    [SerializeField] private Camera camara;

    // Selección simple (sin SelectorData): un solo objetivo.
    public Personaje seleccionado;
    public bool haySeleccion = false;

    // Selección guiada por SelectorData.
    private SelectorData datos;
    public List<Personaje> objetivos = new List<Personaje>();

    private readonly List<MallaPersonaje> mallas = new List<MallaPersonaje>();
    private readonly Dictionary<Personaje, List<ParteResaltada>> partesResaltadas = new Dictionary<Personaje, List<ParteResaltada>>();
    // MaterialPropertyBlock usa código nativo: hay que crearlo en Awake, no aquí.
    private MaterialPropertyBlock bloquePropiedades;
    // Amarillo HDR para que el efecto destaque claramente incluso en modelos oscuros.
    private readonly Color colorResaltado = new Color(2f, 1.5f, 0.05f, 1f);
    private const float intensidadResaltado = 1f;
    private Personaje personajeResaltado;

    // True cuando se han acumulado todos los objetivos que pedía el SelectorData.
    public bool seleccionCompleta
    {
        get { return datos != null && objetivos.Count >= datos.numeroObjetivos; }
    }

    private sealed class MallaPersonaje
    {
        public Personaje personaje;
        public Renderer renderer;
        public MeshCollider collider;
        public SkinnedMeshRenderer mallaAnimada;
        public Mesh geometriaAnimada;

        public bool Preparar()
        {
            if (personaje == null || renderer == null || !renderer.enabled ||
                !renderer.gameObject.activeInHierarchy || collider == null)
            {
                return false;
            }

            if (mallaAnimada != null)
            {
                // BakeMesh conserva la pose que está mostrando el personaje este frame.
                mallaAnimada.BakeMesh(geometriaAnimada);

                // Forzar a MeshCollider a recocinar la malla ya actualizada.
                collider.sharedMesh = null;
                collider.sharedMesh = geometriaAnimada;
            }

            return true;
        }
    }

    private sealed class ParteResaltada
    {
        public Renderer renderer;
        public string propiedadColor;
        public Color colorOriginal;
    }

    private void Awake()
    {
        bloquePropiedades = new MaterialPropertyBlock();

        if (camara == null)
        {
            camara = Camera.main;
        }

        CrearMallasSeleccionables();
    }

    private void LateUpdate()
    {
        if (camara == null)
        {
            camara = Camera.main;
            if (camara == null)
            {
                ActualizarResaltado(null);
                return;
            }
        }

        // Evita seleccionar un personaje a través de un botón o panel de la interfaz.
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            ActualizarResaltado(null);
            return;
        }

        Ray rayo = camara.ScreenPointToRay(Input.mousePosition);
        Personaje bajoElCursor = ObtenerPersonajeMasCercano(rayo);
        Personaje objetivoResaltado = PuedeSeleccionar(bajoElCursor) ? bajoElCursor : null;
        ActualizarResaltado(objetivoResaltado);

        if (Input.GetMouseButtonDown(0) && PuedeSeleccionar(bajoElCursor))
        {
            Registrar(bajoElCursor);
        }
    }

    // Empieza un modo de selección: solo se suman personajes del equipo permitido.
    public void EmpezarSeleccion(SelectorData datos)
    {
        Reset();
        this.datos = datos;
    }

    private void CrearMallasSeleccionables()
    {
        LimpiarMallasSeleccionables();
        partesResaltadas.Clear();

        Personaje[] personajes = FindObjectsByType<Personaje>(FindObjectsSortMode.None);
        foreach (Personaje personaje in personajes)
        {
            RegistrarPartesResaltables(personaje);

            foreach (SkinnedMeshRenderer renderizador in personaje.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                AgregarMalla(personaje, renderizador, renderizador.sharedMesh, renderizador);
            }

            foreach (MeshRenderer renderizador in personaje.GetComponentsInChildren<MeshRenderer>(true))
            {
                MeshFilter filtro = renderizador.GetComponent<MeshFilter>();
                if (filtro != null)
                {
                    AgregarMalla(personaje, renderizador, filtro.sharedMesh, null);
                }
            }
        }
    }

    private void RegistrarPartesResaltables(Personaje personaje)
    {
        List<ParteResaltada> partes = new List<ParteResaltada>();
        Renderer[] renderizadores = personaje.GetComponentsInChildren<Renderer>(true);

        foreach (Renderer renderizador in renderizadores)
        {
            if (!(renderizador is MeshRenderer) && !(renderizador is SkinnedMeshRenderer))
            {
                continue;
            }

            Material material = renderizador.sharedMaterial;
            if (material == null)
            {
                continue;
            }

            string propiedadColor = null;
            if (material.HasProperty("_BaseColor"))
            {
                propiedadColor = "_BaseColor";
            }
            else if (material.HasProperty("_Color"))
            {
                propiedadColor = "_Color";
            }

            if (propiedadColor == null)
            {
                continue;
            }

            partes.Add(new ParteResaltada
            {
                renderer = renderizador,
                propiedadColor = propiedadColor,
                colorOriginal = material.color
            });
        }

        if (partes.Count > 0)
        {
            partesResaltadas[personaje] = partes;
        }
    }

    private void AgregarMalla(Personaje personaje, Renderer renderizador, Mesh malla, SkinnedMeshRenderer mallaAnimada)
    {
        if (malla == null)
        {
            return;
        }

        GameObject proxy = new GameObject("MallaSeleccion_" + renderizador.name);
        proxy.hideFlags = HideFlags.HideAndDontSave;
        proxy.transform.SetParent(renderizador.transform, false);

        MeshCollider meshCollider = proxy.AddComponent<MeshCollider>();
        meshCollider.convex = false;
        meshCollider.isTrigger = true;

        MallaPersonaje seleccionable = new MallaPersonaje
        {
            personaje = personaje,
            renderer = renderizador,
            collider = meshCollider,
            mallaAnimada = mallaAnimada
        };

        if (mallaAnimada != null)
        {
            seleccionable.geometriaAnimada = new Mesh
            {
                name = "GeometriaSeleccion_" + renderizador.name,
                hideFlags = HideFlags.HideAndDontSave
            };
            seleccionable.geometriaAnimada.MarkDynamic();
            mallaAnimada.BakeMesh(seleccionable.geometriaAnimada);
            meshCollider.sharedMesh = seleccionable.geometriaAnimada;
        }
        else
        {
            meshCollider.sharedMesh = malla;
        }

        mallas.Add(seleccionable);
    }

    private Personaje ObtenerPersonajeMasCercano(Ray rayo)
    {
        Personaje masCercano = null;
        float distanciaMasCercana = float.PositiveInfinity;

        foreach (MallaPersonaje malla in mallas)
        {
            if (!malla.Preparar())
            {
                continue;
            }

            if (malla.collider.Raycast(rayo, out RaycastHit hit, distanciaMasCercana))
            {
                distanciaMasCercana = hit.distance;
                masCercano = malla.personaje;
            }
        }

        return masCercano;
    }

    private bool PuedeSeleccionar(Personaje personaje)
    {
        return personaje != null && (datos == null || datos.equipos.Contains(personaje.equipo));
    }

    private void ActualizarResaltado(Personaje nuevoPersonaje)
    {
        if (personajeResaltado == nuevoPersonaje)
        {
            return;
        }

        ResaltarPersonaje(personajeResaltado, false);
        personajeResaltado = nuevoPersonaje;
        ResaltarPersonaje(personajeResaltado, true);
    }

    private void ResaltarPersonaje(Personaje personaje, bool activo)
    {
        if (personaje == null || !partesResaltadas.TryGetValue(personaje, out List<ParteResaltada> partes))
        {
            return;
        }

        foreach (ParteResaltada parte in partes)
        {
            if (parte.renderer == null)
            {
                continue;
            }

            Color color = parte.colorOriginal;
            if (activo)
            {
                color = Color.Lerp(color, colorResaltado, intensidadResaltado);
            }

            parte.renderer.GetPropertyBlock(bloquePropiedades);
            bloquePropiedades.SetColor(parte.propiedadColor, color);
            parte.renderer.SetPropertyBlock(bloquePropiedades);
        }
    }

    private void Registrar(Personaje personaje)
    {
        // Modo guiado: los clics de equipos no válidos no suman.
        if (datos != null)
        {
            if (seleccionCompleta)
            {
                return;
            }

            objetivos.Add(personaje);
            Debug.Log("Objetivo " + objetivos.Count + "/" + datos.numeroObjetivos + ": " + personaje.nombre);
            return;
        }

        // Modo simple (de prueba).
        seleccionado = personaje;
        haySeleccion = true;
    }

    public void Reset()
    {
        seleccionado = null;
        haySeleccion = false;
        datos = null;
        objetivos.Clear();
    }

    private void OnDestroy()
    {
        ActualizarResaltado(null);
        LimpiarMallasSeleccionables();
    }

    private void LimpiarMallasSeleccionables()
    {
        foreach (MallaPersonaje malla in mallas)
        {
            if (malla.geometriaAnimada != null)
            {
                DestruirObjeto(malla.geometriaAnimada);
            }

            if (malla.collider != null)
            {
                DestruirObjeto(malla.collider.gameObject);
            }
        }

        mallas.Clear();
    }

    private void DestruirObjeto(Object objeto)
    {
        if (Application.isPlaying)
        {
            Destroy(objeto);
        }
        else
        {
            DestroyImmediate(objeto);
        }
    }
}
