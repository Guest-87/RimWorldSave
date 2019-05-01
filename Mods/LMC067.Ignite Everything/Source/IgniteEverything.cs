using System.Reflection;
using System.Collections.Generic;
using Harmony;
using Verse;
using Verse.AI;
using RimWorld;

namespace IgniteEverything
{
    [StaticConstructorOnStartup]
    public class IgniteEverything : Mod
    {
        public static List<Pawn> Igniters;

        public IgniteEverything(ModContentPack content) : base(content)
        {
            Igniters = new List<Pawn>();

            HarmonyInstance harmony = HarmonyInstance.Create("rimworld.undone.igniteverything");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Log.Message("Ignite Everything loaded");
        }

        public static TargetingParameters IgniteTargetParams()
        {
            TargetingParameters targetParams = new TargetingParameters();

            targetParams.canTargetBuildings = true;
            targetParams.canTargetItems = true;
            targetParams.canTargetLocations = false;
            targetParams.canTargetSelf = false;
            targetParams.canTargetPawns = false;
            targetParams.canTargetFires = false;
            targetParams.onlyTargetFlammables = true;
            targetParams.mapObjectTargetsMustBeAutoAttackable = false;

            targetParams.validator = delegate (TargetInfo target)
            {
                Thing thing = target.Thing;

                if (thing == null || thing.IsBurning() || thing is Pawn)
                {
                    return false;
                }

                return true;
            };

            return targetParams;
        }

        public static Gizmo GetIgniteGizmo()
        {
            Command_Target target = new Command_Target();
            target.defaultLabel = "Ignite";
            target.defaultDesc = "Set a thing on fire.";
            target.icon = IgniteResources.IgniteIcon;
            target.targetingParams = IgniteTargetParams();
            target.hotKey = KeyBindingDefOf.Command_ItemForbid;

            target.action = delegate (Thing thing)
            {
                Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;

                Job job = new Job(JobDefOf.Ignite, thing);

                if (pawn.jobs.TryTakeOrderedJob(job, JobTag.DraftedOrder))
                {
                    ReservationUtility.Reserve(pawn, thing, job);

                    // We're going to fight the automatic fire extinguishing system with this
                    if (!pawn.story.WorkTagIsDisabled(WorkTags.Firefighting) && !Igniters.Contains(pawn))
                    {
                        Igniters.Add(pawn);
                    }
                }
            };

            return target;
        }
    }
}
