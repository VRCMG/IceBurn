using BestHTTP;
using MelonLoader;
using NET_SDK;
using Notorious;
using IceBurn.Utils;
using IceBurn.Mods.Buttons;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.SDKBase;
using VRC.UI;
using UnityEngine.UI;

namespace IceBurn.Mods
{
    public class InputHandler : VRCMod
    {
        public override string Name => "Input Handler";

        //public override string Description => "This module handles all the given user input.";
        public override string Description => "";

        public InputHandler() : base() { }

        public override void OnStart()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
        }

        private static UserInteractMenu CachedUserInteract { get; set; }

        public static void RayTeleport()
        {
            Ray ray = new Ray(Wrappers.GetPlayerCamera().transform.position, Wrappers.GetPlayerCamera().transform.forward);

            RaycastHit[] hits = Physics.RaycastAll(ray);
            if (hits.Length > 0)
            {
                RaycastHit raycastHit = hits[0];
                var thisPlayer = PlayerWrappers.GetCurrentPlayer();
                thisPlayer.transform.position = raycastHit.point;
            }

        }

        private float currentSpeed = 5f;
        private bool isLockedLook = false;

        public override void OnUpdate()
        {

            if (Input.GetKeyDown(KeyCode.B))
            {
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
            }

            if (Input.GetKey(KeyCode.F10))
            {
                var sPlayer = Wrappers.GetQuickMenu().GetSelectedPlayer();
                var CurrentPlayerCam = Wrappers.GetPlayerCamera();
                isLockedLook = !isLockedLook;
                if (sPlayer != null && isLockedLook)
                {
                    CurrentPlayerCam.transform.LookAt(sPlayer.transform);
                }
            }
            if (Input.GetKeyUp(KeyCode.F10))
            {
                var CurrentPlayerCam = Wrappers.GetPlayerCamera();
                CurrentPlayerCam.transform.rotation = Quaternion.identity;
            }

            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.F5))
            {
                Process.GetCurrentProcess().Kill();
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
            {
                //Respawn Делай даун
            }

            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.T))
            {
                //TPS Делай Сука
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                GlobalUtils.DirectionalFlight = !GlobalUtils.DirectionalFlight;
                Physics.gravity = GlobalUtils.DirectionalFlight ? Vector3.zero : GlobalUtils.Grav;
                if (!GlobalUtils.DirectionalFlight)
                {
                    GlobalUtils.ToggleColliders(true);
                }
                else
                {
                    GlobalUtils.ToggleColliders(false);
                }
                //UIButtons.ToggleUIButton(0, GlobalUtils.DirectionalFlight);
                Console.WriteLine($"Flight has been {(GlobalUtils.DirectionalFlight ? "Enabled" : "Disabled")}.");
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                GlobalUtils.SelectedPlayerESP = !GlobalUtils.SelectedPlayerESP;
                Console.WriteLine($"Selected ESP has been {(GlobalUtils.SelectedPlayerESP ? "Enabled" : "Disabled")}.");
                //UIButtons.ToggleUIButton(1, GlobalUtils.SelectedPlayerESP);
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
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.T))
            {
                RayTeleport();
                Console.WriteLine("Teleported");
            }

            if (GlobalUtils.DirectionalFlight)
            {
                GameObject gameObject = Wrappers.GetPlayerCamera();
                var player = PlayerWrappers.GetCurrentPlayer();

                if (currentSpeed <= 0f)
                {
                    currentSpeed = 1f;
                }

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    currentSpeed *= 2f;
                }
                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    currentSpeed /= 2f;
                }

                if (Input.mouseScrollDelta.y != 0)
                {
                    currentSpeed += Input.mouseScrollDelta.y;
                    Console.WriteLine("Speed Changed: [" + currentSpeed +"]");
                }

                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
                {
                    currentSpeed = 5f;
                    Console.WriteLine("Fly Speed Reset [5]");
                }

                if (Input.GetKey(KeyCode.W))
                {
                    player.transform.position += gameObject.transform.forward * currentSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    player.transform.position -= player.transform.right * currentSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    player.transform.position -= gameObject.transform.forward * currentSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    player.transform.position += player.transform.right * currentSpeed * Time.deltaTime;
                }

                if (Input.GetKey(KeyCode.E))
                {
                    player.transform.position += player.transform.up * currentSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.Q))
                {
                    player.transform.position -= player.transform.up * currentSpeed * Time.deltaTime;
                }

                if (Math.Abs(Input.GetAxis("Joy1 Axis 2")) > 0f)
                {
                    player.transform.position += gameObject.transform.forward * currentSpeed * Time.deltaTime * (Input.GetAxis("Joy1 Axis 2") * -1f);
                }
                if (Math.Abs(Input.GetAxis("Joy1 Axis 1")) > 0f)
                {
                    player.transform.position += gameObject.transform.right * currentSpeed * Time.deltaTime * Input.GetAxis("Joy1 Axis 1");
                }
            }
        }
    }
}
