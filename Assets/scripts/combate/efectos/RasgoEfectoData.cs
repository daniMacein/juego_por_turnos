using System.Collections.Generic;
public class RasgoEfectoData
{

    public float numero;
    public TipoStat tipoStat;
    public TiempoRasgo tiempoRasgo;
    public int duracion;
    public int duracionTotal;
    public TipoAnimacion tipoAnimacion;


    public List<float> danoStack = new List<float>();

    public List<int> duracionStack = new List<int>();






    public RasgoEfectoData(float numero, TipoStat tipoStat, TiempoRasgo tiempoRasgo, int duracion, TipoAnimacion tipoAnimacion)
    {

        if (tipoStat == TipoStat.Vida)
        {
            danoStack.Add(numero);
            duracionStack.Add(duracion);
        }

        else
        {
            this.numero = numero;
            this.duracionTotal = duracion;
        }

        this.tipoStat = tipoStat;
        this.tiempoRasgo = tiempoRasgo;
        this.duracion = duracion;

        this.tipoAnimacion = tipoAnimacion;


    }







    public void AnadirTiempoInicial(TiempoRasgo tiempoRasgo, int tiempoInicial)
    {

    }



    public void AnadirCooldown(int cooldown)
    {

    }



}
