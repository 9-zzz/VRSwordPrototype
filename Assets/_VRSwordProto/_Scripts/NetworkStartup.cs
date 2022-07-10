using Unity.Netcode;
using UnityEngine;

public class NetworkStartup : MonoBehaviour
{
    public bool isHost;

    // Start is called before the first frame update
    void Start()
    {
        //if(SceneTransitionHandler.Instance.InitializedAsHost)
        if(isHost)
        {
            NetworkManager.Singleton.StartHost();
        }
        else
        {
            NetworkManager.Singleton.StartClient();
        }
    }

}
