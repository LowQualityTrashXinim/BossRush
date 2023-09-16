using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using BossRush.Contents.BuffAndDebuff;

namespace BossRush.Contents.Artifact
{
    internal class FateDecider : ArtifactModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void ArtifactSetDefault()
        {
            width = height = 32;
            Item.rare = ItemRarityID.Cyan;
        }
    }
    class FateDeciderPlayer : ArtifactPlayerHandleLogic
    {
        bool FateDice = false;
        bool IsBuffCurrentlyActive = false;
        public int GoodBuffIndex = -1;
        public int BadBuffIndex = -1;
        public override void ResetEffects()
        {
            FateDice = ArtifactDefinedID == ModContent.ItemType<FateDecider>();
        }
        public override void PostUpdate()
        {
            if (FateDice)
            {
                FateDeciderEffect();
            }
        }
        private void FateDeciderEffect()
        {
            if (Player.HasBuff(ModContent.BuffType<FateDeciderBuff>()))
            {
                IsBuffCurrentlyActive = true;
            }
            else
            {
                Player.AddBuff(ModContent.BuffType<FateDeciderBuff>(), 18000);
                IsBuffCurrentlyActive = false;
            }
            if (IsBuffCurrentlyActive)
            {
                if (GoodBuffIndex == -1)
                    GoodBuffIndex = Main.rand.Next(0, 6);
                if (BadBuffIndex == -1)
                    do
                    {
                        BadBuffIndex = Main.rand.Next(0, 6);
                    }
                    while (BadBuffIndex == GoodBuffIndex);
            }
            else
            {
                GoodBuffIndex = -1;
                BadBuffIndex = -1;
            }
        }
        public string BadEffectString()
        {
            switch (BadBuffIndex)
            {
                case 0:
                    return "Your arrow is worse";
                case 1:
                    return "Your shot appear at random and is shot everywhere";
                case 2:
                    return "Your sword's projectiles appear at wrong place and fly at wrong velocity";
                case 3:
                    return "Your magic weapon also use life as well";
                case 4:
                    return "Your minion attack may cause some negative spirit to chase after you";
                case 5:
                    return "Sword are significantly slower";
                default:
                    return "No current bad effect are active";
            }
        }
        public string GoodEffectString()
        {
            switch (GoodBuffIndex)
            {
                case 0:
                    return "You shoot the same type arrow that have ability to home into enemy";
                case 1:
                    return "You shoot the same type of bullet that have can accelerate";
                case 2:
                    return "Melee weapon that shoot projectile can now shoot extra projectiles";
                case 3:
                    return "Your magic weapon summon a extra magic projectile";
                case 4:
                    return "Your minion can randomly heal you";
                case 5:
                    return "Hitting a enemy with the sword will release a slash";
                default:
                    return "No current good effect are active";
            }
        }
        public override float UseSpeedMultiplier(Item item)
        {
            if (BadBuffIndex == 5 && item.DamageType == DamageClass.Melee && !item.noMelee)
                return .5f;
            return base.UseSpeedMultiplier(item);
        }
        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (item.useAmmo == AmmoID.Arrow)
            {
                if (BadBuffIndex == 0)
                {
                    type = ProjectileID.WoodenArrowFriendly;
                    velocity *= .5f;
                    damage = (int)(damage * .5f);
                }
            }
            if (item.useAmmo == AmmoID.Bullet)
            {
                if (BadBuffIndex == 1)
                {
                    velocity = velocity.Vector2RotateByRandom(360);
                    position = position.PositionOFFSET(velocity, Main.rand.NextFloat(-500, 500));
                }
            }
            if (!item.noMelee && !item.noUseGraphic && item.DamageType == DamageClass.Melee)
            {
                if (BadBuffIndex == 2)
                {
                    velocity = -velocity;
                    position = Player.Center + Player.Center - Main.MouseWorld;
                }
            }
        }
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.useAmmo == AmmoID.Arrow)
            {
                if (GoodBuffIndex == 0)
                {
                    velocity *= 2;
                    int proj = Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(10), type, damage, knockback, Player.whoAmI);
                    if (Main.projectile[proj].ModProjectile is null)
                    {
                        Main.projectile[proj].aiStyle = -1;
                        Main.projectile[proj].ai[0] = -1;
                        Main.projectile[proj].ai[1] = AmmoID.Arrow;
                    }
                }
            }
            if (item.useAmmo == AmmoID.Bullet)
            {
                if (GoodBuffIndex == 1)
                {
                    int proj = Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(10) * .25f, type, damage, knockback, Player.whoAmI);
                    if (Main.projectile[proj].ModProjectile is null)
                    {
                        Main.projectile[proj].aiStyle = -1;
                        Main.projectile[proj].ai[0] = -1;
                        Main.projectile[proj].ai[1] = AmmoID.Bullet;
                    }
                }
            }
            if (!item.noMelee && !item.noUseGraphic && item.DamageType == DamageClass.Melee)
            {
                if (GoodBuffIndex == 2)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 newpos = position.PositionOFFSET(velocity.Vector2DistributeEvenly(3, 45, i), -90);
                        int proj = Projectile.NewProjectile(source, newpos, (Main.MouseWorld - newpos).SafeNormalize(Vector2.Zero) * item.shootSpeed, type, damage, knockback, Player.whoAmI);
                        if (Main.projectile[proj].ModProjectile is null)
                        {
                            Main.projectile[proj].penetrate = 1;
                        }
                    }
                }
            }
            if (item.mana > 0 && item.DamageType == DamageClass.Magic)
            {
                if (GoodBuffIndex == 3)
                {
                    Vector2 newpos = Main.rand.NextVector2Circular(100, 100);
                    Projectile.NewProjectile(source, newpos, (Main.MouseWorld - newpos).SafeNormalize(Vector2.Zero) * item.shootSpeed, type, damage, knockback, Player.whoAmI);
                }
                if (BadBuffIndex == 3)
                {
                    Player.statLife -= item.mana;
                }
            }
            return base.Shoot(item, source, position, velocity, type, damage, knockback);
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPCWithItem(item, target, hit, damageDone);
            if (GoodBuffIndex == 5)
            {
                Vector2 pos = target.Center - Main.rand.NextVector2CircularEdge(100, 100);
                Projectile.NewProjectile(Player.GetSource_ItemUse(item), pos, (target.Center - pos).SafeNormalize(Vector2.Zero) * 3f, ModContent.ProjectileType<ArtifactSwordSlashProjectile>(), item.damage, 0, Player.whoAmI);
            }
        }
    }
}