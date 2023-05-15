using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NifuDev
{
    public abstract class BaseSlowMotionObjectAbstract
    {
        public abstract void ActiveSlowMotion(float slowDownMult);

        public abstract void StopSlowMotion(float slowDownMult);
    }
}