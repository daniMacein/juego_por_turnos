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