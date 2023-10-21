using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace forthright
{
    public class warpMenu : UIMenu
    {
        public override void makeSelection()
        {
            //GM.gm.warping.warpTo(selection);
            GM.gm.pause.menu.resume();
            gameObject.SetActive(false);
        }
        public override void goBack()
        {
            GM.gm.pause.menu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}