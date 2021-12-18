using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCashTableData", menuName = "MeatTruck/Cash Table Data")]
public class CashTableData : ScriptableObject
{
    [SerializeField]
    private List<CashBehaviour> cashBehaviours;

    public Dictionary<int, CashBehaviour> CashValues { get; private set; }

    private void OnEnable()
    {
        CashValues = cashBehaviours.ToDictionary(cash => cash.value);
    }
}
