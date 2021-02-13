using System.IO;
using System.Reflection;
using RoR2;
using R2API;
using UnityEngine;

namespace Ursa.Core
{
    public static class Assets
    {

        public static AssetBundle MainAssetBundle = null;
        public static AssetBundleResourcesProvider Provider;

        public static Sprite icon1;
        public static Sprite icon2;
        public static Sprite icon3;
        public static Sprite icon4;
        public static Sprite icon5;
        public static Sprite portrait;

        public const string ursaSwingSound = "Ursa_swing";
        public const string ursaHitSound = "Ursa_hit";
        public const string ursaOverpowerSound = "Ursa_overpower";
        public const string ursaEarthshockSound = "Ursa_earthshock";
        public const string ursaEnrageSound = "Ursa_enrage";

        public static NetworkSoundEventDef ursaHitNetworkSoundEventDef;

        public static void InitializeAssets()
        {

            if (MainAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ursa.ursasurvivorbundle"))
                {
                    MainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                    Provider = new AssetBundleResourcesProvider("@Ursa", MainAssetBundle);
                }
            }

            icon1 = MainAssetBundle.LoadAsset<Sprite>("Skill1Icon");
            icon2 = MainAssetBundle.LoadAsset<Sprite>("Skill2Icon");
            icon3 = MainAssetBundle.LoadAsset<Sprite>("Skill3Icon");
            icon4 = MainAssetBundle.LoadAsset<Sprite>("Skill4Icon");
            icon5 = MainAssetBundle.LoadAsset<Sprite>("Skill5Icon");
            portrait = MainAssetBundle.LoadAsset<Sprite>("Portrait");

            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ursa.UrsaSoundbank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }
        }
    }
}
