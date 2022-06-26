using Unity.Netcode;
using UnityEngine;

public class NetworkStartup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(SceneTransitionHandler.Instance.InitializedAsHost)
        {
            NetworkManager.Singleton.StartHost();
        }
        else
        {
            NetworkManager.Singleton.StartClient();
        }
    }

}
