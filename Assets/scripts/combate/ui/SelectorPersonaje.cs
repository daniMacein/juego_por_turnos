using UnityEngine;
using System.Collections.Generic;

public class SelectorPersonaje : MonoBehaviour
{
    private Camera camara; // asigna la cámara en el inspector

    //*** Selección simple (sin SelectorData): un solo objetivo
    public Personaje seleccionado;
    public bool haySeleccion = false;

    //*** Selección guiada por SelectorData
    private SelectorData datos;
    public List<Personaje> objetivos = new List<Personaje>();

    //true cuando se han acumulado todos los objetivos que pedía el SelectorData
    public bool seleccionCompleta
    {
        get { return datos != null && objetivos.Count >= datos.numeroObjetivos; }
    }

    void Start()
    {
        camara = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Seleccionar();
        }
    }

    //Empieza un modo de selección: solo se suman los personajes
    //cuyo equipo esté en datos.equipos, hasta numeroObjetivos
    public void EmpezarSeleccion(SelectorData datos)
    {
        Reset();
        this.datos = datos;
    }

    void Seleccionar()
    {
        Ray rayo = camara.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(rayo, out RaycastHit hit))
        {
            Personaje p = hit.collider.GetComponent<Personaje>();

            if (p != null)
            {
                Registrar(p);
            }
        }
    }

    void Registrar(Personaje p)
    {
        //modo guiado: los clics de equipos no válidos no suman
        if (datos != null)
        {
            if (!datos.equipos.Contains(p.equipo))
            {
                return;
            }

            //el mismo personaje no cuenta dos veces
            if (objetivos.Contains(p))
            {
                return;
            }

            objetivos.Add(p);
            Debug.Log("Objetivo " + objetivos.Count + "/" + datos.numeroObjetivos + ": " + p.nombre);
            return;
        }

        //modo simple (de prueba)
        seleccionado = p;
        haySeleccion = true;
    }

    public void Reset()
    {
        seleccionado = null;
        haySeleccion = false;
        datos = null;
        objetivos.Clear();
    }
}
