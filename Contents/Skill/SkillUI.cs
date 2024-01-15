using Terraria;
using Terraria.UI;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;

namespace BossRush.Contents.Skill;
internal class SkillUI : UIState {
	public List<btn_SkillSlotHolder> skill = new List<btn_SkillSlotHolder>();
	public List<btn_SkillSlotHolder> inventory = new List<btn_SkillSlotHolder>();
	public ExitUI exitUI;
	public const string UItype_SKILL = "skill";
	public const string UIType_INVENTORY = "inventory";
	public override void OnActivate() {
		Player player = Main.LocalPlayer;
		if (player.TryGetModPlayer(out SkillHandlePlayer modplayer)) {
			//Explain : since most likely in the future we aren't gonna expand the skill slot, we just hard set it to 10
			//We are also pre render these UI first
			int[] SkillHolder = modplayer.GetCurrentActiveSkillHolder();
			Vector2 textureSize = new Vector2(52, 52);
			Vector2 OffSetPosition_Skill = player.Center;
			OffSetPosition_Skill.X -= textureSize.X * 5;
			if (skill.Count < 1) {
				for (int i = 0; i < 10; i++) {
					btn_SkillSlotHolder skillslot = new btn_SkillSlotHolder(TextureAssets.InventoryBack, player, i, SkillHolder[i], UItype_SKILL);
					skillslot.UISetPosition(OffSetPosition_Skill + new Vector2(52, 0) * i, textureSize);
					skill.Add(skillslot);
					Append(skill[i]);
				}
			}
			Vector2 InvOffSet = new Vector2(520, -55);
			if (inventory.Count < 1) {
				for (int i = 0; i < 30; i++) {
					btn_SkillSlotHolder skillslot = new btn_SkillSlotHolder(TextureAssets.InventoryBack, player, i, modplayer.SkillInventory[i], UIType_INVENTORY);
					Vector2 InvPos = OffSetPosition_Skill + new Vector2(0, 72);
					if (i >= 10) {
						InvPos -= InvOffSet;
					}
					if (i >= 20) {
						InvPos -= InvOffSet;
					}
					skillslot.UISetPosition(InvPos + new Vector2(52, 0) * i, textureSize);
					inventory.Add(skillslot);
					Append(inventory[i]);
				}
			}
			if (exitUI == null) {
				exitUI = new ExitUI(TextureAssets.InventoryBack10);
				exitUI.UISetPosition(player.Center + new Vector2(300, 0), textureSize);
				Append(exitUI);
			}
		}
	}
	public override void OnDeactivate() {
		SkillModSystem.SelectInventoryIndex = -1;
		SkillModSystem.SelectSkillIndex = -1;
	}
}
class SkillUIpannel : UIPanel {

}
class btn_SkillSlotHolder : UIImageButton {
	public int whoAmI = -1;
	public int sKillID = -1;
	public string uitype = "";
	Player player;
	Texture2D Texture;
	SkillHandlePlayer modplayer;
	public btn_SkillSlotHolder(Asset<Texture2D> texture, Player Tplayer, int WhoAmI, int SkillID, string UItype) : base(texture) {
		player = Tplayer;
		whoAmI = WhoAmI;
		sKillID = SkillID;
		Texture = texture.Value;
		uitype = UItype;
		modplayer = player.GetModPlayer<SkillHandlePlayer>();
	}
	public override void LeftClick(UIMouseEvent evt) {
		//Moving skill around in inventory
		if (uitype == SkillUI.UIType_INVENTORY) {
			if (SkillModSystem.SelectInventoryIndex == -1) {
				if (SkillModSystem.SelectSkillIndex == -1) {
					//This mean the player haven't select anything
					SkillModSystem.SelectInventoryIndex = whoAmI;
				}
				else {
					//Player are Attempting to move a skill from their skill slot back to inventory
					modplayer.ReplaceSkillFromSkillHolderToInv(SkillModSystem.SelectSkillIndex, whoAmI);
					SkillModSystem.SelectSkillIndex = -1;
				}
			}
			else {
				if (SkillModSystem.SelectSkillIndex == -1) {
					//Player are moving skill around their inventory
					int cache = modplayer.SkillInventory[whoAmI];
					modplayer.SkillInventory[whoAmI] = modplayer.SkillInventory[SkillModSystem.SelectInventoryIndex];
					modplayer.SkillInventory[SkillModSystem.SelectInventoryIndex] = cache;
					SkillModSystem.SelectInventoryIndex = -1;
				}
				//It is impossible where SelectSkillIndex can be other than -1
			}
		}
		else if (uitype == SkillUI.UItype_SKILL) {
			if (SkillModSystem.SelectSkillIndex == -1) {
				if (SkillModSystem.SelectInventoryIndex == -1) {
					//This mean the player haven't select anything
					SkillModSystem.SelectSkillIndex = whoAmI;
				}
				else {
					//Player are Attempting to move a skill from their inventory into a skill holder
					modplayer.ReplaceSkillFromInvToSkillHolder(whoAmI, SkillModSystem.SelectInventoryIndex);
					SkillModSystem.SelectInventoryIndex = -1;
				}
			}
			else {
				if (SkillModSystem.SelectInventoryIndex == -1) {
					//Player are moving skill around their skill holder
					int cache = modplayer.SkillInventory[whoAmI];
					modplayer.SkillInventory[whoAmI] = modplayer.SkillInventory[SkillModSystem.SelectSkillIndex];
					modplayer.SkillInventory[SkillModSystem.SelectSkillIndex] = cache;
					SkillModSystem.SelectSkillIndex = -1;
				}
			}
		}
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (uitype == SkillUI.UIType_INVENTORY) {
			if (modplayer.SkillInventory[whoAmI] != sKillID) {
				sKillID = modplayer.SkillInventory[whoAmI];
			}
		}
		else if (uitype == SkillUI.UItype_SKILL) {
			int[] skillholder = modplayer.GetCurrentActiveSkillHolder();
			if (skillholder[whoAmI] != sKillID) {
				sKillID = skillholder[whoAmI];
			}
		}
		if (IsMouseHovering) {
			string tooltipText = "";
			string Name = "";
			if (SkillLoader.GetSkill(sKillID) != null) {
				Name = SkillLoader.GetSkill(sKillID).DisplayName;
				tooltipText = SkillLoader.GetSkill(sKillID).Description;
			}
			Main.instance.MouseText(Name + "\n" + tooltipText);
		}
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		if (sKillID < 0 || sKillID >= SkillLoader.TotalCount) {
			return;
		}
		Vector2 drawpos = new Vector2(Left.Pixels, Top.Pixels) + Texture.Size() * .5f;
		Texture2D skilltexture = ModContent.Request<Texture2D>(SkillLoader.GetSkill(sKillID).Texture).Value;
		Vector2 origin = skilltexture.Size() * .5f;
		float scaling = ScaleCalculation(Texture.Size(), skilltexture.Size());
		spriteBatch.Draw(skilltexture, drawpos, null, Color.White, 0, origin, scaling, SpriteEffects.None, 0);
	}
	private float ScaleCalculation(Vector2 originalTexture, Vector2 textureSize) => originalTexture.Length() / (textureSize.Length() * 1.5f);
}
