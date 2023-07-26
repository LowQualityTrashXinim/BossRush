using Terraria;
using Terraria.ID;
using Terraria.DataStructures;

namespace BossRush.Contents.Items.Spawner
{
    public class BleedingWorm : BaseSpawnerItem
    {
        public override int[] NPCtypeToSpawn => new int[] { NPCID.BloodNautilus };
        public override void PostSetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 7));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetSpawnerDefault(out int width, out int height)
        {
            height = 55;
            width = 53;
        }
        public override bool CanUseItem(Player player)
        {
            return !Main.dayTime && player.ZoneBeach;
        }
        public override bool UseSpecialSpawningMethod => true;
        public override void SpecialSpawningLogic(Player player)
        {
            int spawnY = 750;
            NPC.SpawnBoss((int)player.Center.X, (int)(player.Center.Y - spawnY), NPCtypeToSpawn[0], player.whoAmI);
            Main.bloodMoon = true;
        }
    }
}