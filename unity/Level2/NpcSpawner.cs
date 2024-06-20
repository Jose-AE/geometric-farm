
using System;
using UnityEngine;
using UnityEngine.AI;

public class NpcSpawner : MonoBehaviour
{
    [SerializeField] GameObject npcPrefab;
    [SerializeField] Transform startPos;
    [SerializeField] Transform shopPos;
    [SerializeField] Transform exitPos;


    private Npc npc;


    private void SpawnNpc(Transaction transaction)
    {
        GameObject npcObject = Instantiate(npcPrefab, startPos.position, startPos.rotation);

        npc = npcObject.GetComponent<Npc>();
        npc.transaction = transaction;

        npc.MoveTo(shopPos);
    }


    void DespawnNpc()
    {
        npc.HideUi();
        npc.MoveTo(exitPos);
        Destroy(npc.gameObject, 3);
    }

    void OnEnable()
    {
        Level2GameplayLogic.OnGenerateTransaction += SpawnNpc;
        Level2GameplayLogic.OnTransactionCompleted += DespawnNpc;
    }


    void OnDisable()
    {
        Level2GameplayLogic.OnGenerateTransaction -= SpawnNpc;
        Level2GameplayLogic.OnTransactionCompleted -= DespawnNpc;
    }
}
