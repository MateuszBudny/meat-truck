using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : DontDestroySingleBehaviour<GameManager>
{
    [SerializeField]
    private Player player;
    public Player Player
    {
        get => player;
        private set => player = value;
    }

    protected override void Awake()
    {
        base.Awake();

        player.Init();
    }
}