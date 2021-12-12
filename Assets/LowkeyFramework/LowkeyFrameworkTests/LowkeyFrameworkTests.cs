using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class LowkeyFrameworkTests
{
    [Test]
    public void WeightedListTest()
    {
        int testsNum = 100000;
        WeightedElement element1 = new WeightedElement(1f);
        WeightedElement element3 = new WeightedElement(3f);
        WeightedElement element6 = new WeightedElement(6f);

        Dictionary<WeightedElement, int> chosenElementNums = new Dictionary<WeightedElement, int>();
        chosenElementNums.Add(element1, 0);
        chosenElementNums.Add(element3, 0);
        chosenElementNums.Add(element6, 0);

        WeightedList<WeightedElement> weightedList = new WeightedList<WeightedElement>(new List<WeightedElement> 
        {
            element1,
            element3,
            element6,
        });

        for(int i = 0; i < testsNum; i++)
        {
            WeightedElement chosenElement = weightedList.GetRandomWeightedElement();
            chosenElementNums[chosenElement]++;
        }

        Assert.GreaterOrEqual(chosenElementNums[element1], 9000, "Element with weight of 1");
        Assert.LessOrEqual(chosenElementNums[element1], 11000, "Element with weight of 1");
        Assert.GreaterOrEqual(chosenElementNums[element3], 29000, "Element with weight of 3");
        Assert.LessOrEqual(chosenElementNums[element3], 31000, "Element with weight of 3");
        Assert.GreaterOrEqual(chosenElementNums[element6], 59000, "Element with weight of 6");
        Assert.LessOrEqual(chosenElementNums[element6], 61000, "Element with weight of 6");
    }

    private class WeightedElement : IWeightedElement
    {
        private readonly float weight = 1f;

        public float Weight => weight;

        public WeightedElement(float weight)
        {
            this.weight = weight;
        }
    }
}
