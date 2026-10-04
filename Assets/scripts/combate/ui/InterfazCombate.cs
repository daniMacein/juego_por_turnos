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


    void Awake()
    {
        interfazCombate=this;
    }

    void Start()
    {
        menuCombate = transform.GetChild(0).gameObject;
        menuCombate.SetActive(false);
    }
    public void MostrarMenuPersonaje(Personaje p)
        {
            
            if (p == null)
            {
                Debug.LogError("ERROR: Se llamó a MostrarMenuPersonaje con p = NULL");
                return;
            }

             menuCombate.SetActive(true);
            nombre.text=p.nombre;

            textoBoton1.MostrarPersonaje(p,0);
            textoBoton2.MostrarPersonaje(p,1);
            textoBoton3.MostrarPersonaje(p,2);
            textoBoton4.MostrarPersonaje(p,3);
            personaje=p;

            
        }

    private Personaje personaje;

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
        //personaje.Ataque2(); Se ponrá el ataque 2 aquí cuando la función este creada 
        Debug.Log("Botón 2 pulsado, pero Ataque2 no implementado aún");
    }

    public void AlpulsarBoton3()
    {
        if (personaje == null)
        {
            Debug.LogError("Se pulsó el botón sin personaje seleccionado");
            return;
        }
        //personaje.Ataque3(); Se ponrá el ataque 3 aquí cuando la función este creada 
        Debug.Log("Botón 3 pulsado, pero Ataque3 no implementado aún");
    }

    public void AlpulsarBoton4()
    {
        if (personaje == null)
        {
            Debug.LogError("Se pulsó el botón sin personaje seleccionado");
            return;
        }
        //personaje.Ataque4(); Se ponrá el ataque 4 aquí cuando la función este creada 
        Debug.Log("Botón 4 pulsado, pero Ataque4 no implementado aún");
    }
}
