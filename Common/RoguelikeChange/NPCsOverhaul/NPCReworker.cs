using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace BossRush.Common.RoguelikeChange.NPCsOverhaul;
public class NPCReworker : GlobalNPC {

	public override bool InstancePerEntity => true;
	public virtual int VanillaNPCType => 0;

	public override void SetStaticDefaults() {
		NPCID.Sets.TrailingMode[VanillaNPCType] = 3;
		NPCID.Sets.TrailCacheLength[VanillaNPCType] = 30;
	}


	public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter) {
		binaryWriter.Write(Timer);
		binaryWriter.Write(Delay);
		binaryWriter.Write(Delay2);
		binaryWriter.Write(Counter);
		binaryWriter.Write(Counter2);
		binaryWriter.Write(Counter3);
		binaryWriter.Write(Counter4);
		binaryWriter.Write(AIState);
		binaryWriter.Write(NeedsNetUpdate);
	}

	public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader) {
		Timer = binaryReader.ReadInt32();
		Delay = binaryReader.ReadInt32();
		Delay2 = binaryReader.ReadInt32();
		Counter = binaryReader.ReadInt32();
		Counter2 = binaryReader.ReadInt32();
		Counter3 = binaryReader.ReadInt32();
		Counter4 = binaryReader.ReadInt32();
		AIState = binaryReader.ReadInt32();
		NeedsNetUpdate = binaryReader.ReadBoolean();
		
	}

	//reuseable general purpose fields
	internal bool justHitTheGround = false;
	internal bool inGround = false;
	internal bool justMovedAwayFromTheGround = false;
	internal bool afterimages = false;
	internal int Timer = 0;
	internal int Delay = 0;
	internal int Delay2 = 0;
	internal int Counter = 0;
	internal int Counter2 = 0;
	internal int Counter3 = 0;
	internal int Counter4 = 0;
	internal int AIState = 0;
	internal bool NeedsNetUpdate = false;
	public override bool PreAI(NPC npc) {
		npc.TargetClosest();
		afterimages = false;
		if(Delay > 0)
			Delay--;
		if(Delay2 > 0)
			Delay2--;

		if (justHitTheGround) 
		{
			justHitTheGround = false;

		}

		if (npc.collideY && !inGround) 
		{
		
			justHitTheGround = true;
			inGround = true;
		}

		if(justMovedAwayFromTheGround) 
		{
			justMovedAwayFromTheGround = false;
		}

		if(!npc.collideY && inGround) 
		{
			inGround = false;
			justMovedAwayFromTheGround = true;
		}

		if(npc.HasValidTarget)
		{
			ReworkedAI(ref npc, Main.player[npc.target]);
		}
		else
		{
			npc.EncourageDespawn(2);
			npc.position.Y += 5;
		}

		if(NeedsNetUpdate)
		{

			npc.netUpdate = true;
			NeedsNetUpdate = false;

		}

		Timer++;

		return false;

	}


	public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {

		DrawAfterimages(npc, spriteBatch, screenPos, drawColor);

		return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
	}
	public virtual void ReworkedAI(ref NPC npc, Player target) 
	{
		
		
		
	}

	public void DrawAfterimages(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
	
	
		if (afterimages) 
		{
			for (int i = npc.oldPos.Length - 1; i > 0; i--) 
			{

				spriteBatch.Draw(TextureAssets.Npc[VanillaNPCType].Value, npc.oldPos[i] - screenPos + npc.Hitbox.Size() / 2f, npc.frame, drawColor * 0.1f , npc.oldRot[i],npc.frame.Size() / 2f, npc.scale, npc.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,0f);

			}
		}
	}

	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		return entity.type == VanillaNPCType;
	}

}
