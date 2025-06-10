using BossRush.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace BossRush.Contents.NPCs.TrialNPC;
public abstract class BaseTrialNPC : ModNPC {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public virtual void TrialNPCDefaults() {

	}
	public sealed override void SetDefaults() {
		base.SetDefaults();
	}
}
