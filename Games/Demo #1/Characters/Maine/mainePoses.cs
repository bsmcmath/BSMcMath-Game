using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace forthright
{
    public partial class maine : characterBase
    {
        public void poser()
        {
            stepStateParams p = new(mp.walkState);
            p.pose = mp.poserPose;
            step_bare_state(p);
        }
    }
}