using Terraria.ModLoader.Config;
using System.ComponentModel;

namespace BossRush.Common
{
    public class BossRushModConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(true)]
        public bool EnableChallengeMode { get; set; }

        [DefaultValue(true)]
        public bool SynergyMode { get; set; }

        [DefaultValue(false)]
        public bool VeteranMode { get; set; }

        [DefaultValue(false)]
        public bool Enraged { get; set; }

        [ReloadRequired]
        [DefaultValue(false)]
        public bool Nightmare { get; set; }
        [ReloadRequired]
        [DefaultValue(false)]
        public bool NightmarePlus { get; set; }
        [ReloadRequired]
        [DefaultValue(false)]
        public bool ExtraChallenge { get; set; }

        [DefaultValue(true)]
        public bool RoguelikeOverhaul { get; set; }
    }
}