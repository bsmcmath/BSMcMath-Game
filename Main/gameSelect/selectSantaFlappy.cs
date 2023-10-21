using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectSantaFlappy : UIoption
{
    public override void MakeSelection()
    {
        Main.main.streaming.loadScene(Main.main.streaming.santaFlappyIndex);
        Main.main.streaming.unloadScene(Main.main.streaming.gameSelectIndex);
    }
}
