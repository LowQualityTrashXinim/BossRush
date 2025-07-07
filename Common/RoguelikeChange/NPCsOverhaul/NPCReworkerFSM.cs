using BossRush.Common.Systems;
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
public class NPCReworkerFSM : GlobalNPC {

	public override bool InstancePerEntity => true;
	public virtual int VanillaNPCType => 0;
	public NPCAIStates states = null;

	public override void SetStaticDefaults() {
		NPCID.Sets.TrailingMode[VanillaNPCType] = 3;
		NPCID.Sets.TrailCacheLength[VanillaNPCType] = 30;
	}
	public override void SetDefaults(NPC entity) {
		entity.aiStyle = -1;
	}

	public override void OnSpawn(NPC npc, IEntitySource source) {
		states = new(npc,RegisterStates());
		PostOnSpawn(npc, source);
	}

	public virtual void PostOnSpawn(NPC npc, IEntitySource source)
	{
		
	}

	public virtual int[] RegisterStates()
	{
		return new int[0];
	}

	public override bool PreAI(NPC npc) {
		UpdateAnimation(npc);
		ReworkedAI(ref npc);
		return false;

	}


	public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
		return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
	}
	public virtual void ReworkedAI(ref NPC npc) 
	{
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
	public Rectangle frameRect = new();
	public override void FindFrame(NPC npc, int frameHeight) 
	{
		if(UseCustomAnimation())
			frameRect = new Rectangle(0,currentFrame * this.frameHeight, TextureAssets.Npc[VanillaNPCType].Width(),this.frameHeight);
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
		if(UseCustomAnimation())
			return new DrawData(TextureAssets.Npc[VanillaNPCType].Value,npc.Center - screenPos,frameRect,drawColor,npc.rotation,new Vector2(TextureAssets.Npc[VanillaNPCType].Width() / 2f,frameHeight / 2f),npc.scale,npc.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
		return new DrawData(TextureAssets.Npc[VanillaNPCType].Value,npc.Center - screenPos,npc.frame,drawColor,npc.rotation,new Vector2(TextureAssets.Npc[VanillaNPCType].Width() / 2f,frameHeight / 2f),npc.scale,npc.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
	
	}

}


