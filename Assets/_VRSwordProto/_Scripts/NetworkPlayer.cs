using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using System.IO;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField]
    private Vector2 placementArea = new Vector2(-10.0f,10.0f);

    public override void OnNetworkSpawn()
    {
        DisableClientInput();
    }

    public void DisableClientInput()
    {
        if(IsClient && !IsOwner)
        {
            var clientMoveProvider = GetComponent<NetworkMoveProvider>();
            var clientControllers = GetComponentsInChildren<ActionBasedController>();
            //var clientTurnProvider = GetComponent<ActionBasedContinuousTurnProvider>();
            var clientTurnProvider = GetComponent<ActionBasedSnapTurnProvider>();
            var clientHead = GetComponentsInChildren<TrackedPoseDriver>(); 
            var clientCamera = GetComponentsInChildren<Camera>();

            clientCamera[0].enabled = false;
            GetComponent<NetworkMoveProvider>().enableInputActions = false;
            clientTurnProvider.enableTurnLeftRight = false;
            clientTurnProvider.enableTurnAround = false;
            clientHead[0].enabled = false;

            foreach (var controller in clientControllers)
            {
                controller.enableInputActions = false;
                controller.enableInputTracking = false;

            }
        }

    }
    
    private void Start()
    {
        if(IsClient && IsOwner)
        {
            transform.position =  new Vector3(Random.Range(placementArea.x, placementArea.y),transform.position.y,
            Random.Range(placementArea.x,placementArea.y));
        }
    }

    public void OnSelectGrabbable(SelectEnterEventArgs eventArgs)
    {
        if(IsClient && IsOwner)
        {
            NetworkObject networkObjectSelected = eventArgs.interactableObject.transform
                .GetComponent<NetworkObject>();
            
            //if connection droped
            if(networkObjectSelected != null)
            {
                // request
                RequestGrabbableOwnershipServerRpc(OwnerClientId, networkObjectSelected);
            }
            
        }
    }

    [ServerRpc]
    public void RequestGrabbableOwnershipServerRpc(ulong newOwnerClienId, 
    NetworkObjectReference networkObjectReference)
    {
        if(networkObjectReference.TryGet(out NetworkObject networkObject))
        {
            networkObject.ChangeOwnership(newOwnerClienId);
        }
        else
        {
            //Logger.Instance.LogWarning($"Unable to change ownership for clientid {newOwnerClientId}");
            print("idklogger");
        }

    }
}
