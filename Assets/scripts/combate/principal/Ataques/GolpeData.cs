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

    public bool ignoraArmadura=false;

    //** Definir ataque
    public TipoAtaque tipoAtaque;
    public TipoObjetivo tipoObjetivo;
    public EstadoGolpe estadoGolpe= EstadoGolpe.Previo;




    //**características
    public bool aplicaEfecto = false;

    public bool esPositivo=true;

    //**Efectos
    public List<Efecto> efectos;

    //** Métodos
    //metodo que se llama para aplicar la vida que va a curar
   


public GolpeData(float valor, List<Personaje> objetivos, TipoAtaque tipoAtaque,TipoObjetivo tipoObjetivo)
{
    this.tipoAtaque = tipoAtaque;
    this.objetivos = objetivos;
    this.tipoObjetivo=tipoObjetivo;

    switch (tipoAtaque)
    {
        case TipoAtaque.Daño:
        case TipoAtaque.DañoCritico:
        case TipoAtaque.Aturdimiento:
        case TipoAtaque.Mental:
            esPositivo = false;
            vida = -valor;
            armadura=valor;
            break;


        case TipoAtaque.Curacion:
        case TipoAtaque.curacionCritico:
        case TipoAtaque.Escudo:
        case TipoAtaque.Especial:
           esPositivo = true;
            vida = --valor;
            armadura=0;
             
            break;

        default:
            vida = 0;
            Debug.Log("ERROR TIPO NO PUESTO EN EL SWTICH");
            break;
    }
}}
