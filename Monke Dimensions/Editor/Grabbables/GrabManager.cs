#if !EDITOR
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Monke_Dimensions.Behaviours;
using Monke_Dimensions.API;

namespace Monke_Dimensions.Editor.Grabbables;

public class GrabManager : MonoBehaviour
{
    private GameObject callbacksObject;
    private void Start()
    {
        callbacksObject = new GameObject("MonkeDimensionsCallbacks");
        Callbacks callbacks = callbacksObject.AddComponent<Callbacks>();
        callbacks.FindGrabbables();
        callbacks.FindTriggerObjects();
        DimensionEvents.OnDimensionLeave += DimensionLeave;
    }

    private void OnDestroy()
    {
        DimensionEvents.OnDimensionLeave -= DimensionLeave;
    }

    private void DimensionLeave(string s)
    {
        Destroy(callbacksObject);
    }


    public static void RaiseTheEvent(string objectId, bool left, bool grabbing, int actorNumber, Vector3 postion, Vector3 rotation)
    {
        if (!PhotonNetwork.InRoom)
            return;
        object[] content = new object[]
        {
            EventCodes.Grab,
            objectId,
            left,
            grabbing,
            actorNumber,
            postion,
            rotation
        };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        PhotonNetwork.RaiseEvent((byte)EventCodes.EventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public static void SetNetworkedGrabbable(VRRig rig, bool isLeft, Transform grabbableObject, Vector3 postion, Vector3 rotation)
    {
        grabbableObject.position = postion;
        Vector3 ihatedecalfreetheyresostupid = grabbableObject.rotation.eulerAngles;
        ihatedecalfreetheyresostupid = rotation;

        if (isLeft)
            grabbableObject.SetParent(rig.leftHandTransform.parent);
        else
            grabbableObject.SetParent(rig.rightHandTransform.parent);
    }

    public static VRRig FindVRRigForPlayer(Player player)
    {
        return GorillaGameManager.StaticFindRigForPlayer(player);
    }
}
#endif