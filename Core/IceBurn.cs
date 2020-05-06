using IceBurn.Mods.Fav;
using IceBurn.Mods.Buttons;
using IceBurn.Mods;
using MelonLoader;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC.Core;
using IceBurn.Mods.Fav.Config;
using IceBurnIceBurn.Mods.Joke;
using Notorious;
//using IceBurn.Mods.Buttons;

namespace IceBurn
{
	public class IceBurn : MelonMod
    {
        public static AvatarListApi CustomList;
        public static AviPButton FavoriteButton;
        public static AviPButton DownloadButton;
        public static List<VRCMod> Modules = new List<VRCMod>();
        public override void OnApplicationStart()
        {
            Modules.Add(new UIButtons());
            Modules.Add(new InputHandler());
			Modules.Add(new BanNaxuy());

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=========== KEYBINDS ==============");
            Console.WriteLine("B  - Clone Selected Avatar");
            Console.WriteLine("F - Enable/Disable Flight");
			Console.WriteLine("O - Enable/Disable Selected ESP");
			Console.WriteLine("T - Ray Teleport");
            Console.WriteLine("Alt + F5 - ShutDown This Shit");
            Console.WriteLine("===================================");
			Config.LoadConfig();
		
	}

        public override void OnUpdate()
        {
			foreach (var item in Modules)
			{
				item.OnUpdate();
			}
		}

		public override void VRChat_OnUiManagerInit()
		{
			//Modules.Add(new UIButtons());

			foreach (var item in Modules)
			{
				item.OnStart();
			}

			if (Config.CFG.Custom)
			{
				CustomList = AvatarListApi.Create(Config.CFG.CustomName + " / " + Config.DAvatars.Count, 1);
				CustomList.AList.FirstLoad(Config.DAvatars);


				//New Age - Delegates lol // thanks for the help khan understanding this.
				Il2CppSystem.Delegate test = (Il2CppSystem.Action<string, GameObject, VRCSDK2.Validation.Performance.Stats.AvatarPerformanceStats>)new Action<string, GameObject, VRCSDK2.Validation.Performance.Stats.AvatarPerformanceStats>((x, y, z) =>
				{
					if (Config.DAvatars.Any(v => v.AvatarID == CustomList.AList.avatarPedestal.field_Internal_ApiAvatar_0.id))
					{
						FavoriteButton.Title.text = Config.CFG.RemoveFavoriteTXT;
						CustomList.ListTitle.text = Config.CFG.CustomName + " / " + Config.DAvatars.Count;
					}
					else
					{
						FavoriteButton.Title.text = Config.CFG.AddFavoriteTXT;
						CustomList.ListTitle.text = Config.CFG.CustomName + " / " + Config.DAvatars.Count;
					}

				});

				//Insane how long this line is LOL;
				CustomList.AList.avatarPedestal.field_Internal_Action_3_String_GameObject_AvatarPerformanceStats_0 = Il2CppSystem.Delegate.Combine(CustomList.AList.avatarPedestal.field_Internal_Action_3_String_GameObject_AvatarPerformanceStats_0, test).Cast<Il2CppSystem.Action<string, GameObject, VRCSDK2.Validation.Performance.Stats.AvatarPerformanceStats>>();


				//Add-Remove Favorite Button
				FavoriteButton = AviPButton.Create(Config.CFG.AddFavoriteTXT, 0f, 2f);
				FavoriteButton.SetAction(() =>
				{
					var avatar = CustomList.AList.avatarPedestal.field_Internal_ApiAvatar_0;
					if (avatar.releaseStatus != "private")
					{
						if (!Config.DAvatars.Any(v => v.AvatarID == avatar.id))
						{
							AvatarListHelper.AvatarListPassthru(avatar);
							CustomList.AList.Refresh(Config.DAvatars.Select(x => x.AvatarID).Reverse());
							FavoriteButton.Title.text = Config.CFG.RemoveFavoriteTXT;
							CustomList.ListTitle.text = Config.CFG.CustomName + " / " + Config.DAvatars.Count;
						}
						else
						{

							AvatarListHelper.AvatarListPassthru(avatar);
							CustomList.AList.Refresh(Config.DAvatars.Select(x => x.AvatarID).Reverse());
							FavoriteButton.Title.text = Config.CFG.AddFavoriteTXT;
							CustomList.ListTitle.text = Config.CFG.CustomName + " / " + Config.DAvatars.Count;
						}
					}
				});

				DownloadButton = AviPButton.Create(Config.CFG.DownloadTXT, 1150f, 9.6f);
				DownloadButton.SetAction(() =>
				{
					var avatar = CustomList.AList.avatarPedestal.field_Internal_ApiAvatar_0;
					Process.Start(avatar.assetUrl);
				});

				//Author Button
				var t = AviPButton.Create("Show Author", 0f, 9.6f);
				var scale = t.GameObj.transform.localScale;
				t.GameObj.transform.localScale = new Vector3(scale.x - 0.1f, scale.y - 0.1f, scale.z - 0.1f);
				t.SetAction(() =>
				{
					VRCUiManager.prop_VRCUiManager_0.Method_Public_Void_Boolean_0(true);
					APIUser.FetchUser(CustomList.AList.avatarPedestal.field_Internal_ApiAvatar_0.authorId, new Action<APIUser>(x =>
					{

						QuickMenu.prop_QuickMenu_0.prop_APIUser_0 = x;
						QuickMenu.prop_QuickMenu_0.Method_Public_Void_Int32_Boolean_0(4, false);


					}), null);
				});
			}
		}
	}
}
