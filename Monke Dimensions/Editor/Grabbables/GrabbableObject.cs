#if !EDITOR
using Photon.Pun;
using GorillaLocomotion;
#endif
using UnityEngine;
// shout out to @dev9998
namespace Monke_Dimensions.Editor.Grabbables;

public class GrabbableObject :
#if !EDITOR
HoldableObject
#else
MonoBehaviour
#endif
{
    [HideInInspector]
    public GameObject grabParent;

    public bool networked;
    public string networkID;
#if !EDITOR
    public bool
        InHand = false,
        InLeftHand = false,
        PickUp = true,
        DidSwap = false,
        SwappedLeft = true;

    public float
        GrabDistance = 0.15f,
        ThrowForce = 1.75f;

    private void Start()
    {
        grabParent = gameObject.transform.parent.gameObject;
    }
    public void OnGrab(bool isLeft)
    {
        if (!networked) return;
        GrabManager.RaiseTheEvent(networkID, isLeft, true, PhotonNetwork.LocalPlayer.ActorNumber, transform.position, transform.rotation.eulerAngles);
    }

    public void OnDrop(bool isLeft)
    {
        if(!networked) return;
        GrabManager.RaiseTheEvent(networkID, isLeft, false, PhotonNetwork.LocalPlayer.ActorNumber, transform.position, transform.rotation.eulerAngles);
    }

    public void Update()
    {
        bool leftGrip = ControllerInputPoller.instance.leftGrab;
        bool rightGrip = ControllerInputPoller.instance.rightGrab;

        var Distance = GrabDistance * Player.Instance.scale;
        if (DidSwap && (!SwappedLeft ? !leftGrip : !rightGrip))
            DidSwap = false;

        bool pickLeft = PickUp && leftGrip && Vector3.Distance(Player.Instance.leftControllerTransform.position, transform.position) < Distance && !InHand && EquipmentInteractor.instance.leftHandHeldEquipment == null && !DidSwap;
        bool swapLeft = InHand && leftGrip && rightGrip && !DidSwap && (Vector3.Distance(Player.Instance.leftControllerTransform.position, transform.position) < Distance) && !SwappedLeft && EquipmentInteractor.instance.leftHandHeldEquipment == null;
        if (pickLeft || swapLeft)
        {
            DidSwap = swapLeft;
            SwappedLeft = true;
            InLeftHand = true;
            InHand = true;

            transform.SetParent(GorillaTagger.Instance.offlineVRRig.leftHandTransform.parent);
            GorillaTagger.Instance.StartVibration(true, 0.1f, 0.05f);
            EquipmentInteractor.instance.leftHandHeldEquipment = this;
            if (DidSwap) EquipmentInteractor.instance.rightHandHeldEquipment = null;

            OnGrab(true);
        }
        else if (!leftGrip && InHand && InLeftHand)
        {
            InLeftHand = true;
            InHand = false;
            transform.SetParent(grabParent.transform);

            EquipmentInteractor.instance.leftHandHeldEquipment = null;
            OnDrop(true);
        }

        bool pickRight = PickUp && rightGrip && Vector3.Distance(Player.Instance.rightControllerTransform.position, transform.position) < Distance && !InHand && EquipmentInteractor.instance.rightHandHeldEquipment == null && !DidSwap;
        bool swapRight = InHand && leftGrip && rightGrip && !DidSwap && (Vector3.Distance(Player.Instance.rightControllerTransform.position, transform.position) < Distance) && SwappedLeft && EquipmentInteractor.instance.rightHandHeldEquipment == null;
        if (pickRight || swapRight)
        {
            DidSwap = swapRight;
            SwappedLeft = false;

            InLeftHand = false;
            InHand = true;
            transform.SetParent(GorillaTagger.Instance.offlineVRRig.rightHandTransform.parent);

            GorillaTagger.Instance.StartVibration(false, 0.1f, 0.05f);
            EquipmentInteractor.instance.rightHandHeldEquipment = this;
            if (DidSwap) EquipmentInteractor.instance.leftHandHeldEquipment = null;

            OnGrab(false);
        }
        else if (!rightGrip && InHand && !InLeftHand)
        {
            InLeftHand = false;
            InHand = false;
            transform.SetParent(null);

            EquipmentInteractor.instance.rightHandHeldEquipment = null;
            OnDrop(false);
        }
    }
#endif
}