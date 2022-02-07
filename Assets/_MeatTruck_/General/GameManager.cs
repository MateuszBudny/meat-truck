using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : DontDestroySingleBehaviour<GameManager>
{
    [SaveField]
    [SerializeField]
    private Player player;
    [JsonIgnore]
    public Player Player
    {
        get => player;
        private set => player = value;
    }

    protected override void Awake()
    {
        base.Awake();

        Player.Init();
    }
}