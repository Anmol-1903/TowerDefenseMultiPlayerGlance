using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    private void Awake()
    {
        MasterManager.NetworkInstantiate(prefab, transform.position, Quaternion.identity);
    }
}
