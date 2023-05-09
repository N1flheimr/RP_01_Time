using System.Collections;
using UnityEngine;

namespace NifuDev
{
    public interface ISlowMotionObject
    {
        public void ActiveSlowMotion(float slowDownMult);

        public void StopSlowMotion(float slowDownMult);
    }
}

