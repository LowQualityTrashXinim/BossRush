using Terraria.ModLoader.Config;
using System.ComponentModel;

namespace BossRush
{
    public class BossRushModConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Enable Challenge Mode ( On World Generation )")]
        [Tooltip("The intended way to play the mod" +
            "\nWARNING : NOT RECOMMEND TO PLAY THIS WITH OTHER MOD" +
            "\nTHIS WILL MESS WITH YOUR WORLD GEN" +
            "\nit is recommend to enable this if you want to experience what the mod have to offer along with Synergy Mode" +
            "\nDisable it for more vanilla QoL feel of the mod and to prevent world gen conflict" +
            "\nWhat this mode do :" +
            "\n-Pot drop nothing" +
            "\n-NPC sell nothing" +
            "\n-World gen change so you could easily build arena and chest gone")]
        [DefaultValue(true)]
        public bool EnableChallengeMode { get; set; }

        [Label("Synergy Mode (W.I.P)")][ReloadRequired]
        [Tooltip("Make bosses queen bee, deerclop, queen slime, duke fishron and empress of light drop synergy energy\n" +
            "Allow possibility of getting even stronger and possibly OP weapons/accessories\n" +
            "recommend enable after at least 3 playthroughs of the mod")]
        [DefaultValue(true)]
        public bool SynergyMode { get; set; }

        [Label("Enable Easy Mode ( On Making Character )")]
        [Tooltip("Grant you a small starter boost for those who are casual\n" +
            "Give you 3 Mana Crystal and Life Crystal\n" +
            "1 random starter weapon that this mod have to offer if synergy mode is enable")]
        [DefaultValue(true)]
        public bool EasyMode { get; set; }

        [Label("Enraged Mode (W.I.P)")][ReloadRequired]
        [Tooltip("Will make every boss in the vanilla if there a special fight variant activate\n" +
            "won't drop the reward, will still require to spawn it manually using power energy to get reward\n" +
            "recommend for practicing")]
        [DefaultValue(false)]
        public bool Enraged { get; set; }

        [Label("You like to hurt yourself(W.I.P)")]
        [Tooltip("Will enable ForTheWorthy seeds regardless of world type, require you to play in master mode\n" +
            "Upon entering a world, you will be given a broken artifact that you can craft into a stronger artifact\n" +
            "Some secret are reveal when you have certain player name")]
        [ReloadRequired]
        [DefaultValue(false)]
        public bool YouLikeToHurtYourself { get; set; }

        [Label("ExtraChallange(W.I.P)")]
        [Tooltip("The first time you kill a boss, you will get a challenge added into your game, this is irrevertible" +
            "\nThe next time you kill a boss, the current challenge will be swap for a random challenge" +
            "\nThese mode offer no real reward or benefit, only activate if you are bored and want extra challenge")]
        [ReloadRequired]
        [DefaultValue(false)]
        public bool ExtraChallenge { get; set; }
    }
}

