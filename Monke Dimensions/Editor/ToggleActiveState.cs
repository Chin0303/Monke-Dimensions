using UnityEngine;
using Monke_Dimensions.Helpers;
using ExitGames.Client.Photon;
using Monke_Dimensions.Behaviours;
using Photon.Pun;

using Photon.Realtime;


#if !EDITOR
using Monke_Dimensions.API;
#endif

namespace Monke_Dimensions.Editor;

public class ToggleActiveState : MonkeTriggerObject
{
    public GameObject ObjectToToggle;
    private bool isOn;

    public bool networked;
    public string networkID;

    private void Awake() =>
        isOn = gameObject.activeSelf;

#if !EDITOR

    public override void MonkeTrigger(Collider collider)
    {
        base.MonkeTrigger(collider);
        isOn = !isOn;

        ObjectToToggle.SetActive(isOn);

        if (networked)
            NetworkStuff();

        DimensionEvents.OnDimensionTriggerEvent(TriggerEvent.ToggleActiveState, this.gameObject, ObjectToToggle, isOn);
    }

    private void NetworkStuff()
    {
        object[] content = new object[]
        {
            EventCodes.ToggleActiveState,
            networkID,
            isOn
        };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        PhotonNetwork.RaiseEvent((byte)EventCodes.EventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

#endif
}