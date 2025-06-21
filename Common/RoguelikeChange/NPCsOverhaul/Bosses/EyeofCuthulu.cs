using BossRush.Common.Graphics;
using BossRush.Common.Graphics.Structs.QuadStructs;
using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.IO;
using static tModPorter.ProgressUpdate;

namespace BossRush.Common.RoguelikeChange.NPCsOverhaul.Bosses;
public class EyeofCuthulu : NPCReworker {
	public override int VanillaNPCType => NPCID.EyeofCthulhu;
	ref int isPhase2 => ref Counter4;
	enum State : byte 
	{
		Idle,
		LongDash,
		MiniDash,
		DashMinions,
		IdleMinions,
		goingPhase2
	}

	State state
	{
	
	get {  return (State)AIState; }

	set
	{
		
		AIState = (int)value;

	}

	}

	public override int frameHeight => 996 / 6;
	public override int startingFrame => isPhase2 == 0 ? 0 : 3;
	public override int maxFrames => 2;

	// IDLE STATE FIELDS
	float Accel = 0;
	float targetSpeed = 0;
	float mag = 0;
	const float MAXSPEED = 10;
	public override void ReworkedAI(ref NPC npc, Player target) {
		
		Accel = Main.masterMode ? 0.015f : Main.expertMode ? 0.01f : 0.0075f;

		switch(state)
		{

			case State.Idle:
			{
				npc.rotation = npc.rotation.AngleTowards(npc.DirectionTo(target.Center).ToRotation() - MathHelper.PiOver2,MathHelper.Pi / 60f);

				if(++Counter < 120)
				{
					npc.velocity.RotatedBy(npc.DirectionTo(target.Center).ToRotation());
					npc.velocity *= 0.96f;

				}else
				{


					float d = npc.Distance(target.Center);
					targetSpeed = d * Accel;

					

				
					if(d > 32)
						npc.velocity = (npc.rotation + MathHelper.PiOver2).ToRotationVector2() * targetSpeed;


					if(Counter >= 300)
					{

						state = State.MiniDash;
						Counter = 0;
					}
				}

			
				break;
			}

			case State.MiniDash:
			{
				float dashspeed = Main.masterMode ? 30f : Main.expertMode ? 20f : 15f;
				if(++Counter < 45)
				{
					npc.velocity *= 0.90f;
					npc.rotation = npc.rotation.AngleTowards(target.DirectionFrom(npc.Center).ToRotation() - MathHelper.PiOver2,MathHelper.Pi / 120f);
					ClearOldCache(npc);
				}
				else{
					afterimages = true;
					npc.velocity = (npc.rotation + MathHelper.PiOver2).ToRotationVector2() * dashspeed;
				}
				if(Counter >= 60)
				{
					if(Main.netMode != NetmodeID.MultiplayerClient){
						bool hasMinion = false;
						foreach(NPC n in Main.ActiveNPCs)
							if(n.type == NPCID.ServantofCthulhu)
								hasMinion = true;
						if(!hasMinion)
							for(int i = 8; i > 0; i--)
							{
								NPC.NewNPCDirect(npc.GetSource_FromAI(),npc.Center,NPCID.ServantofCthulhu,0).velocity = Vector2.UnitX.RotatedBy(i / 8f * MathHelper.TwoPi) * 5;
							}
					}
					state = State.Idle;
					Counter = 0;
				}
				break;
			}
		
		}	
	
	}
	public override bool UseCustomAnimation() => true;

	public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
		
		DrawAfterimages(npc,spriteBatch,screenPos,drawColor);
		NPCSpriteDrawData(npc,drawColor,screenPos).Draw(spriteBatch);

		return false;
	}
}

public class ServerntOfCuthulu : NPCReworker 
{
	public override int VanillaNPCType => NPCID.ServantofCthulhu;
	public override void ReworkedAI(ref NPC npc, Player target) {
	
		switch(state)
		{
			case State.Following:
			{

				float accel = Main.masterMode ? 0.015f : Main.expertMode ? 0.01f : 0.0075f;
				float d = npc.Distance(target.Center);
				float targetSpeed = d * accel;

				npc.rotation = npc.
				npc.velocity = npc.rotation.ToRotationVector2() * targetSpeed;	
				
				break;
			}
		}
		

	}
	enum State {

		Following,
		Dashing,
		ChargingUp
	}

	State state
	{
	
	get { return (State)AIState; }

	set {
		
		AIState = (int)value;
	}
	}
	public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
		if(state == State.ChargingUp)
		{
		
			ShaderSettings shaderSettings = new ShaderSettings();
			shaderSettings.image1 = TextureAssets.Extra[193];
			shaderSettings.image2 = null;
			shaderSettings.image3 = null;
			shaderSettings.Color = Color.Red;
			shaderSettings.shaderData = new Vector4(0.5f, 500, 0, 0);
			default(ColorIndicatorQuad).Draw(npc.Center, npc.rotation - MathHelper.PiOver2, new Vector2(1000, 256), shaderSettings);


		}
	return true;
	}
	public override bool UseCustomAnimation() {
		return false;
	}
}


