public enum TipoAtaque
{
    Daño,
    Curacion,
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
    Bloqueado
}