#if !EDITOR
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Monke_Dimensions.Editor.Grabbables;
using System.Collections.Generic;
using UnityEngine;
using Monke_Dimensions.Editor;

namespace Monke_Dimensions.Behaviours;

public class Callbacks : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private Dictionary<string, GrabbableObject> grabbablesDict = new Dictionary<string, GrabbableObject>();
    private Dictionary<string, ToggleActiveState> activeStateObjects = new Dictionary<string, ToggleActiveState>();

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData eventData)
    {
        byte eventCode = eventData.Code;
        if (eventData.CustomData == null || eventCode != (byte)EventCodes.EventCode)
        {
            return;
        }
        object[] data = (object[])eventData.CustomData;

        if ((int)data[0] == (int)EventCodes.Grab) 
        {
            GrabbableNetwork(data);
        }
        else
        {
            NetworkTrigger(data);
        }
    }

    private void NetworkTrigger(object[] data)
    {
        string triggerID = (string)data[1];
        bool isOn = (bool)data[2];

        ToggleActiveState toggleActiveState = activeStateObjects[triggerID];
        toggleActiveState.ObjectToToggle.SetActive(isOn);
    }
    
    private void GrabbableNetwork(object[] data)
    {
        string grabID = (string)data[1];
        bool isLeft = (bool)data[2];
        bool isGrab = (bool)data[3];
        int actorNumber = (int)data[4];
        Vector3 position = (Vector3)data[5];
        Vector3 rotation = (Vector3)data[6];


        GrabbableObject grabbableObject = grabbablesDict[grabID];

        if (!isGrab)
        {
            grabbableObject.transform.SetParent(grabbableObject.grabParent.transform);
            grabbableObject.transform.position = position;
            Vector3 eulerAnglesObject = grabbableObject.transform.rotation.eulerAngles;
            eulerAnglesObject = rotation;
            return;
        }

        VRRig rig = GrabManager.FindVRRigForPlayer(GetPlayerByActorNumber(actorNumber));

        GrabManager.SetNetworkedGrabbable(rig, isLeft, grabbableObject.transform, position, rotation);
    }

    public void FindGrabbables()
    {
        GrabbableObject[] _grabbables = FindObjectsOfType<GrabbableObject>();
        foreach (var obj in _grabbables)
        {
            grabbablesDict[obj.networkID] = obj;
        }
    }

    public void FindTriggerObjects()
    {
        ToggleActiveState[] triggerObjects = FindObjectsOfType<ToggleActiveState>();
        foreach (var obj in triggerObjects)
        {
            activeStateObjects[obj.networkID] = obj;
        }
    }

    public Player GetPlayerByActorNumber(int actorNumber)
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.ActorNumber == actorNumber)
            {
                return player;
            }
        }
        return null;
    }
}
public enum EventCodes
{
    EventCode = 25,
    ToggleActiveState = 26,
    Grab = 27
}
#endif