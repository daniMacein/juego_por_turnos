public enum TipoAtaque
{
    Daño,
    DañoCritico,
    Curacion,
    curacionCritico,
    Aturdimiento,
    Mental,
    Escudo,
    Especial
}

public enum TipoObjetivo
{
    Unitario,
    AreaTodos,
    AreaPrincipalesAliados,
    AreaPrincipalesEnemigos,
    AreaTodosAliados,
    AreaTodosEnemigos
}

public enum EstadoGolpe
{
    Previo,
    Atacando,
    Golpeado,
    Fallado,
    Persuadido,
    Critico
}


public enum TipoAnimacion
{
    Ataque1_Golpe1,
    Ataque1_Golpe2,
    AtaqueFuerte,
    Magia_Fuego
}