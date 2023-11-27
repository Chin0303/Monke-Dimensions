using Monke_Dimensions.Behaviours;
using UnityEngine;

namespace Monke_Dimensions.Interaction
{
    internal class LoadButton : GorillaPressableButton
    {
        public override void Start()
        {
            BoxCollider boxCollider = GetComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            gameObject.layer = 18;
            WardrobeItemButton wardrobeItemButton = FindObjectOfType<WardrobeItemButton>();

            buttonRenderer = GetComponent<MeshRenderer>();
            pressedMaterial = wardrobeItemButton.pressedMaterial;
            unpressedMaterial = wardrobeItemButton.unpressedMaterial;

            onPressButton = new UnityEngine.Events.UnityEvent();
            onPressButton.AddListener(new UnityEngine.Events.UnityAction(ButtonActivation));
        }

        public void ButtonActivation() => DimensionManager.Instance.LoadSelectedDimension(DimensionManager.Instance.currentSelectedDimensionName);
    }
}
