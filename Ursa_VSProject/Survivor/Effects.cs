namespace Ursa.Survivor
{
    internal static class Effects
    {
        public static void Initialize()
        {
            if (!Core.Config.utilityRocksEffect.Value)
            {
                var rocks = Core.Assets.earthshockEffect.transform.Find("RockKickUp").gameObject;
                if (rocks)
                {
                    UnityEngine.Object.Destroy(Core.Assets.earthshockEffect.transform.Find("RockKickUp").gameObject);
                }
            }
        }
    }
}
