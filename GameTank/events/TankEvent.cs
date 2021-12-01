using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameTank.entity;
using GameTank.sender;

namespace GameTank.events
{
    public class TankEvent
    {
        public virtual void OnEnable()
        {
        }

        public virtual void OnDisable()
        {

        }

        public virtual void OnCommand(string command, Sender sender)
        {
            
        }
    }
}
