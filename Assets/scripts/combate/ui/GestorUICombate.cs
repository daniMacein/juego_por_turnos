using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;
using System;
public class GestorUICombate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static GestorUICombate gestorUICombate;
    public GameObject prefab;
    private void Awake()
    {
        gestorUICombate = this;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MostrarGolpe(ResultadoGolpe resultadoGolpe)
    {


        Color color = Color.yellow;
          float  numero = Mathf.Abs( resultadoGolpe.dañoFinal);
        string texto = numero.ToString("0");
        
         switch (resultadoGolpe.tipoAtaque)
        {
            case TipoAtaque.Daño:
            break;

            case TipoAtaque.Curacion:
             color=Color.green;
            break;

            case TipoAtaque.efecto:
            texto= "Aplicado";
            break;
        }

        switch (resultadoGolpe.estadoGolpe)
        {
            case EstadoGolpe.Normal:
                break;

            case EstadoGolpe.Critico:
            if (resultadoGolpe.tipoAtaque == TipoAtaque.Curacion)
                {
                    texto+=" Crítico";
                    color= Color.forestGreen;
                }
            else
                {
                    texto+=" Crítico";
                    color= Color.red;  
                }

                break;

            case EstadoGolpe.Evadido:
                 texto="Evadido";
                color= Color.grey;
                break;

            case EstadoGolpe.Fallado:
                 texto="Fallado";
                color= Color.grey;
                break;
        }

        

        Debug.Log(resultadoGolpe.objetivo.gameObject.name);
        MostrarNumGolpe(resultadoGolpe.objetivo.transform.position,texto, color );

    }

    public void MostrarNumGolpe(Vector3 posicion, string texto, Color color)
    {
        var ventana = Instantiate(prefab, posicion, Quaternion.identity);
        var temp = ventana.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        temp.text = texto;

        temp.faceColor = color;

        Destroy(ventana, 1f);
    }
}
