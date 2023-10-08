using BossRush.Common.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace BossRush.Common.Systems.ArtifactSystem
{
    public abstract class Artifact : ModType
    {
        public static List<Artifact> AllArtifacts { get; private set; }
        public int Type { get; private set; }
        public virtual string TexturePath => (GetType().Namespace + "." + Name).Replace('.', '/');
        public Asset<Texture2D> Texture { get; private set; }
        public string DisplayName => Language.GetTextValue($"Mods.BossRush.Artifacts.{Name}.DisplayName");
        public string Description => Language.GetTextValue($"Mods.BossRush.Artifacts.{Name}.Description");
        public virtual Color DisplayNameColor => Color.White;
        public virtual float Scale => 1f;
        public virtual int Frames => 1;
		public virtual bool CanBeSelected(Player player) => true;
		private int animationTimer;
		public virtual void DrawInUI(SpriteBatch spriteBatch, CalculatedStyle dimensions)
        {
            Rectangle source = Texture.Value.GetSource(Frames, animationTimer++ / 6);
            spriteBatch.Draw(
                Texture.Value,
                dimensions.Center(),
                source,
                Color.White,
                0f,
                source.Size() / 2f,
                Scale * (dimensions.Height - 12f) / source.Height,
                SpriteEffects.None,
                0f
            );
        }

        protected sealed override void Register()
        {
            AllArtifacts ??= new List<Artifact>();

            Type = AllArtifacts.Count;
            Texture = ModContent.Request<Texture2D>(TexturePath);
            AllArtifacts.Add(this);
        }

        public static int ArtifactType<T>() where T : Artifact
        {
            return AllArtifacts.FirstOrDefault(artifact => artifact is T).Type;
        }

        public static Artifact GetArtifact(int type)
        {
            return type > 0 && type < AllArtifacts.Count ? AllArtifacts[type] : null;
        }
    }
}
