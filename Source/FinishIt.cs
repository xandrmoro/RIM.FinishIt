using HarmonyLib;
using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace FinishIt
{

    public class FinishIt : Mod
    {
        public static Settings Settings { get; private set; }

        public FinishIt(ModContentPack contentPack) : base(contentPack)
        {
            Settings = GetSettings<Settings>();

            new Harmony(Content.PackageIdPlayerFacing).PatchAll();
        }

        public override void WriteSettings()
        {
            base.WriteSettings();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return null;
        }
    }
}
