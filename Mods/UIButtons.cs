using Notorious;
using Notorious.API;
using IceBurn.API;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using IceBurn.Mods.Fav;
using UnityEngine;
using IceBurn.Utils;
using VRC;
using VRC.UI;
using VRC.Core;
using VRC.SDKBase;
using static IceBurn.API.QMButtonBase;

namespace IceBurn.Mods.Buttons
{
	public class UIButtons : VRCMod
    {
        public static AvatarListApi CustomList; //это надо

        public static List<GameObject> Buttons = new List<GameObject>();
        public override string Name => "UI Buttons";
        //public override string Description => "This module adds more buttons to the menu.";
        public override string Description => "";
        public UIButtons() : base() {}
        public override void OnStart() 
        {
            //var testBtn = new QMNestedButton("ShortcutMenu", 0, 0, "lol", "BAN NAXUY!");
            //var testBtn2 = new QMSingleButton(testBtn, 0, 1, "BAN!",new Action(() => Console.WriteLine("ssss")), "BAN NAXUY!");

            var DoneteButton = new QMSingleButton("UIElementsMenu", 0, 0,"Donate", new Action(() =>
            {
                Console.WriteLine("Thanks (^-^)");
                Process.Start("https://www.donationalerts.com/r/ice_fox");
            }), "Support Author Please (^-^)");

            var DiscordButton = new QMSingleButton("UIElementsMenu", 0, 1, "Join us", new Action(() =>
            {
                Console.WriteLine("Welcome");
                Process.Start("https://discord.gg/kNDRN7k");
            }), "Join us in discord server");

            var Flightbutton = new QMToggleButton("UIElementsMenu", 3, 0,
            "Fly On", new Action(() => 
            {
                GlobalUtils.DirectionalFlight = true;
                Physics.gravity = GlobalUtils.DirectionalFlight ? Vector3.zero : GlobalUtils.Grav;
                GlobalUtils.ToggleColliders(false);
                Console.WriteLine($"Flight has been enabled.");
            }), "Fly Off", new Action(() => 
            {
                GlobalUtils.DirectionalFlight = false;
                Physics.gravity = GlobalUtils.DirectionalFlight ? Vector3.zero : GlobalUtils.Grav;
                GlobalUtils.ToggleColliders(true);
                Console.WriteLine($"Flight has been disabled.");
            }), "Enable or Disable Flight");

            var ESPbutton = new QMToggleButton("UIElementsMenu", 4, 0,
            "ESP On", new Action(() =>
            {
                GlobalUtils.SelectedPlayerESP = true;
                Console.WriteLine($"Selected ESP has been enabled.");

                GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].transform.Find("SelectRegion"))
                    {
                        array[i].transform.Find("SelectRegion").GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.magenta);
                        HighlightsFX.prop_HighlightsFX_0.EnableOutline(array[i].transform.Find("SelectRegion").GetComponent<Renderer>(), GlobalUtils.SelectedPlayerESP);
                    }
                }

                foreach (VRC_Pickup pickup in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
                {
                    if (pickup.gameObject.transform.Find("SelectRegion"))
                    {
                        pickup.gameObject.transform.Find("SelectRegion").GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.magenta);
                        Wrappers.GetHighlightsFX().EnableOutline(pickup.gameObject.transform.Find("SelectRegion").GetComponent<Renderer>(), GlobalUtils.SelectedPlayerESP);
                    }
                }
            }), "ESP Off", new Action(() =>
            {
                GlobalUtils.SelectedPlayerESP = false;
                Console.WriteLine($"Selected ESP has been disabled");

                GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].transform.Find("SelectRegion"))
                    {
                        array[i].transform.Find("SelectRegion").GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.magenta);
                        HighlightsFX.prop_HighlightsFX_0.EnableOutline(array[i].transform.Find("SelectRegion").GetComponent<Renderer>(), GlobalUtils.SelectedPlayerESP);
                    }
                }

                foreach (VRC_Pickup pickup in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
                {
                    if (pickup.gameObject.transform.Find("SelectRegion"))
                    {
                        pickup.gameObject.transform.Find("SelectRegion").GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.magenta);
                        Wrappers.GetHighlightsFX().EnableOutline(pickup.gameObject.transform.Find("SelectRegion").GetComponent<Renderer>(), GlobalUtils.SelectedPlayerESP);
                    }
                }
            }), "Enable or Disable ESP");

            var JumpButton = new QMToggleButton("UIElementsMenu", 1, 1,
            "Jump On", new Action(() => 
            {
                if (PlayerWrappers.GetCurrentPlayer() != null)
                {
                    if (PlayerWrappers.GetCurrentPlayer().GetComponent<PlayerModComponentJump>() == null)
                    {
                        PlayerWrappers.GetCurrentPlayer().gameObject.AddComponent<PlayerModComponentJump>();
                    }
                }
            }), "Jump Off", new Action(() => 
            {
                if (PlayerWrappers.GetCurrentPlayer() != null)
                {
                    if (PlayerWrappers.GetCurrentPlayer().GetComponent<PlayerModComponentJump>() != null)
                    {
                        UnityEngine.GameObject.Destroy(PlayerWrappers.GetCurrentPlayer().GetComponent<PlayerModComponentJump>());
                    }
                }
            }), "Enable or Disable Jump");

            var ForceQuitButton = new QMSingleButton("UIElementsMenu", 5, 2, "Force Quit",new Action(() =>
            {
                Process.GetCurrentProcess().Kill();
            }), "Quit Game");

            var CloneAvatarButton = new QMSingleButton("UserInteractMenu", 4, 2, "Force Clone", new Action(() =>
            {
                Console.WriteLine("Cloned Avatar");
                var avi = Wrappers.GetQuickMenu().GetSelectedPlayer().field_Internal_VRCPlayer_0.prop_ApiAvatar_0;

                if (avi.releaseStatus != "private")
                {
                    new PageAvatar
                    {
                        avatar = new SimpleAvatarPedestal
                        {
                            field_Internal_ApiAvatar_0 = new ApiAvatar
                            {
                                id = avi.id
                            }
                        }
                    }.ChangeToSelectedAvatar();
                }
            }), "Clone Avatar");

            var DownloadAvatarButton = new QMSingleButton("UserInteractMenu", 4, 3, "Download VRCA", new Action(() =>
            {
                Console.WriteLine("Downloading Avatar...");
                var avatar = Wrappers.GetQuickMenu().GetSelectedPlayer().field_Internal_VRCPlayer_0.prop_ApiAvatar_0;
                Process.Start(avatar.assetUrl);
                Console.WriteLine(avatar.assetUrl);
            }), "Clone Avatar");

            /*var CloneAvatarButton2 = new QMSingleButton("/UserInterface/MenuContent/Screens/Social", 0, 0, "Download VRCA", new Action(() =>
            {
                Console.WriteLine("Downloading Avatar...");
                var avatar = CustomList.AList.avatarPedestal.field_Internal_ApiAvatar_0;
                Process.Start("https://api.vrchat.cloud/api/1/avatars/" + avatar.id);
            }), "Clone Avatar");*/

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (GlobalUtils.DirectionalFlight)
                {
                    Flightbutton.btnOn.SetActive(true);
                    Flightbutton.btnOff.SetActive(false);
                }
                else
                {
                    Flightbutton.btnOn.SetActive(false);
                    Flightbutton.btnOff.SetActive(true);
                }
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                if (GlobalUtils.SelectedPlayerESP)
                {
                    ESPbutton.btnOn.SetActive(true);
                    ESPbutton.btnOff.SetActive(false);
                }
                else
                {
                    ESPbutton.btnOn.SetActive(false);
                    ESPbutton.btnOff.SetActive(true);
                }
            }  //не имеет смысла но пусть будет ._.

            /*try
            {
                if (Buttons.Count() == 0)
                {
                    Transform parent = Wrappers.GetQuickMenu().transform.Find("UIElementsMenu");
                    Transform parentMain = Wrappers.GetQuickMenu().transform.Find("ShortcutMenu");

                    if (parentMain != null)
                    {
                        var TPLists = ButtonAPI.CreateMenuNestedButton("Teleport List", "Teleport List", Color.white, Color.red, 1, 1, parentMain);

                        var TeleportToPlayerI = ButtonAPI.CreateButton(ButtonType.Default, "Teleport", "Teleport To Player", Color.white, Color.red, -2, 1, TPLists, new Action(() =>
                        {

                        }));

                        Buttons.Add(TeleportToPlayerI.gameObject);
                    }
                    if (parent != null)
                    {
                        var Flightbutton = ButtonAPI.CreateButton(ButtonType.Toggle, "Flight", "Enable/Disable Flight", Color.white, Color.red, -1, 0, parent, new Action(() =>
                        {
                            GlobalUtils.DirectionalFlight = true;
                            Physics.gravity = GlobalUtils.DirectionalFlight ? Vector3.zero : GlobalUtils.Grav;
                            GlobalUtils.ToggleColliders(false);
                            Console.WriteLine($"Flight has been {(GlobalUtils.DirectionalFlight ? "Enabled" : "Disabled")}.");
                        }), new Action(() =>
                        {
                            GlobalUtils.DirectionalFlight = false;
                            Physics.gravity = GlobalUtils.DirectionalFlight ? Vector3.zero : GlobalUtils.Grav;
                            GlobalUtils.ToggleColliders(true);
                            Console.WriteLine($"Flight has been {(GlobalUtils.DirectionalFlight ? "Enabled" : "Disabled")}.");
                        }));

                        var ESPbutton = ButtonAPI.CreateButton(ButtonType.Toggle, "ESP", "Enable/Disable Selected ESP", Color.white, Color.red, 0, 0, parent, new Action(() =>
                        {
                            GlobalUtils.SelectedPlayerESP = true;
                            Console.WriteLine($"Selected ESP has been {(GlobalUtils.SelectedPlayerESP ? "Enabled" : "Disabled")}.");

                            GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
                            for (int i = 0; i < array.Length; i++)
                            {
                                if (array[i].transform.Find("SelectRegion"))
                                {
                                    array[i].transform.Find("SelectRegion").GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.magenta);
                                    HighlightsFX.prop_HighlightsFX_0.EnableOutline(array[i].transform.Find("SelectRegion").GetComponent<Renderer>(), GlobalUtils.SelectedPlayerESP);
                                }
                            }

                            foreach (VRC_Pickup pickup in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
                            {
                                if (pickup.gameObject.transform.Find("SelectRegion"))
                                {
                                    pickup.gameObject.transform.Find("SelectRegion").GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.magenta);
                                    Wrappers.GetHighlightsFX().EnableOutline(pickup.gameObject.transform.Find("SelectRegion").GetComponent<Renderer>(), GlobalUtils.SelectedPlayerESP);
                                }
                            }
                        }), new Action(() =>
                        {
                            GlobalUtils.SelectedPlayerESP = false;
                            Console.WriteLine($"Selected ESP has been {(GlobalUtils.SelectedPlayerESP ? "Enabled" : "Disabled")}.");

                            GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
                            for (int i = 0; i < array.Length; i++)
                            {
                                if (array[i].transform.Find("SelectRegion"))
                                {
                                    array[i].transform.Find("SelectRegion").GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.magenta);
                                    HighlightsFX.prop_HighlightsFX_0.EnableOutline(array[i].transform.Find("SelectRegion").GetComponent<Renderer>(), GlobalUtils.SelectedPlayerESP);
                                }
                            }

                            foreach (VRC_Pickup pickup in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
                            {
                                if (pickup.gameObject.transform.Find("SelectRegion"))
                                {
                                    pickup.gameObject.transform.Find("SelectRegion").GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.magenta);
                                    Wrappers.GetHighlightsFX().EnableOutline(pickup.gameObject.transform.Find("SelectRegion").GetComponent<Renderer>(), GlobalUtils.SelectedPlayerESP);
                                }
                            }
                        }));

                        var teleportButton = ButtonAPI.CreateButton(ButtonType.Default, "Teleport", "Teleport to the selected player", Color.white, Color.red, 0, 0, Wrappers.GetQuickMenu().transform.Find("UserInteractMenu"), new Action(() =>
                        {
                            Console.WriteLine("Teleporting to selected player.");
                            var player = PlayerWrappers.GetCurrentPlayer();
                            var SelectedPlayer = Wrappers.GetQuickMenu().GetSelectedPlayer();
                            player.transform.position = SelectedPlayer.transform.position;
                        }));

                        var CloneAvatar = ButtonAPI.CreateButton(ButtonType.Default, "Clone", "Clone the selected player", Color.white, Color.red, 0, 1, Wrappers.GetQuickMenu().transform.Find("UserInteractMenu"), new Action(() =>
                        {
                            Console.WriteLine("Cloned Avatar");
                            var avi = Wrappers.GetQuickMenu().GetSelectedPlayer().field_Internal_VRCPlayer_0.prop_ApiAvatar_0;

                            if (avi.releaseStatus != "private")
                            {
                                new PageAvatar
                                {
                                    avatar = new SimpleAvatarPedestal
                                    {
                                        field_Internal_ApiAvatar_0 = new ApiAvatar
                                        {
                                            id = avi.id
                                        }
                                    }
                                }.ChangeToSelectedAvatar();
                            }
                        }));

                        var Freezebutton = ButtonAPI.CreateButton(ButtonType.Toggle, "Serialize", "Enable/Disable Custom Serialisation", Color.white, Color.red, -3, -1, parent, new Action(() =>
                        {
                            GlobalUtils.Serialise = false;
                            Console.WriteLine($"Custom Serialisation has been Enabled.");
                        }), new Action(() =>
                        {
                            GlobalUtils.Serialise = true;
                            Console.WriteLine($"Custom Serialisation has been Disabled.");
                        }));

                        var ForceCloneButton = ButtonAPI.CreateButton(ButtonType.Toggle, "Jump", "Enable/disable jumping in the current world", Color.white, Color.red, -2, -1, parent, new Action(() =>
                        {
                            if (PlayerWrappers.GetCurrentPlayer() != null)
                            {
                                if (PlayerWrappers.GetCurrentPlayer().GetComponent<PlayerModComponentJump>() == null)
                                {
                                    PlayerWrappers.GetCurrentPlayer().gameObject.AddComponent<PlayerModComponentJump>();
                                }
                            }

                            Console.WriteLine($"Jumping has been Enabled.");
                        }), new Action(() =>
                        {
                            if (PlayerWrappers.GetCurrentPlayer() != null)
                            {
                                if (PlayerWrappers.GetCurrentPlayer().GetComponent<PlayerModComponentJump>() != null)
                                {
                                    UnityEngine.GameObject.Destroy(PlayerWrappers.GetCurrentPlayer().GetComponent<PlayerModComponentJump>());
                                }
                            }

                            Console.WriteLine($"Jumping has been Disabled.");
                        }));

                        var ForceQuit = ButtonAPI.CreateButton(ButtonType.Default, "Force Quit", "Quit From Game", Color.white, Color.red, 1, 1, parent, new Action(() =>
                        {
                            Process.GetCurrentProcess().Kill();
                        }));

                        Buttons.Add(Flightbutton.gameObject);
                        Buttons.Add(ESPbutton.gameObject);
                        Buttons.Add(CloneAvatar.gameObject);
                        Buttons.Add(teleportButton.gameObject);
                        Buttons.Add(Freezebutton.gameObject);
                        Buttons.Add(ForceCloneButton.gameObject);
                        Buttons.Add(ForceQuit.gameObject);
                    }
                }
            }
            catch (Exception) { }*/
        }
        public static void ToggleUIButton(int Index, bool state)
        {
            var gameObject = Buttons[Index];

            if (gameObject == null) return;

            var transform = gameObject.transform;

            var EnableButton = transform.Find("Toggle_States_Visible/ON").gameObject;
            var DisableButton = transform.Find("Toggle_States_Visible/OFF").gameObject;

            if (state)
            {
                DisableButton.SetActive(false);
                EnableButton.SetActive(true);
            }
            else
            {
                EnableButton.SetActive(false);
                DisableButton.SetActive(true);
            }
        }
        public override void OnUpdate()
        {

        }
    }
}
