using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IceBurn.Mods
{
    public class VRCMod
    {
        public virtual string Name => "Example Module";
        public virtual string Description => "";
        public virtual void OnStart()
        {
        }
        public virtual void OnUpdate()
        {
        }
    }
}
