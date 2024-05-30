using UnityEngine;
namespace Monke_Dimensions.Models;
public class DimensionDescriptor : MonoBehaviour
{
    public string Name;
    public string Author;
    public string Description;

    public GameObject SpawnPosition;
    public GameObject TerminalPosition;

    public Texture2D Photo;
}