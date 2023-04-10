using System;
using Terraria;
using Terraria.ID;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush
{
    public static partial class BossRushUtils
    {
        public static void ResetMinion(this Projectile projectile, Vector2 position, float distance)
        {
            if (CompareSquareFloatValue(projectile.Center, position, distance))
            {
                return;
            }
            projectile.position = position;
            projectile.velocity *= 0.1f;
            projectile.netUpdate = true;
        }
        public static void IdleFloatMovement(this Projectile projectile, Player player, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition, int index = 0)
        {
            Vector2 idlePosition = player.Center;
            float minionPositionOffsetX = (30 + index * 40) * -player.direction;
            idlePosition.X += minionPositionOffsetX;
            vectorToIdlePosition = idlePosition - projectile.Center;
            distanceToIdlePosition = vectorToIdlePosition.Length();
            projectile.ResetMinion(player.Center, 1500);
            float overlapVelocity = 0.04f;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];
                if (i != projectile.whoAmI 
                    && other.active 
                    && other.owner == projectile.owner 
                    && Math.Abs(projectile.position.X - other.position.X) + Math.Abs(projectile.position.Y - other.position.Y) < projectile.width)
                {
                    if (projectile.position.X < other.position.X)
                    {
                        projectile.velocity.X -= overlapVelocity;
                    }
                    else
                    {
                        projectile.velocity.X += overlapVelocity;
                    }

                    if (projectile.position.Y < other.position.Y)
                    {
                        projectile.velocity.Y -= overlapVelocity;
                    }
                    else
                    {
                        projectile.velocity.Y += overlapVelocity;
                    }
                }
            }
        }
        public static void MoveToIdle(this Projectile projectile, Vector2 vectorToIdlePosition, float distanceToIdlePosition, float speed, float inertia)
        {
            if (distanceToIdlePosition > 20f)
            {
                // The immediate range around the player (when it passively floats about)

                // This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
                vectorToIdlePosition.Normalize();
                vectorToIdlePosition *= speed;
                projectile.velocity = (projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
            }
            else if (projectile.velocity == Vector2.Zero)
            {
                // If there is a case where it's not moving at all, give it a little "poke"
                projectile.velocity.X = -0.15f;
                projectile.velocity.Y = -0.05f;
            }
        }
        public static void MoveToIdle(this Projectile projectile, Vector2 vectorToIdlePosition, float speed, float inertia, bool disablepoke = false)
        {
            float disToIdle = vectorToIdlePosition.LengthSquared();
            if (disToIdle > 20 * 20)
            {
                // The immediate range around the player (when it passively floats about)

                // This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
                vectorToIdlePosition.Normalize();
                vectorToIdlePosition *= speed;
                projectile.velocity = (projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
            }
            else if (projectile.velocity == Vector2.Zero && !disablepoke)
            {
                // If there is a case where it's not moving at all, give it a little "poke"
                projectile.velocity.X = -0.15f;
                projectile.velocity.Y = -0.05f;
            }
        }
        /// <summary>
        /// Find the closest NPC to the player
        /// </summary>
        /// <param name="player"></param>
        /// <param name="distance"></param>
        /// <param name="npc"></param>
        /// <returns>
        /// Return true if found and return NPC that is closest to player<br/>
        /// Return false if not found any NPC and NPC set to null
        /// </returns>
        public static bool closestToPlayer(this Projectile projectile, Player player ,float distance, out NPC npc)
        {
            LookForHostileNPC(player.Center, out List<NPC> npclocal, distance);
            for (int i = 0; i < npclocal.Count; i++)
            {
                if (!npclocal[i].CanBeChasedBy())
                {
                    continue;
                }
                float between = Vector2.DistanceSquared(npclocal[i].Center, player.Center);
                bool closest = Vector2.DistanceSquared(projectile.Center, npclocal[i].Center) > between;
                if (closest)
                {
                    npc = npclocal[i];
                    return true;
                }
            }
            npc = null;
            return false;
        }

        public static void MinionShootProjectileGeneric(this Projectile projectile, Vector2 velocity, int type)
        {
            Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, velocity, type, projectile.damage, projectile.knockBack, projectile.owner);
        }

        public static int CoolDown(int timer) => timer > 0 ? --timer : 0;
    }
}
