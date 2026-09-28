using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering;
public class EfectoData
{


    //**objetivos
    public List<Personaje> objetivos;

    //**Estadisticas
    public float vida;
    public float armadura;

    public bool esCritico=false;

    public float penetracionArmadura=1;

    //** Definir ataque
    public TipoEfecto tipoEfecto;
    public TipoObjetivo tipoObjetivo;

    public TipoAtaque tipoAtaque=TipoAtaque.efecto;
    public EstadoGolpe estadoGolpe= EstadoGolpe.Normal;

    public TipoAnimacion tipoAnimacion;


    //**características
    public bool aplicaEfecto = false;

    public bool esPositivo=true;

    //**Efectos
    public List<Efecto> efectos;

    //** Métodos
    //metodo que se llama para aplicar la vida que va a curar
   


public EfectoData(float valor, List<Personaje> objetivos, TipoEfecto tipoEfecto,TipoAnimacion tipoAnimacion)
{
    this.tipoEfecto = tipoEfecto;
    this.objetivos = objetivos;
    this.tipoAnimacion=tipoAnimacion;

    switch (tipoEfecto)
    {
        case TipoEfecto.Daño:
            esPositivo = false;
            vida = -valor;
            armadura=valor;
            break;


        case TipoEfecto.Curacion:
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
