public class ResultadoGolpe
{
    public float dañoFinal;
    public float armaduraReducida;

    public EstadoGolpe estadoGolpe;

    public TipoObjetivo tipoObjetivo;
    public TipoAtaque tipoAtaque;

    public ResultadoGolpe(float Dañofinal,float armaduraReducida,EstadoGolpe estadoGolpe,
     TipoObjetivo tipoObjetivo,TipoAtaque tipoAtaque)
    {
        this.dañoFinal=Dañofinal;
        this.armaduraReducida=armaduraReducida;
        this.estadoGolpe=estadoGolpe;
        this.tipoObjetivo=tipoObjetivo;
        this.tipoAtaque=tipoAtaque;

    }

    
}