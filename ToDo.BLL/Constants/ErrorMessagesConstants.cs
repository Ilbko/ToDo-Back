namespace ToDo.BLL.Constants;

public static class ErrorMessagesConstants
{
    public static string NotFound()
    {
        return "Not Found";
    }

    public static string NotFound(object? id, Type entityType)
    {
        if (entityType == null)
        {
            throw new ArgumentNullException(nameof(entityType));
        }

        return $"Entity {entityType.Name} with id '{id}' was not found.";
    }

    public static string PropertyMustHaveAMaximumLengthOfNCharacters(string property, int length)
    {
        return $"{property} field must have a maximum length of {length} characters.";
    }

    public static string PropertyIsRequired(string property)
    {
        return $"{property} is required.";
    }

    public static string PropertyMustBeValidEnum(string property)
    {
        return $"{property} must be a valid value.";
    }
}
