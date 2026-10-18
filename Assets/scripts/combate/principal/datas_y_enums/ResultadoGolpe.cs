using Unity.VisualScripting;

public class ResultadoGolpe
{

    public Personaje objetivo;
    public float dañoFinal;
    public float armaduraReducida;

    public EstadoGolpe estadoGolpe;

    public TipoObjetivo tipoObjetivo;
    public TipoAtaque tipoAtaque;

    public ResultadoGolpe( Personaje objetivo,float Dañofinal,float armaduraReducida,EstadoGolpe estadoGolpe,
     TipoObjetivo tipoObjetivo,TipoAtaque tipoAtaque)
    {
        this.objetivo= objetivo;
        this.dañoFinal=Dañofinal;
        this.armaduraReducida=armaduraReducida;
        this.estadoGolpe=estadoGolpe;
        this.tipoObjetivo=tipoObjetivo;
        this.tipoAtaque=tipoAtaque;

    }

    
}