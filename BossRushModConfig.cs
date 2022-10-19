using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;

namespace BossRush
{
    public class BossRushModConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Enable Challenge Mode ( On World Generation )")]
        [Tooltip("The intended way to play the mod\n" +
            "it is recommend to enable this if you want to experience what the mod have to offer along with Synergy Mode\n" +
            "Disable it for more vanilla QoL feel of the mod to offer and to prevent world gen conflict")]
        public bool EnableChallengeMode { get; set; }

        [Label("Synergy Mode (W.I.P)")][ReloadRequired]
        [Tooltip("Make bosses queen bee, deerclop, queen slime, duke fishron and empress of light drop synergy energy\n" +
            "Allow possibility of getting even stronger and possibly OP weapons/accessories\n" +
            "recommend enable after at least 3 playthroughs of the mod")]
        public bool SynergyMode { get; set; }

        [Label("Enable Easy Mode ( On Making Character )")]
        [Tooltip("Grant you a small starter boost for those who are casual\n" +
            "Give you 3 Mana Crystal and Life Crystal\n" +
            "1 random starter weapon that this mod have to offer")]
        public bool EasyMode { get; set; }

        [Label("Enraged Mode (W.I.P)")][ReloadRequired]
        [Tooltip("Will make every boss in the vanilla if there a special fight variant activate\n" +
            "won't drop the reward, will still require to spawn it manually using power energy to get reward\n" +
            "recommend for practicing")]
        public bool Enraged { get; set; }

        [Label("You like to hurt yourself(W.I.P)")]
        [Tooltip("Will enable ForTheWorthy seeds regardless of world type, require you to play in master mode\n" +
            "Upon entering a world, you will be given a broken artifact that you can craft into a stronger artifact\n" +
            "Some secret are reveal when you have certain player name")]
        [ReloadRequired]
        public bool YouLikeToHurtYourself { get; set; }

    }
}

