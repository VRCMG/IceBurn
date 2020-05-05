using Harmony;
using MelonLoader;
using NET_SDK;
using NET_SDK.Harmony;
using NET_SDK.Reflection;
using IceBurn.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC;

namespace IceBurn.Mods
{
    public class Protections : VRCMod
    {
        public override string Name => "Protections";

        //public override string Description => "A module for IceBurn which gives you the basic necessities of not being forcekicked";
        public override string Description => "";

        public Protections() : base() { }

        public override void OnStart()
        {
            try
            {
                var HarmonyInstance = Manager.CreateInstance("Quality Assurance");
                var API = SDK.GetClass("VRC.Core", "API");
                var Amp = SDK.GetClass("AmplitudeSDKWrapper", "AmplitudeWrapper");
                var Photon = SDK.GetClass("Photon.Pun", "PhotonView");
                var AvatarManager = SDK.GetClass("", "VRCAvatarManager");
                var moderationManager = SDK.GetClass("", "ModerationManager");
                HarmonyInstance.Patch(API.GetMethod("DeviceID"), AccessTools.Method(typeof(Protections), "HWIDSpoofer"));
                HarmonyInstance.Patch(Amp.GetMethod("InitializeDeviceId"), AccessTools.Method(typeof(Protections), "HWIDSpoofer"));
                //HarmonyInstance.Patch(Photon.GetMethod("Method_Public_Type1595182416_Type2348106871_2"), AccessTools.Method(typeof(Protections), "SerializeView")); //Last function to take class, struct parameters only.

            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An exception has occurred. Dm IceFox#1996 on discord. This mod may be outdated.");
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Console.WriteLine("Protections have been applied.");
                Console.WriteLine("Your New HWID: " + VRC.Core.API.DeviceID);
                Console.WriteLine("IsOffline: " + VRC.Core.API.IsOffline());
            }
        }
        public static bool SerializeView()
        {
            return GlobalUtils.Serialise;
        }
        public static string HWIDSpoofer()
        {
            var crypt = new SHA256Managed();
            string hash = string.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(new System.Random().Next(1, 999999999).ToString()));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }
    }
}
