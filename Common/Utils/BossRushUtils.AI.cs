using System;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

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
                vectorToIdlePosition = vectorToIdlePosition.SafeNormalize(Vector2.Zero) * speed;
                projectile.velocity = (projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
            }
            else if (projectile.velocity == Vector2.Zero)
            {
                projectile.velocity.X = -0.15f;
                projectile.velocity.Y = -0.05f;
            }
        }
        public static void MoveToIdle(this Projectile projectile, Vector2 vectorToIdlePosition, float speed, float inertia, bool disablepoke = false)
        {
            float disToIdle = vectorToIdlePosition.LengthSquared();
            if (disToIdle > 20 * 20)
            {
                vectorToIdlePosition = vectorToIdlePosition.SafeNormalize(Vector2.Zero) * speed;
                projectile.velocity = (projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
            }
            else if (projectile.velocity == Vector2.Zero && !disablepoke)
            {
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
        public static bool ClosestToPlayer(this Projectile projectile, Player player, float distance, out NPC npc)
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
        public static void ProjectileSwordSwingAI(Projectile projectile, Player player, Vector2 PositionFromMouseToPlayer, int swing = 1, int swingdegree = 120)
        {
            if (projectile.timeLeft > player.itemAnimationMax)
            {
                projectile.timeLeft = player.itemAnimationMax;
            }
            player.heldProj = projectile.whoAmI;
            float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
            if (swing == -1)
            {
                percentDone = 1 - percentDone;
            }
            percentDone = Math.Clamp(percentDone, 0, 1);
            projectile.spriteDirection = player.direction;
            float baseAngle = PositionFromMouseToPlayer.ToRotation();
            float angle = MathHelper.ToRadians(baseAngle + swingdegree) * player.direction;
            float start = baseAngle + angle;
            float end = baseAngle - angle;
            float currentAngle = MathHelper.SmoothStep(start, end, percentDone);
            projectile.rotation = currentAngle;
            projectile.rotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3f;
            projectile.velocity.X = player.direction; 
            projectile.Center = player.MountedCenter + Vector2.UnitX.RotatedBy(currentAngle) * 42;
            player.compositeFrontArm = new Player.CompositeArmData(true, Player.CompositeArmStretchAmount.Full, currentAngle - MathHelper.PiOver2);
        }
        public static void ModifyProjectileDamageHitbox(ref Rectangle hitbox, Player player, int width, int height, float offset = 0)
        {
            float length = new Vector2(width, height).Length() * player.GetAdjustedItemScale(player.HeldItem);
            Vector2 handPos = Vector2.UnitY.RotatedBy(player.compositeFrontArm.rotation);
            Vector2 endPos = handPos;
            endPos *= length;
            Vector2 offsetVector = handPos * offset - handPos;
            handPos += player.MountedCenter + offsetVector;
            endPos += player.MountedCenter + offsetVector;
            (int X1, int X2) XVals = Order(handPos.X, endPos.X);
            (int Y1, int Y2) YVals = Order(handPos.Y, endPos.Y);
            hitbox = new Rectangle(XVals.X1 - 2, YVals.Y1 - 2, XVals.X2 - XVals.X1 + 2, YVals.Y2 - YVals.Y1 + 2);
        }
        /// <summary>
        /// Please applied the rotation to that is not <see cref="Projectile.rotation"/>
        /// </summary>
        /// <param name="hitbox"></param>
        /// <param name="player"></param>
        /// <param name="rotation"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="offset"></param>
        public static void ModifyProjectileDamageHitbox(ref Rectangle hitbox, Player player,float rotation, int width, int height, float offset = 0)
        {
            float length = new Vector2(width, height).Length() * player.GetAdjustedItemScale(player.HeldItem);
            Vector2 handPos = Vector2.UnitX.RotatedBy(rotation);
            Vector2 endPos = handPos;
            endPos *= length;
            Vector2 offsetVector = handPos * offset - handPos;
            handPos += player.MountedCenter + offsetVector;
            endPos += player.MountedCenter + offsetVector;
            (int X1, int X2) XVals = Order(handPos.X, endPos.X);
            (int Y1, int Y2) YVals = Order(handPos.Y, endPos.Y);
            hitbox = new Rectangle(XVals.X1 - 2, YVals.Y1 - 2, XVals.X2 - XVals.X1 + 2, YVals.Y2 - YVals.Y1 + 2);
        }
        public static int CoolDown(int timer, int timeDecrease = 1) => timer > 0 ? timer - timeDecrease : 0;
        public static float CoolDown(float timer, int timeDecrease = 1) => timer > 0 ? timer - timeDecrease : 0;
    }
}