using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startup : MonoBehaviour
{
    public startupBehavior onStart;
    public enum startupBehavior
    {
        gameSelect, demo1, santaFlappy
    }
    private void Start()
    {
        switch (onStart)
        {
            case startupBehavior.gameSelect:
                Main.main.streaming.loadScene(Main.main.streaming.gameSelectIndex);
                break;
            case startupBehavior.demo1:
                Main.main.streaming.loadScene(Main.main.streaming.demo1Index);
                break;
            case startupBehavior.santaFlappy:
                Main.main.streaming.loadScene(Main.main.streaming.santaFlappyIndex);
                break;
        }
        
        Destroy(gameObject);
    }
}