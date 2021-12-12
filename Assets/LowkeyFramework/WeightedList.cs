using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class WeightedList<T> where T : IWeightedElement
{
    [SerializeField]
    private List<T> list;

    public List<T> List { get => list; private set => list = value; }

    public WeightedList(List<T> standardList)
    {
        List = new List<T>(standardList);
    }

    public WeightedList()
    {
        List = new List<T>();
    }

    public T GetRandomWeightedElement()
    {
        float weightsSum = List.Sum(weightedElement => weightedElement.Weight);
        float randomWeight = UnityEngine.Random.Range(0f, weightsSum);
        float currentWeightsSum = 0f;
        T randomWeightedElement = List.Find(weightedElement =>
        {
            currentWeightsSum += weightedElement.Weight;
            return currentWeightsSum >= randomWeight;
        });

        return randomWeightedElement;
    }
}
