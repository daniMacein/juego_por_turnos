using UnityEngine;

public class SelectorPersonaje : MonoBehaviour
{
    private Camera camara; // asigna la cámara en el inspector
    public Personaje seleccionado;
    public bool haySeleccion = false;

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

    void Seleccionar()
    {
        Ray rayo = camara.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(rayo, out RaycastHit hit))
        {
            Personaje p = hit.collider.GetComponent<Personaje>();

            if (p != null)
            {
                seleccionado = p;
                haySeleccion = true;
            }
        }
    }

    public void Reset()
    {
        seleccionado = null;
        haySeleccion = false;
    }
}