using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.RuntimeDetour.HookGen;
using MonoMod.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BossRush.Common.Systems.ArtifactSystem
{
    internal class CharacterCreationIL : ILoadable
    {
        private const int ARTIFACT_SELECTION_UI_HEIGHT = 200;
        private const int TOP_PADDING = 10;

        private static ILHook buildPageHook;
        public void Load(Mod mod)
        {
            buildPageHook = new ILHook(
                typeof(UICharacterCreation).GetMethod("BuildPage", BindingFlags.NonPublic | BindingFlags.Instance),
                BuildPageIL
            );
        }

        public void Unload()
        {
            buildPageHook?.Dispose();
        }

        private static void BuildPageIL(ILContext context)
        {
            ILCursor cursor = new(context);

            if (!cursor.TryGotoNext(
                    MoveType.After,
                    instruction => instruction.MatchStfld<UIElement>("Width"),
                    instruction => instruction.MatchDup(),
                    instruction => instruction.MatchLdcI4(380)))
            {
                Logging.PublicLogger.Error($"{typeof(CharacterCreationIL).Name}: Couldn't find correct instruction.");
                return;
            }

			cursor.EmitLdcI4(ARTIFACT_SELECTION_UI_HEIGHT + TOP_PADDING);
			cursor.EmitAdd();

            if (!cursor.TryGotoNext(
                    MoveType.After,
                    instruction => instruction.MatchLdloc1(),
                    instruction => instruction.MatchLdflda<UIElement>("Height"),
                    instruction => instruction.MatchLdfld<StyleDimension>("Pixels"),
                    instruction => instruction.MatchLdcR4(150f)))
            {
                Logging.PublicLogger.Error($"{typeof(CharacterCreationIL).Name}: Couldn't find correct instruction.");
                return;
            }

			cursor.EmitLdcR4(ARTIFACT_SELECTION_UI_HEIGHT + TOP_PADDING);
			cursor.EmitAdd();

            if (!cursor.TryGotoNext(
                    MoveType.After,
                    instruction => instruction.MatchLdloc1(),
                    instruction => instruction.MatchLdloc2(),
                    instruction => instruction.MatchCallvirt<UIElement>("Append")))
            {
                Logging.PublicLogger.Error($"{typeof(CharacterCreationIL).Name}: Couldn't find correct instruction.");
                return;
            }

            FieldInfo playerFieldInfo = typeof(UICharacterCreation).GetField("_player", BindingFlags.NonPublic | BindingFlags.Instance);
            if (playerFieldInfo is null)
            {
                return;
            }

            cursor.EmitLdloc1();
            cursor.EmitLdarg0();
            cursor.EmitLdfld(playerFieldInfo);
			cursor.EmitDelegate<Action<UIElement, Player>>((element, player) =>
            {
                element.Append(new ArtifactSelectionUIPanel(player, ARTIFACT_SELECTION_UI_HEIGHT, 280 + TOP_PADDING));
            });
        }
    }
}
