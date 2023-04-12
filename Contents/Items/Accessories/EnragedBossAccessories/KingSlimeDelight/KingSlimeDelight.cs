using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using BossRush.Texture;

namespace BossRush.Contents.Items.Accessories.EnragedBossAccessories.KingSlimeDelight
{
    internal class KingSlimeDelight : ModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Truely slime-da-ful" +
                "\nIncrease player Jump speed" +
                "\nIncrease movement speed by 10%" +
                "\nIncrease defense by 5" +
                "\nYou shoot out slime spike when enemy is in range" +
                "\nShoot out slime spike when you shot a enemy");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 40;
            Item.width = 40;
            Item.rare = 7;
            Item.value = 10000000;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.jumpSpeedBoost += 2.5f;
            player.accRunSpeed += .1f;
            player.statDefense += 5;
            player.GetModPlayer<KingSlimePowerPlayer>().KingSlimePower = true;
        }
    }

    internal class KingSlimePowerPlayer : ModPlayer
    {
        //King Slime
        int KSPcounter = 0;
        public bool KingSlimePower;
        public override void ResetEffects()
        {
            KingSlimePower = false;
        }

        public override void UpdateEquips()
        {
            if (KingSlimePower)
            {
                EntitySource_ItemUse entity = new EntitySource_ItemUse(Player, new Item(ModContent.ItemType<KingSlimeDelight>()));
                bool foundTarget = false;
                float distanceFromTarget = 500;
                Vector2 targetCenter = Player.Center;

                if (!foundTarget)
                {
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.CanBeChasedBy())
                        {
                            float between = Vector2.Distance(npc.Center, Player.Center);
                            bool inRange = between < distanceFromTarget;
                            if (inRange && !foundTarget)
                            {
                                distanceFromTarget = between;
                                targetCenter = npc.Center;
                                foundTarget = true;
                            }
                        }
                    }
                }
                if (foundTarget)
                {
                    KSPcounter++;
                    if (KSPcounter == 100)
                    {
                        float rotation = MathHelper.ToRadians(180);
                        float ProjectileNum = 18;
                        Vector2 DistanceFromProjToAim = targetCenter - Player.Center;
                        Vector2 DirectionFromProjToAim = DistanceFromProjToAim.SafeNormalize(Vector2.UnitX);
                        Vector2 speed = DirectionFromProjToAim * 8f;
                        for (int i = 0; i < ProjectileNum; i++)
                        {
                            Vector2 RotateSpeed = speed.RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (ProjectileNum - 1)));
                            Projectile.NewProjectile(entity, Player.Center, RotateSpeed, ModContent.ProjectileType<FriendlySlimeProjectile>(), 25, 1f, Player.whoAmI);
                            KSPcounter = 0;
                        }
                    }
                }
            }
        }

        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (KingSlimePower)
            {
                EntitySource_ItemUse entity = new EntitySource_ItemUse(Player, new Item(ModContent.ItemType<KingSlimeDelight>()));
                Vector2 DistanceFromProjToAim = Main.MouseWorld - Player.Center;
                Vector2 DirectionFromProjToAim = DistanceFromProjToAim.SafeNormalize(Vector2.UnitX);
                Vector2 speed = DirectionFromProjToAim * 20f;
                Projectile.NewProjectile(entity, Player.Center, speed, ModContent.ProjectileType<FriendlySlimeProjectile>(), (int)(damage * 0.35f), 1f, Player.whoAmI);
            }
        }
    }
}
