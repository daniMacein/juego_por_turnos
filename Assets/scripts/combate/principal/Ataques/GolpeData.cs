using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering;
public class GolpeData 
{


    //**objetivos
    public List<Personaje> objetivos;

    //**Estadisticas
    public float vida;
    public float armadura;

    public bool esCritico=false;

    public float penetracionArmadura=1;

    //** Definir ataque
    public TipoAtaque tipoAtaque;
    public TipoObjetivo tipoObjetivo;
    public EstadoGolpe estadoGolpe= EstadoGolpe.Normal;

    public TipoAnimacion tipoAnimacion;


    //**características
    public bool aplicaEfecto = false;

    public bool esPositivo=true;

    //**Efectos
    public List<Efecto> efectos;

    //** Métodos
    //metodo que se llama para aplicar la vida que va a curar
   


public GolpeData(float valor, List<Personaje> objetivos, TipoAtaque tipoAtaque,TipoObjetivo tipoObjetivo,TipoAnimacion tipoAnimacion)
{
    this.tipoAtaque = tipoAtaque;
    this.objetivos = objetivos;
    this.tipoObjetivo=tipoObjetivo;
    this.tipoAnimacion=tipoAnimacion;

    switch (tipoAtaque)
    {
        case TipoAtaque.Daño:
            esPositivo = false;
            vida = -valor;
            armadura=valor;
            break;


        case TipoAtaque.Curacion:
           esPositivo = true;
            vida = valor;
            armadura=0;
             
            break;

        default:
            vida = 0;
            Debug.Log("ERROR TIPO NO PUESTO EN EL SWTICH");
            break;
    }
}}
