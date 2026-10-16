using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using JetBrains.Annotations;
using UnityEngine.UI;

public class botonInfoAtaque : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI   textoinfo;

    private TextMeshProUGUI textoBoton;

    private string descripcion;
    private int id;

   

  

    private Button boton;
    private Image imagenBoton;

    private Color colorOriginal;

    void Awake()
    {
        boton = GetComponent<Button>();
        textoBoton = GetComponentInChildren<TextMeshProUGUI>();

            imagenBoton = GetComponent<Image>();

            colorOriginal = imagenBoton.color;
        
        }

    public void MostrarPersonaje(Personaje p, int num)
    {
        
        //textoBoton.text= p.Ataque1Nombre;
        

            textoBoton.text = p.ataquesInfo[num].nombre;
            
            descripcion= p.ataquesInfo[num].descripcion;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Ratón encima del botón");
        // Acción al pasar el ratón

         textoinfo.text=descripcion;
        textoinfo.gameObject.SetActive(true); //se muestra la informacion del ataque
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Ratón fuera del botón");
        // Acción al salir
        textoinfo.gameObject.SetActive(false); //se vuelve a ocultar la información del ataque 
    }

    public void RestaurarColor()
    {
        imagenBoton.color=colorOriginal;
    }

    public void ColorPulsar()
    {
         imagenBoton.color=Color.brown;
    }
}


