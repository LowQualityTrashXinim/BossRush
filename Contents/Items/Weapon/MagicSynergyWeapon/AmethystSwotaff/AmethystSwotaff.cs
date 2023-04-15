using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Runtime.CompilerServices;

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
            Item.BossRushDefaultMagic(60, 58, 20, 3f, 4, 20, ItemUseStyleID.Shoot, ModContent.ProjectileType<AmethystSwotaffP>(), 7, 10, false);
            Item.crit = 10;
            Item.reuseDelay = 20;
            Item.value = Item.buyPrice(gold: 50);
            Item.useTurn = false;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item8;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<AmethystSwotaffP>()] < 1;
        }
        public override void OnMissingMana(Player player, int neededMana)
        {
            if (player.statMana <= player.GetManaCost(Item))
            {
                CanShootProjectile = false;
            }
            player.statMana += neededMana;
        }
        bool CanShootProjectile = true;
        int countIndex = 1;
        int time = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI, countIndex);
            if (CanShootProjectile)
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
        public override bool AltFunctionUse(Player player)
        {
            return true;
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
            Projectile.width = 60;
            Projectile.height = 58;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
        }
        Vector2 PosToGo;
        int FirstFrame = 0;
        public override void AI()
        {
            if (Projectile.ai[0] != 1 || Projectile.ai[0] != -1)
            {
                SpinAtCursorAI();
                return;
            }
            BossRushUtils.ProjectileSwordSwingAI(Projectile, ref PosToGo, ref FirstFrame, (int)Projectile.ai[0]);
        }
        bool isAlreadyHeldDown = false;
        int countdownBeforeReturn = 0;
        int AbsoluteCountDown = 900;
        private void SpinAtCursorAI()
        {
            Player player = Main.player[Projectile.owner];
            if (FirstFrame == 0)
            {
                PosToGo = Main.MouseWorld;
                FirstFrame++;
            }
            Vector2 length = PosToGo - Projectile.Center;
            Projectile.velocity = length.SafeNormalize(Vector2.Zero) * length.Length() + player.velocity;
            if (Main.mouseRight && !isAlreadyHeldDown)
            {
                isAlreadyHeldDown = true;
            }
            if (isAlreadyHeldDown)
            {
                countdownBeforeReturn = 100;
            }
            if (Main.mouseRightRelease && isAlreadyHeldDown)
            {
                isAlreadyHeldDown = false;
            }
            countdownBeforeReturn -= countdownBeforeReturn > 0 ? 1 : 0;
            AbsoluteCountDown -= AbsoluteCountDown > 0 ? 1 : 0;
            if ((countdownBeforeReturn <= 0 && !isAlreadyHeldDown) || AbsoluteCountDown <= 0)
            {
                PosToGo = player.Center;
            }
            Projectile.rotation += MathHelper.ToRadians(20);
            SpawnDust();
            Vector2 velocity = Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(5, 7), Main.rand.NextFloat(5, 7));
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity, ProjectileID.AmethystBolt, (int)(Projectile.damage * .65f), Projectile.knockBack, Projectile.owner);
        }
        private void SpawnDust()
        {
            for (int i = 0; i < 3; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemAmethyst);
                Main.dust[dust].scale = Main.rand.NextFloat(.8f, 1.2f);
                Main.dust[dust].velocity = Main.rand.NextVector2Circular(5, 5);
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 6;
        }
    }
}
