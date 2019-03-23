﻿using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Thor
{
    public enum AnimationActions
    {
        CallingForMjolnir = 0,
        ThrowHammer1 = 1,
        ThrowHammer2 = 2,
        ThrowHammer3 = 3,
        ThrowHammer4 = 4,
        ThrowHammer5 = 5,
        CatchingMjolnir1,
        GroundAttack1,
        GroundAttack2,
        GroundAttack3,
        GroundAttack4,
        SummonThunder,
        WhirlingHammer,
    }

    public enum ParticleEffects
    {
        Thunder
    }

    public enum RagdollType
    {
        Normal = 0,
        StiffLegs = 1,
        NarrowLegs = 2,
        WideLegs = 3,
    }

    public static class NativeHelper
    {
        private static float MELEE_HIT_FORCE = 250.0f;
        private static int MELEE_HIT_PED_DAMAGE = 100;
        private static string[] AnimationDictNames = (new List<string>
        {
            "weapons@first_person@aim_lt@p_m_zero@projectile@misc@sticky_bomb@aim_trans@lt_to_rng",
            "anim@melee@machete@streamed_core@",
            "anim@melee@machete@streamed_core@",
            "melee@small_wpn@streamed_core_fps",
            "melee@small_wpn@streamed_core_fps",
            "melee@small_wpn@streamed_core",
            "melee@small_wpn@streamed_core",
            "melee@small_wpn@streamed_core_fps",
            "melee@knife@streamed_core",
            "melee@small_wpn@streamed_core",
            "melee@small_wpn@streamed_core",
            "anim@mp_fm_event@intro",
            "melee@small_wpn@streamed_core"
        }).ToArray();
        private static string[] AnimationNames = (new List<string>
        {
            "aim_trans_low",
            "small_melee_wpn_short_range_0",
            "plyr_walking_attack_a",
            "small_melee_wpn_short_range_0",
            "small_melee_wpn_long_range_0",
            "small_melee_wpn_long_range_0",
            "melee_damage_back",
            "ground_attack_on_spot",
            "ground_attack_on_spot",
            "ground_attack_0",
            "ground_attack_on_spot",
            "beast_transform",
            "dodge_generic_centre"
        }).ToArray();
        private static Dictionary<string, Dictionary<string, int>> AnimationWaitTime = new Dictionary<string, Dictionary<string, int>>()
        {
            {
                "combat@aim_variations@1h@gang",
                new Dictionary<string, int>() { }
            },
            {
                "anim@melee@machete@streamed_core@",
                new Dictionary<string, int>()
                {
                    {
                        "small_melee_wpn_short_range_0", 800
                    },
                    {
                        "plyr_walking_attack_a", 1000
                    }
                }
            },
            {
                "melee@small_wpn@streamed_core_fps",
                new Dictionary<string, int>()
                {
                    {
                        "small_melee_wpn_short_range_0", 600
                    },
                    {
                        "small_melee_wpn_short_range_+90", 600
                    },
                    {
                        "small_melee_wpn_short_range_+180", 500
                    },
                    {
                        "small_melee_wpn_short_range_-90", 400
                    },
                    {
                        "small_melee_wpn_short_range_-180", 800
                    },
                    {
                        "small_melee_wpn_long_range_0", 1000
                    },
                    {
                        "small_melee_wpn_long_range_+90", 700
                    },
                    {
                        "small_melee_wpn_long_range_+180", 1000
                    },
                    {
                        "small_melee_wpn_long_range_-90", 700
                    },
                    {
                        "small_melee_wpn_long_range_-180", 800
                    }
                }
            },
            {
                "melee@small_wpn@streamed_core",
                new Dictionary<string, int>()
                {
                    {
                        "small_melee_wpn_long_range_0", 1000
                    },
                    {
                        "small_melee_wpn_long_range_+90", 600
                    },
                    {
                        "small_melee_wpn_long_range_+180", 1100
                    },
                    {
                        "small_melee_wpn_long_range_-90", 700
                    },
                    {
                        "small_melee_wpn_long_range_-180", 800
                    }
                }
            },
            {
                "guard_reactions",
                new Dictionary<string, int>() { }
            },
            {
                "cover@weapon@1h",
                new Dictionary<string, int>() { }
            },
            {
                "combat@fire_variations@1h@gang",
                new Dictionary<string, int>() { }
            }
        };
        private static bool[] AnimationWithAngles = (new List<bool>
        {
            false,
            false,
            false,
            true,
            true,
            true,
            false,
            false,
            false
        }).ToArray();

        private static string[] ParticleEffectSetNames = (new List<string>
        {
            "core"
        }).ToArray();
        private static string[] ParticleEffectNames = (new List<string>
        {
            "ent_dst_elec_fire_sp"
        }).ToArray();

        public static int GetAnimationWaitTimeByDictNameAndAnimName(string dictName, string animName)
        {
            if (AnimationWaitTime.ContainsKey(dictName) &&
                AnimationWaitTime[dictName].ContainsKey(animName))
            {
                return AnimationWaitTime[dictName][animName];
            }

            return 0;

        }

        public static string GetAnimationDictNameByAction(AnimationActions action)
        {
            return AnimationDictNames[(int)action];
        }

        public static string GetAnimationNameByAction(AnimationActions action)
        {
            return AnimationNames[(int)action];
        }

        public static string GetParticleSetName(ParticleEffects fx)
        {
            return ParticleEffectSetNames[(int)fx];
        }

        public static string GetParticleName(ParticleEffects fx)
        {
            return ParticleEffectNames[(int)fx];
        }

        public static bool DoesAnimationActionHaveAngles(AnimationActions action)
        {
            return AnimationWithAngles[(int)action];
        }

        public static void ClearPlayerAnimation(Ped ped, string dictName, string animName)
        {
            Function.Call(Hash.STOP_ANIM_TASK, ped, dictName, animName, 3);
        }

        public static void PlayPlayerAnimation(Ped ped, string dictName, string animName, AnimationFlags flag, int duration = -1, bool checkIsPlaying = true)
        {
            if (checkIsPlaying && IsEntityPlayingAnim(ped, dictName, animName))
            {
                return;
            }
            Function.Call(Hash.REQUEST_ANIM_DICT, dictName);
            if (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, dictName))
            {
                Function.Call(Hash.REQUEST_ANIM_DICT, dictName);
            }

            Function.Call(Hash.TASK_PLAY_ANIM, ped, dictName, animName, 8.0f, 1.0f, duration, (int)flag, -8.0f, 0, 0, 0);
        }

        public static bool IsEntityPlayingAnim(Entity ent, string dictName, string animName)
        {
            Function.Call(Hash.REQUEST_ANIM_DICT, dictName);
            if (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, dictName))
            {
                Function.Call(Hash.REQUEST_ANIM_DICT, dictName);
            }
            return Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, ent, dictName, animName, 3);
        }

        public static void SetEntityVelocity(InputArgument entity, Vector3 velocity)
        {
            Function.Call(Hash.SET_ENTITY_VELOCITY, entity, velocity.X, velocity.Y, velocity.Z);
        }

        public static Entity CreateWeaponObject(WeaponHash weaponHash, int amountCount, Vector3 position, bool showWorldModel = true, float heading = 1.0f)
        {
            new WeaponAsset((WeaponHash)weaponHash).Request(3000);

            return Function.Call<Entity>(Hash.CREATE_WEAPON_OBJECT, (int)weaponHash, amountCount, position.X, position.Y, position.Z, showWorldModel, heading);
        }

        public static void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            Function.Call(Hash.DRAW_LINE, start.X, start.Y, start.Z, end.X, end.Y, end.Z, color.R, color.G, color.B, color.A);
        }

        public static bool IsPed(Entity entity)
        {
            return Function.Call<bool>(Hash.IS_ENTITY_A_PED, entity);
        }

        public static bool IsVehicle(Entity entity)
        {
            return Function.Call<bool>(Hash.IS_ENTITY_A_VEHICLE, entity);
        }

        public static void SetPedWeaponVisible(Ped ped, bool visible)
        {
            Function.Call(Hash.SET_PED_CURRENT_WEAPON_VISIBLE, ped, visible, 0, 0, 0);
        }

        private static void BeforePlayingParticleFx(string effectSetName)
        {
            Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, effectSetName);
            Function.Call(Hash._SET_PTFX_ASSET_NEXT_CALL, effectSetName);
        }

        public static void PlayParticleFx(string effectSetName, string effect, Entity entity, float scale = 1.0f)
        {
            BeforePlayingParticleFx(effectSetName);
            Function.Call(Hash.START_PARTICLE_FX_NON_LOOPED_ON_ENTITY, effect, entity, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, scale, 0, 0, 0);
        }

        public static void PlayParticleFx(string effectSetName, string effect, Entity entity, Vector3 pos, float scale = 1.0f)
        {
            BeforePlayingParticleFx(effectSetName);
            Function.Call(Hash.START_PARTICLE_FX_NON_LOOPED_ON_ENTITY, effect, entity, pos.X, pos.Y, pos.Z, 0.0f, 0.0f, 0.0f, scale, 0, 0, 0);
        }

        public static void PlayParticleFx(string effectSetName, string effect, Ped ped, Bone boneId, float scale = 1.0f)
        {
            BeforePlayingParticleFx(effectSetName);
            Function.Call(Hash.START_PARTICLE_FX_NON_LOOPED_ON_PED_BONE, effect, ped, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, (int)boneId, scale, 0, 0, 0);
        }

        public static void PlayParticleFx(string effectSetName, string effect, Vector3 pos, Vector3 rot, float scale = 1.0f)
        {
            BeforePlayingParticleFx(effectSetName);
            Function.Call(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, effect, pos.X, pos.Y, pos.Z, rot.X, rot.Y, rot.Z, scale, 0, 0, 0);
        }

        public static int PlayParticleFxLooped(string effectSetName, string effect, Ped ped, Bone boneId, float scale = 1.0f)
        {
            BeforePlayingParticleFx(effectSetName);
            return Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_ON_PED_BONE, effect, ped, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, (int)boneId, scale, 0, 0, 0);
        }

        public static int PlayParticleFxLooped(string effectSetName, string effect, Vector3 pos, Vector3 rot, float scale = 1.0f)
        {
            BeforePlayingParticleFx(effectSetName);
            return Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, effect, pos.X, pos.Y, pos.Z, rot.X, rot.Y, rot.Z, scale, 0, 0, 0);
        }

        public static void SetObjectPhysicsParams(
            Entity entity,
            float mass,
            float gravity = -1,
            float dragCoefficient1 = 0.0f,
            float dragCoefficient2 = 0.0f,
            float dragCoefficient3 = 0.0f,
            float rotationDragCoefficient1 = 0.0f,
            float rotationDragCoefficient2 = 0.0f,
            float rotationDragCoefficient3 = 0.0f)
        {
            Function.Call(
                Hash.SET_OBJECT_PHYSICS_PARAMS,
                entity,
                mass,
                gravity,
                dragCoefficient1,
                dragCoefficient2,
                dragCoefficient3,
                rotationDragCoefficient1,
                rotationDragCoefficient2,
                rotationDragCoefficient3,
                0.0f
            );
        }

        public static void SetPedToRagdoll(Ped ped, RagdollType ragdollType, int timeToStayInRagdoll, int timeToStandUp)
        {
            ped.CanRagdoll = true;
            Function.Call(Hash.SET_PED_TO_RAGDOLL, ped, timeToStayInRagdoll, timeToStandUp, (int)ragdollType, 0, 0, 0);
        }

        public static void PlayThunderFx(Entity ent, float scale = 1.0f)
        {
            PlayParticleFx(GetParticleSetName(ParticleEffects.Thunder), GetParticleName(ParticleEffects.Thunder), ent, scale);
        }

        public static void PlayThunderFx(Ped ped, Bone boneId, float scale = 1.0f)
        {
            PlayParticleFx(GetParticleSetName(ParticleEffects.Thunder), GetParticleName(ParticleEffects.Thunder), ped, boneId, scale);
        }

        public static void PlayThunderFx(Vector3 pos, float scale = 1.0f)
        {
            PlayParticleFx(GetParticleSetName(ParticleEffects.Thunder), GetParticleName(ParticleEffects.Thunder), pos, Vector3.Zero, scale);
        }

        public static void ApplyForcesAndDamages(Entity ent, Vector3 direction)
        {
            if (IsPed(ent) && ent != Game.Player.Character)
            {
                var ped = (Ped)ent;
                SetPedToRagdoll(ped, RagdollType.Normal, 100, 100);
                ped.ApplyDamage(MELEE_HIT_PED_DAMAGE);
            }
            ent.ApplyForce(direction * MELEE_HIT_FORCE);
            Function.Call(Hash.CLEAR_ENTITY_LAST_DAMAGE_ENTITY, ent);
        }

        public static void DrawLines(ref List<Line> lines)
        {
            foreach (var line in lines)
            {
                line.Draw();
            }
        }

        public static void DrawBox(Vector3 a, Vector3 b, Color col)
        {
            Function.Call(Hash.DRAW_BOX, a.X, a.Y, a.Z, b.X, b.Y, b.Z, col.R, col.G, col.B, col.A);
        }

        public static IntersectOptions IntersectAllObjects
        {
            get
            {
                return (IntersectOptions)(2 | 4 | 8 | 16);
            }
        }
    }
}
