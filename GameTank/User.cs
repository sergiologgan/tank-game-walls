using GameTank.entity;
using GameTank.events;
using GameTank.events.tanque;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTank
{
    public class User : TankEvent
    {
        public override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }

        [EventHanddler]
        public void TanqueIntersect(IntersectParede e)
        {
            if (e.IsIntersect)
            {
                //
            }
        }

        [EventHanddler]
        public void TanqueIntersect1(IntersectAdversary e)
        {
        }
    }
}
