using Terraria;
using Terraria.ID;
using Terraria.Audio;
using BossRush.Common;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace BossRush.Contents.Items.Spawner
{
    public abstract class EnragedSpawner : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 12;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
            PostSetStaticDefaults();
        }
        public virtual void PostSetStaticDefaults() { }
        public override void SetDefaults()
        {
            Item.height = Item.width = 10;
            Item.maxStack = 30;
            Item.value = 100;
            Item.useAnimation = Item.useTime = 30;
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }
        public virtual int BossToSpawn => NPCID.GreenSlime;
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                player.GetModPlayer<ModdedPlayer>().Enraged = true;
                SoundEngine.PlaySound(SoundID.Roar, player.position);
                int type = BossToSpawn;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.SpawnOnPlayer(player.whoAmI, type);
                }
                else
                {
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
                }
                OnUseItem(player);
            }
            return true;
        }
        /// <summary>
        /// This is called in the check if player whoAmI is myPlayer
        /// </summary>
        /// <param name="player"></param>
        public virtual void OnUseItem(Player player) { }
    }
}