namespace ApiInventory.Application.Constants;

public static class ErrorMessages
{
    public static string ProductNotFound(int id)
        => $"No se encontró el producto {id}.";

    public static string CategoryNotFound(int id)
        => $"No se encontró la categoría {id}.";

    public const string CategoryNotFoundForProduct =
        "La categoría especificada no existe.";

    public const string CategoryHasActiveProducts =
        "No se puede eliminar una categoría con productos asociados activos.";

    public static string CategoryAlreadyExists(string name)
       => $"Ya existe una categoría con el nombre {name}.";
}