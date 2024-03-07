# Monke Dimensions

## For Developers

Monke Dimensions allows mod creators to make their own mods for maps.

```csharp
DimensionEvents.OnDimensionEnter += (dimension) =>
{
    if (dimension == "Demo Map, Chin")
    {
        GameObject objectToFind = FindObjectInDimension("object");
        /* Do stuff */
    }
};

DimensionEvents.OnDimensionLeave += (dimension) =>
{
    if (dimension == "Demo Map, Chin")
    {
        /* Undo stuff */
    }
};

```
### Notes:
- To know if a specific dimension is loaded, you can use the string parameter to check what dimension is loaded.

- The string parameter is formatted as follows: "DimensionName, DimensionAuthor". Not sure what the name or author is of a dimension? You can look at the dimension terminal and see it there, or you can log it as follows: Debug.Log(dimension);

- Use FindObjectInDimension to find an object in the currently loaded dimension.