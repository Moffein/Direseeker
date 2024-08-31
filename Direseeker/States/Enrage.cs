using System;
using System.Collections.Generic;
using DireseekerMod.Components;
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace DireseekerMod.States
{
	public class Enrage : BaseState
	{
		private float lastUpdateTime;	//Maximum cope
		public override void OnEnter()
		{
			base.OnEnter();

			lastUpdateTime = Time.time;

			this.stopwatch = 0f;
			this.entryDuration = Enrage.baseEntryDuration / this.attackSpeedStat;
			this.exitDuration = Enrage.baseExitDuration / this.attackSpeedStat;
			this.childLocator = base.GetModelChildLocator();
			this.direController = base.GetComponent<DireseekerController>();
			bool flag = this.direController;
			if (flag)
			{
				this.direController.StartRageMode();
			}
			bool active = NetworkServer.active;
			if (active)
			{
				base.characterBody.AddBuff(RoR2Content.Buffs.ArmorBoost);
			}
			base.PlayAnimation("Gesture, Override", "PrepFlamebreath", "PrepFlamebreath.playbackRate", this.entryDuration);
			Util.PlaySound("Play_magmaWorm_spawn_VO", base.gameObject);
		}

		private void GrantItems()
		{
			bool active = NetworkServer.active;
			if (active)
			{
				bool flag = base.characterBody.master && base.characterBody.master.inventory;
				if (flag)
				{
					base.characterBody.master.inventory.GiveItem(RoR2Content.Items.AdaptiveArmor, 1);
					base.characterBody.master.inventory.GiveItem(RoR2Content.Items.AlienHead, 2);
					base.characterBody.master.inventory.GiveItem(RoR2Content.Items.Hoof, 5);
					base.characterBody.master.inventory.GiveItem(RoR2Content.Items.Syringe, 5);
				}
			}
		}

		public override void OnExit()
		{
			base.PlayCrossfade("Gesture, Override", "BufferEmpty", 0.1f);
			base.OnExit();
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			float deltaTime = Time.time - lastUpdateTime;
			lastUpdateTime = Time.time;

			this.stopwatch += deltaTime;
			bool flag = this.stopwatch >= this.entryDuration && !this.hasEnraged;
			if (flag)
			{
				this.hasEnraged = true;
				this.GrantItems();
				///AkSoundEngine.StopPlayingID(this.roarStartPlayID);
				//Util.PlaySound("DireseekerRage", base.gameObject);
				//Util.PlaySound("DireseekerRoar", base.gameObject);
				stoppedSound = true;
				Transform modelTransform = base.GetModelTransform();
				bool flag2 = modelTransform;
				if (flag2)
                {
                    CharacterModel cm = modelTransform.gameObject.GetComponent<CharacterModel>();
					if (cm)
                    {
                        if (cm.temporaryOverlays == null) cm.temporaryOverlays = new List<TemporaryOverlayInstance>();
                        TemporaryOverlayInstance temporaryOverlay = TemporaryOverlayManager.AddOverlay(modelTransform.gameObject);
                        temporaryOverlay.duration = 1f;
                        temporaryOverlay.animateShaderAlpha = true;
                        temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                        temporaryOverlay.destroyComponentOnEnd = true;
                        temporaryOverlay.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matOnFire");
                        temporaryOverlay.inspectorCharacterModel = cm;
                    }
                }
				base.PlayAnimation("Gesture, Override", "Flamebreath", "Flamebreath.playbackRate", this.exitDuration);
			}
			bool flag3 = this.stopwatch >= this.entryDuration + 0.75f * this.exitDuration && !this.heck;
			if (flag3)
			{
				this.heck = true;
				base.PlayCrossfade("Gesture, Override", "ExitFlamebreath", "ExitFlamebreath.playbackRate", 0.75f * this.exitDuration, 0.1f);
				bool active = NetworkServer.active;
				if (active)
				{
					base.characterBody.RemoveBuff(RoR2Content.Buffs.ArmorBoost);
				}
			}
			bool flag4 = this.stopwatch >= this.entryDuration + this.exitDuration && base.isAuthority;
			if (flag4)
			{
				this.outer.SetNextStateToMain();
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Skill;
		}

		public static float baseEntryDuration = 1.5f;
		public static float baseExitDuration = 3.5f;

		private float stopwatch;
		private float entryDuration;
		private float exitDuration;
		private bool hasEnraged;
		private bool heck;
		private bool stoppedSound = false;
		private ChildLocator childLocator;
		private DireseekerController direController;
	}
}
