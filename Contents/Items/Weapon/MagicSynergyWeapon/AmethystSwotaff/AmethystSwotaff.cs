using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Global;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.AmethystSwotaff
{
    internal class AmethystSwotaff : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("You know, if this is all what it do\nthen it would be pretty disappointing\nluckily, you can yeet the thing");
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.BossRushDefaultMagic(60, 58, 20, 3f, 20, 20, ItemUseStyleID.Swing, ModContent.ProjectileType<AmethystSwotaffP>(), 7, 10, false);
            Item.crit = 10;
            Item.value = Item.buyPrice(gold: 50);
            Item.useTurn = false;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item8;
            Item.noUseGraphic = true;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<AmethystSwotaffP>()] < 1;
        }
        public override void OnMissingMana(Player player, int neededMana)
        {
            if (player.statMana <= player.GetManaCost(Item))
            {
                CanShootProjectile = -1;
            }
            player.statMana += neededMana;
        }
        int CanShootProjectile = 1;
        int countIndex = 1;
        int time = 1;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.statMana >= player.GetManaCost(Item))
            {
                CanShootProjectile = 1;
            }
            Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI, countIndex, CanShootProjectile);
            if (CanShootProjectile == 1)
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileID.AmethystBolt, damage, knockback, player.whoAmI);
            }
            if (countIndex == 1)
            {
                countIndex = -1;
            }
            else
            {
                countIndex = 1;
            }
            time++;
            if (time >= 3)
            {
                countIndex = 2;
                time = 0;
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CopperBroadsword)
                .AddIngredient(ItemID.AmethystStaff)
                .Register();
        }
    }
    public class AmethystSwotaffP : ModProjectile
    {
        public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<AmethystSwotaff>();
        public override void SetDefaults()
        {
            Projectile.width = 70;
            Projectile.height = 70;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
        }
        Vector2 PosToGo;
        int FirstFrame = 0;
        public override void AI()
        {
            if (Projectile.ai[0] == 1 || Projectile.ai[0] == -1)
            {
                BossRushUtils.ProjectileSwordSwingAI(Projectile, ref PosToGo, ref FirstFrame, (int)Projectile.ai[0]);
                return;
            }
            SpinAtCursorAI();
        }
        bool isAlreadyHeldDown = false;
        bool isAlreadyReleased = false;
        int countdownBeforeReturn = 100;
        int AbsoluteCountDown = 420;
        int timeToSpin = 0;
        private void SpinAtCursorAI()
        {
            Player player = Main.player[Projectile.owner];
            Item item = player.HeldItem;
            if (FirstFrame == 0)
            {
                PosToGo = Main.MouseWorld;
                FirstFrame++;
            }
            Vector2 length = PosToGo - Projectile.Center;
            if (Main.mouseLeft && !isAlreadyHeldDown && !isAlreadyReleased)
            {
                isAlreadyHeldDown = true;
            }
            if (isAlreadyHeldDown)
            {
                countdownBeforeReturn = 10;
            }
            if (!Main.mouseLeft && Main.mouseLeftRelease && isAlreadyHeldDown)
            {
                isAlreadyHeldDown = false;
                isAlreadyReleased = true;
            }
            countdownBeforeReturn -= countdownBeforeReturn > 0 ? 1 : 0;
            AbsoluteCountDown -= AbsoluteCountDown > 0 ? 1 : 0;
            if (countdownBeforeReturn <= 0 || AbsoluteCountDown <= 0 || item.type != ModContent.ItemType<AmethystSwotaff>())
            {
                length = player.Center - Projectile.Center;
                float distanceTo = length.Length();
                if (distanceTo < 60)
                {
                    Projectile.Kill();
                }
            }
            Projectile.velocity = length.SafeNormalize(Vector2.Zero) * length.Length() + player.velocity;
            Projectile.velocity = Projectile.velocity.LimitedVelocity(20);
            Projectile.rotation += MathHelper.ToRadians(15);
            int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemAmethyst);
            Main.dust[dust].scale = Main.rand.NextFloat(.8f, 1.2f);
            Main.dust[dust].velocity = Main.rand.NextVector2Circular(5, 5);
            Main.dust[dust].noGravity = true;
            Vector2 velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * Main.rand.NextFloat(6, 9);
            if (Projectile.ai[1] == 1)
            {
                if (timeToSpin >= 24)
                {
                    if (player.CheckMana(player.GetManaCost(player.HeldItem), true))
                    {
                        player.statMana -= player.GetManaCost(player.HeldItem);
                    }
                    else
                    {
                        Projectile.ai[1] = -1;
                    }
                    timeToSpin = 0;
                }
                timeToSpin++;
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.PositionOFFSET(velocity, 50), velocity, ProjectileID.AmethystBolt, (int)(Projectile.damage * .55f), Projectile.knockBack, Projectile.owner);
                Main.projectile[proj].timeLeft = 30;
            }
            if ((Projectile.Center - player.Center).LengthSquared() > 1000 * 1000)
            {
                Projectile.Kill();
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 6;
        }
    }
}
