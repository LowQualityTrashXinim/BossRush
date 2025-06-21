using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
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
	}
		
	//reuseable general purpose fields
	public bool justHitTheGround = false;
	public bool inGround = false;
	public bool justMovedAwayFromTheGround = false;
	public bool afterimages = false;
	public int Timer = 0;
	public int Delay = 0;
	public int Delay2 = 0;
	public int Counter = 0;
	public int Counter2 = 0;
	public int Counter3 = 0;
	public int Counter4 = 0;
	public int AIState = 0;
	public bool NeedsNetUpdate = false;

	public override bool PreAI(NPC npc) {
		npc.TargetClosest();

		UpdateAnimation(npc);

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
			npc.velocity = Vector2.Zero;
		}

		Timer++;
		
		if(NeedsNetUpdate)
		{

			npc.netUpdate = true;
			NeedsNetUpdate = false;

		}

		return false;

	}


	public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {

		DrawAfterimages(npc, spriteBatch, screenPos, drawColor);

		return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
	}
	public virtual void ReworkedAI(ref NPC npc, Player target) 
	{
	}
	public static void ClearOldCache(NPC npc)
	{ 
	
			Array.Clear(npc.oldPos);
			Array.Clear(npc.oldRot);

	
	}
	public static int NewProjectileWithMPCheck(IEntitySource spawnSource, Vector2 position, Vector2 velocity, int Type, int Damage, float KnockBack, int Owner = -1, float ai0 = 0f, float ai1 = 0f, float ai2 = 0f)
	{
		if(Main.netMode != NetmodeID.MultiplayerClient)
			return Projectile.NewProjectile(spawnSource,position,velocity,Type,Damage,KnockBack,Owner,ai0,ai1,ai2);
		else
			return -1;
	}
	public static int NewNPCWithMPCheck(IEntitySource source, Vector2 Position, int Type, int Start = 0, float ai0 = 0f, float ai1 = 0f, float ai2 = 0f, float ai3 = 0f, int Target = 255)
	{
		if(Main.netMode != NetmodeID.MultiplayerClient)
			return NPC.NewNPC(source,(int)Position.X,(int)Position.Y,Type,Start,ai0,ai1,ai2,ai3,Target);
		else
			return -1;
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
	// This uses custom animation system instead of the weird vanilla system, this aims to be faster and easier to setup for npc reworking purpose
	public virtual bool UseCustomAnimation() => false;
	public int currentFrame = 0;
	public virtual int frameHeight => 128;
	public virtual int startingFrame => 0;
	public virtual int animationSpeed => 7;
	public virtual int maxFrames => 0;
	public override void FindFrame(NPC npc, int frameHeight) 
	{
		if(UseCustomAnimation())
			npc.frame = new Rectangle(0,currentFrame * this.frameHeight, TextureAssets.Npc[VanillaNPCType].Width(),this.frameHeight);
		else
			base.FindFrame(npc, frameHeight);
	}
	public void UpdateAnimation(NPC npc)
	{
		if(++npc.frameCounter % animationSpeed == 0)
			if(++currentFrame > maxFrames - startingFrame)
				currentFrame = startingFrame;
	}
	public DrawData NPCSpriteDrawData(NPC npc, Color drawColor, Vector2 screenPos)
	{
	
		return new DrawData(TextureAssets.Npc[VanillaNPCType].Value,npc.Center - screenPos,npc.frame,drawColor,npc.rotation,new Vector2(TextureAssets.Npc[VanillaNPCType].Width() / 2f,frameHeight / 2f),npc.scale,npc.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
	
	}
}
