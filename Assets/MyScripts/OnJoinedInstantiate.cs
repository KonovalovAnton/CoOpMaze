using UnityEngine;
using System.Collections;

public class OnJoinedInstantiate : MonoBehaviour
{
    public Transform SpawnPosition1;
    public Transform SpawnPosition2;
    public GameObject[] PrefabsToInstantiate;

    public delegate void OnCharacterInstantiated(GameObject character);
    public static event OnCharacterInstantiated CharacterInstantiated;

    public void OnJoinedRoom()
    {
        if (this.PrefabsToInstantiate != null)
        {
            GameObject o = StartOptions.spectator? PrefabsToInstantiate[1] : PrefabsToInstantiate[0];

            Debug.Log("Instantiating: " + o.name);

            Vector3 spawnPos = Vector3.up;
            if (this.SpawnPosition1 != null && PhotonNetwork.playerList.Length % 2 == 0)
            {
                spawnPos = this.SpawnPosition1.position;
            }
            else if (this.SpawnPosition2 != null && PhotonNetwork.playerList.Length % 2 == 1)
            {
                spawnPos = this.SpawnPosition2.position;
            }
            var go = PhotonNetwork.Instantiate(o.name, spawnPos, SpawnPosition1.rotation, 0);
            if (CharacterInstantiated != null)
            {
                CharacterInstantiated(go);
            }
        }
    }
}
