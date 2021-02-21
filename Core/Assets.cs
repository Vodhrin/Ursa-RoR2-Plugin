using System.IO;
using System.Reflection;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using R2API;

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
        public static Sprite defaultSkinIcon;

        public const string ursaSwingSound = "Ursa_swing";
        public const string ursaHitSound = "Ursa_hit";
        public const string ursaOverpowerSound = "Ursa_overpower";
        public const string ursaEarthshockSound = "Ursa_earthshock";
        public const string ursaEnrageSound = "Ursa_enrage";

        public static NetworkSoundEventDef ursaHitNetworkSoundEventDef;

        public static GameObject earthshockEffect;

        public static void InitializeAssets()
        {

            if (MainAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ursa.ursasurvivorbundle"))
                {
                    MainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                    Provider = new AssetBundleResourcesProvider("@Ursa", MainAssetBundle);
                    ResourcesAPI.AddProvider(Provider);
                }
            }

            icon1 = MainAssetBundle.LoadAsset<Sprite>("Skill1Icon");
            icon2 = MainAssetBundle.LoadAsset<Sprite>("Skill2Icon");
            icon3 = MainAssetBundle.LoadAsset<Sprite>("Skill3Icon");
            icon4 = MainAssetBundle.LoadAsset<Sprite>("Skill4Icon");
            icon5 = MainAssetBundle.LoadAsset<Sprite>("Skill5Icon");
            portrait = MainAssetBundle.LoadAsset<Sprite>("Portrait");
            defaultSkinIcon = MainAssetBundle.LoadAsset<Sprite>("DefaultSkinIcon");

            earthshockEffect = LoadCustomEffect("earthshockEffect", "");

            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ursa.UrsaSoundbank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }
        }

        //Yoinked from rob's PaladinMod :)
        private static GameObject LoadCustomEffect(string resourceName, string soundName)
        {
            GameObject newEffect = MainAssetBundle.LoadAsset<GameObject>(resourceName);

            newEffect.AddComponent<DestroyOnTimer>().duration = 12;
            newEffect.AddComponent<NetworkIdentity>();
            newEffect.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            var effect = newEffect.AddComponent<EffectComponent>();
            effect.applyScale = false;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = true;
            effect.positionAtReferencedTransform = true;
            effect.soundName = soundName;

            EffectAPI.AddEffect(newEffect);

            return newEffect;
        }
    }
}
