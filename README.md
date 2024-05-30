# Monke Dimensions

## Installation
First, download the [latest build](https://github.com/Chin0303/Monke-Dimensions/releases/latest) and extract/unzip it.\
 Then navigate to your Plugins folder and drag it into there. If you play on Steam its most likely located at ```C:\Program Files (x86)\Steam\steamapps\common\Gorilla Tag\BepInEx\plugins``` and if you play on Oculus its most likely located at ```C:\Program Files\Oculus\Software\Software\another-axiom-gorilla-tag\BepInEx\plugins```. 

Make sure that both the `Monke Dimensions.dll` and `Dimensions` folder are both in `(GTAG ROOT)\BepInEx\plugins\Monke.Dimensions\`. If you need to, you can create the `Monke.Dimensions` folder and move the files into that.\
If all went according to plan, the mod should now be setup and ready to use!

## Installing Dimensions
In order to add Dimensions, you'll first want to download the maps you want. The easiest way to find maps is to go into the official [Monke Dimensions Discord Server](https://discord.gg/chin-s-server-1041450240135413890) and go into the [dimensions-download](https://discord.com/channels/1041450240135413890/1178592176997941290) channel.

## Making Maps
when i make vidoe abtu it

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
### Dimension Enter Notes:
- To know if a specific dimension is loaded, you can use the string parameter to check what dimension is loaded.

- The string parameter is formatted as follows: "DimensionName, DimensionAuthor". Not sure what the name or author is of a dimension? You can look at the dimension terminal and see it there, or you can log it as follows: Debug.Log(dimension);

- Use FindObjectInDimension to find an object in the currently loaded dimension.

### Trigger Events
```csharp
DimensionEvents.OnDimensionTriggerEvent += 
(triggerevent, triggerGameobject, triggeredGameObject, ison) =>
{
    if(triggerevent == TriggerEvent.ToggleActiveState)
    {
        /* Get the renderer component of the trigger object */
        var matRenderer = triggerGameobject.GetComponent<Renderer>();
        
        /* If the "isOn" boolean is true, then apply A green color onto the Trigger Object (NOT the object that is getting triggered by the trigger object), if not apply A red color onto it. */
        matRenderer.material.color = ison ? Color.green : Color.red;
    }
};
```
### Trigger Event Notes:
- If you want to check for a Teleport trigger event, just do ```if(triggerevent == TriggerEvent.Teleport)```. The "isOn" boolean will alway be false, due to the Teleport trigger event not being A toggleable event.

## Legal Disclaimer
This modification falls under the [GNU General Public License v3.0](https://www.gnu.org/licenses/gpl-3.0.en.html).\
This product is not affiliated with Gorilla Tag or Another Axiom LLC and is not endorsed or otherwise sponsored by Another Axiom LLC. Portions of the materials contained herein are property of Another Axiom LLC. © 2021 Another Axiom LLC.