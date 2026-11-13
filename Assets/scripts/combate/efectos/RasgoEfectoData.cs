

public class RasgoEfectoData 
{

public float numero;
public TipoStat tipoStat;
public TiempoRasgo tiempoRasgo;
public int duracion;
public int duracionTotal;
public TipoAnimacion tipoAnimacion;




public RasgoEfectoData(float numero, TipoStat tipoStat, TiempoRasgo tiempoRasgo, int duracion, TipoAnimacion tipoAnimacion)
    {
        
        this.numero=numero;
        this.tipoStat=tipoStat;
        this.tiempoRasgo=tiempoRasgo;
        this.duracion=duracion;
        this.duracionTotal=duracion;
        this.tipoAnimacion=tipoAnimacion;


    }


    




public void AnadirTiempoInicial(TiempoRasgo tiempoRasgo, int tiempoInicial)
    {
        
    }



public void AnadirCooldown(int cooldown)
    {
        
    }



}
