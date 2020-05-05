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
        public VRCMod()
        {
            //Console.WriteLine($"VRC Mod {this.Name} has Loaded. {this.Description}");
        }
        public virtual void OnStart()
        {
            new Thread(() =>
            {
                for(; ;)
                {
                    Thread.Sleep(15000);
                    OnUpdate();
                }
            })
            { IsBackground = true }.Start();
        }
        public virtual void OnUpdate()
        {

        }
    }
}
