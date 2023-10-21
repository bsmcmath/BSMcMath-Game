using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace forthright
{
    public class bandit : characterBase
    {
        public float health;
        public banditParams bp;

        public banditState state;
        public enum banditState
        {
            standing,
        }
        public void basicStep()
        {
            step_standard_state(bp.basicStep);
        }
    }
}