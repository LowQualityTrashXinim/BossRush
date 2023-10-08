using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BossRush.Contents.Items.Potion
{
    internal class WonderDrug : ModItem
    {
        //HP
        public const int DrugHP = 20;
        public const int DrugHP2 = 10;
        public const int DrugHP3 = 0;
        public const int DrugHP4 = -10;
        //def
        public const int DrugDef1 = 5;
        public const int DrugDef2 = 2;
        public const int DrugDef3 = 1;
        //Regen
        public const int DrugRegen = 1;
        //Time
        public int CountTimeConsume = 0;
        //Speed
        public const float DrugSpeed1 = 0.20f;
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LifeFruit);
        }

        public override bool CanUseItem(Player player)
        {
            return player.statLifeMax2 > 100;
        }

        public override bool? UseItem(Player player)
        {
            // Do not do this: player.statLifeMax += 2;
            switch (CountTimeConsume)
            {
                case 0:
                    break;
                case 1:
                    player.statLifeMax2 += DrugHP;
                    player.statLife += DrugHP;
                    if (Main.myPlayer == player.whoAmI)
                    {
                        player.HealEffect(DrugHP);
                    }
                    break;
                case 2:
                    player.statLifeMax2 += DrugHP2;
                    player.statLife += DrugHP2;
                    if (Main.myPlayer == player.whoAmI)
                    {
                        player.HealEffect(DrugHP2);
                    }
                    break;
                case 3:
                    player.statLifeMax2 += DrugHP3;
                    player.statLife += DrugHP3;
                    if (Main.myPlayer == player.whoAmI)
                    {
                        player.HealEffect(DrugHP3);
                    }
                    break;
                default:
                    player.statLifeMax2 += DrugHP4;
                    player.statLife += DrugHP4;
                    if (Main.myPlayer == player.whoAmI)
                    {
                        player.HealEffect(DrugHP3);
                    }
                    break;
            }
            ++player.GetModPlayer<WonderDrugPlayer>().DrugDealer;
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Vitamins)
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }

    public class WonderDrugPlayer : ModPlayer
    {
        public int DrugDealer = 0;
        //Damage
        public const int DrugDamage = 10;
        public const int DrugDamage2 = 5;
        public const int DrugDamage3 = 1;
        //MoveSpeed

        public override void ResetEffects()
        {
            switch (DrugDealer)
            {
                case 0:
                    break;
                case 1:
                    Player.statLifeMax2 += WonderDrug.DrugHP;
                    Player.GetDamage(DamageClass.Generic).Flat += DrugDamage;
                    Player.statDefense += WonderDrug.DrugDef1;
                    Player.lifeRegen += DrugDealer * WonderDrug.DrugRegen;
                    Player.lifeRegenCount += DrugDealer * WonderDrug.DrugRegen;
                    Player.lifeRegenTime += DrugDealer * WonderDrug.DrugRegen;
                    Player.accRunSpeed += DrugDealer * WonderDrug.DrugSpeed1;
                    break;
                case 2:
                    Player.statLifeMax2 += WonderDrug.DrugHP2 + WonderDrug.DrugHP;
                    Player.GetDamage(DamageClass.Generic).Flat += DrugDamage2 + DrugDamage;
                    Player.statDefense += WonderDrug.DrugDef2 + WonderDrug.DrugDef1;
                    Player.lifeRegen += DrugDealer * WonderDrug.DrugRegen;
                    Player.lifeRegenCount += DrugDealer * WonderDrug.DrugRegen;
                    Player.lifeRegenTime += DrugDealer * WonderDrug.DrugRegen;
                    Player.accRunSpeed += DrugDealer * WonderDrug.DrugSpeed1;
                    break;
                case 3:
                    Player.statLifeMax2 += WonderDrug.DrugHP2 + WonderDrug.DrugHP;
                    Player.GetDamage(DamageClass.Generic).Flat += DrugDamage3 + DrugDamage2 + DrugDamage;
                    Player.statDefense += WonderDrug.DrugDef3 + WonderDrug.DrugDef2 + WonderDrug.DrugDef1;
                    Player.lifeRegen += DrugDealer * WonderDrug.DrugRegen;
                    Player.lifeRegenCount += DrugDealer * WonderDrug.DrugRegen;
                    Player.lifeRegenTime += DrugDealer * WonderDrug.DrugRegen;
                    Player.accRunSpeed += DrugDealer * WonderDrug.DrugSpeed1;
                    break;
                default:
                    Player.statLifeMax2 = Math.Clamp((DrugDealer - 3) * WonderDrug.DrugHP4 + WonderDrug.DrugHP2 + WonderDrug.DrugHP + Player.statLifeMax2, 100, 9999999);
                    Player.GetDamage(DamageClass.Generic).Flat += (DrugDealer - 3) * DrugDamage3 + DrugDamage2 + DrugDamage;
                    Player.statDefense += DrugDealer * WonderDrug.DrugDef3 + WonderDrug.DrugDef2 + WonderDrug.DrugDef1;
                    Player.lifeRegen += DrugDealer * WonderDrug.DrugRegen;
                    Player.lifeRegenCount += DrugDealer * WonderDrug.DrugRegen;
                    Player.lifeRegenTime += DrugDealer * WonderDrug.DrugRegen;
                    Player.accRunSpeed += DrugDealer < 20 ? DrugDealer * WonderDrug.DrugSpeed1 : (20 + (DrugDealer - 20) * .5f) * WonderDrug.DrugSpeed1;
                    break;
            }
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)BossRush.MessageType.DrugSyncPlayer);
            packet.Write((byte)Player.whoAmI);
            packet.Write(DrugDealer);
            packet.Send(toWho, fromWho);
        }
        public override void Initialize()
        {
            DrugDealer = 0;
        }
        public override void SaveData(TagCompound tag)
        {
            tag["Drug"] = DrugDealer;
        }

        public override void LoadData(TagCompound tag)
        {
            DrugDealer = (int)tag["Drug"];
        }
        public void ReceivePlayerSync(BinaryReader reader)
        {
            DrugDealer = reader.ReadByte();
        }

        public override void CopyClientState(ModPlayer targetCopy)
        {
            WonderDrugPlayer clone = (WonderDrugPlayer)targetCopy;
            clone.DrugDealer = DrugDealer;
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            WonderDrugPlayer clone = (WonderDrugPlayer)clientPlayer;
            if (DrugDealer != clone.DrugDealer) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
        }
    }
}