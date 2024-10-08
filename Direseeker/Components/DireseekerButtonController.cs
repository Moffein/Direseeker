﻿using System;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace DireseekerMod.Components
{
	public class DireseekerButtonController : NetworkBehaviour
	{
		[SyncVar]
		public bool isPressedServer = false;

		public bool isPressedLocal = false;

		private void Awake()
		{
			this.animator = base.GetComponent<Animator>();

			if (NetworkServer.active) this.isPressedServer = false;
			this.isPressedLocal = false;

			this.overlapSphereRadius = 1.5f;
			this.overlapSphereFrequency = 5f;
			this.enableOverlapSphere = true;
		}

		//Client handles press visual
		private void FixedUpdate()
		{
			if (NetworkServer.active) FixedUpdateServer();

			if (!isPressedLocal)
            {
				if (isPressedServer)
                {
					Pressed();
				}
            }
		}

		//Server handles press logic
		private void FixedUpdateServer()
        {
			if (!isPressedServer && this.enableOverlapSphere)
            {
				float overlapTick = 1f / this.overlapSphereFrequency;
				this.overlapSphereStopwatch += Time.fixedDeltaTime;
				if (this.overlapSphereStopwatch >= overlapTick)
				{
					this.overlapSphereStopwatch -= overlapTick;

					if (HGPhysics.DoesOverlapSphere(base.transform.position, overlapSphereRadius, LayerIndex.CommonMasks.characterBodiesOrDefault | LayerIndex.CommonMasks.fakeActorLayers, QueryTriggerInteraction.UseGlobal))
					{
						this.isPressedServer = true;
					}
				}
			}
        }

		private void Pressed()
		{
			isPressedLocal = true;
			this.enableOverlapSphere = false;
			bool flag2 = this.animator;
			if (flag2)
			{
				this.animator.SetBool("pressed", true);
			}
			Util.PlaySound("Play_item_proc_bandolierSpawn", base.gameObject);
		}

		private bool enableOverlapSphere;
		private float overlapSphereRadius;
		private float overlapSphereStopwatch;
		private float overlapSphereFrequency;
		private Animator animator;
	}
}
