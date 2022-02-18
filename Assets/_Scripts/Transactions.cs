using System;
using UnityEngine;

[Serializable]
public class Transactions
{
    public int id;

    public string TransactionName;

    public GameObject Prefab;

    public bool IsNew;

    public bool IsUnlockedDefault;
}
