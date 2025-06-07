using System;
using System.Collections.Generic;
using DireseekerMod.Components;
using KinematicCharacterController;
using R2API;
using RoR2;
using RoR2.CharacterAI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;

namespace DireseekerMod.Modules
{
	public static class Prefabs
	{
		public static void CreatePrefab()
        {
            Prefabs.bodyPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/LemurianBruiser/LemurianBruiserBody.prefab").WaitForCompletion().InstantiateClone("DireseekerBody", true);

            Rigidbody rb = Prefabs.bodyPrefab.GetComponent<Rigidbody>();
			if (rb) rb.mass = 900;


            //Add DeathReward
            //This causes all its drops to be replaced by the Perforator. Why?
            /*GameObject WormPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/MagmaWorm/MagmaWormBody.prefab").WaitForCompletion();
			DeathRewards deathReward = Prefabs.bodyPrefab.GetComponent<DeathRewards>();
			deathReward.bossDropTable = WormPrefab.GetComponent<DeathRewards>().bossDropTable;*/

            UnityEngine.Object.Destroy(Prefabs.bodyPrefab.GetComponent<SetStateOnHurt>());
			CharacterBody component = Prefabs.bodyPrefab.GetComponent<CharacterBody>();
			component.name = "DireseekerBossBody";
			component.baseNameToken = "DIRESEEKER_BOSS_BODY_NAME";
			component.subtitleNameToken = "DIRESEEKER_BOSS_BODY_SUBTITLE";
			component.baseMoveSpeed = 11f;
			component.baseMaxHealth = 2800f;
			component.levelMaxHealth = component.baseMaxHealth * 0.3f;
			component.baseDamage = 20f;
			component.levelDamage = component.baseDamage * 0.2f;
			component.isChampion = true;
			component.portraitIcon = Assets.bossPortrait;
			Prefabs.bodyPrefab.GetComponent<SfxLocator>().deathSound = "DireseekerDeath";
			Prefabs.bodyPrefab.GetComponent<ModelLocator>().modelBaseTransform.gameObject.transform.localScale *= 1.5f;
			foreach (KinematicCharacterMotor kinematicCharacterMotor in Prefabs.bodyPrefab.GetComponentsInChildren<KinematicCharacterMotor>())
			{
				kinematicCharacterMotor.SetCapsuleDimensions(kinematicCharacterMotor.Capsule.radius * 1.5f, kinematicCharacterMotor.Capsule.height * 1.5f, 1.5f);
			}
			CharacterModel componentInChildren = Prefabs.bodyPrefab.GetComponentInChildren<CharacterModel>();
			Material material = UnityEngine.Object.Instantiate<Material>(componentInChildren.baseRendererInfos[0].defaultMaterial);
			material.SetTexture("_MainTex", Assets.mainAssetBundle.LoadAsset<Material>("matDireseeker").GetTexture("_MainTex"));
			material.SetTexture("_EmTex", Assets.mainAssetBundle.LoadAsset<Material>("matDireseeker").GetTexture("_EmissionMap"));
			material.SetFloat("_EmPower", 50f);
			componentInChildren.baseRendererInfos[0].defaultMaterial = material;
			GameObject gameObject = Assets.mainAssetBundle.LoadAsset<GameObject>("DireHorn").InstantiateClone("DireseekerHorn", false);
			GameObject gameObject2 = Assets.mainAssetBundle.LoadAsset<GameObject>("DireHornBroken").InstantiateClone("DireseekerHornBroken", false);
			GameObject gameObject3 = Assets.mainAssetBundle.LoadAsset<GameObject>("DireseekerRageFlame").InstantiateClone("DireseekerRageFlame", false);
			GameObject gameObject4 = Assets.mainAssetBundle.LoadAsset<GameObject>("DireseekerBurstFlame").InstantiateClone("DireseekerBurstFlame", false);
			ChildLocator componentInChildren2 = Prefabs.bodyPrefab.GetComponentInChildren<ChildLocator>();
			gameObject.transform.SetParent(componentInChildren2.FindChild("Head"));
			gameObject.transform.localPosition = new Vector3(-2.5f, 1f, -0.5f);
			gameObject.transform.localRotation = Quaternion.Euler(new Vector3(45f, 0f, 90f));
			gameObject.transform.localScale = new Vector3(100f, 100f, 100f);
			gameObject2.transform.SetParent(componentInChildren2.FindChild("Head"));
			gameObject2.transform.localPosition = new Vector3(2.5f, 1f, -0.5f);
			gameObject2.transform.localRotation = Quaternion.Euler(new Vector3(45f, 0f, 90f));
			gameObject2.transform.localScale = new Vector3(100f, -100f, 100f);
			gameObject3.transform.SetParent(componentInChildren2.FindChild("Head"));
			gameObject3.transform.localPosition = new Vector3(0f, 1f, 0f);
			gameObject3.transform.localRotation = Quaternion.Euler(new Vector3(270f, 180f, 0f));
			gameObject3.transform.localScale = new Vector3(4f, 4f, 4f);
			gameObject4.transform.SetParent(componentInChildren2.FindChild("Head"));
			gameObject4.transform.localPosition = new Vector3(0f, 1f, 0f);
			gameObject4.transform.localRotation = Quaternion.Euler(new Vector3(270f, 180f, 0f));
			gameObject4.transform.localScale = new Vector3(6f, 6f, 6f);
			DireseekerController direseekerController = Prefabs.bodyPrefab.AddComponent<DireseekerController>();
			direseekerController.burstFlame = gameObject4.GetComponent<ParticleSystem>();
			direseekerController.rageFlame = gameObject3.GetComponent<ParticleSystem>();
			direseekerController.rageFlame.Stop();

			//This section nullrefs.
            Shader shader = Addressables.LoadAssetAsync<Shader>("RoR2/Base/Shaders/HGStandard.shader").WaitForCompletion();
			Material material2 = gameObject.GetComponentInChildren<MeshRenderer>().material;
			material2.shader = shader;
			CharacterModel.RendererInfo[] baseRendererInfos = componentInChildren.baseRendererInfos;
			CharacterModel.RendererInfo[] baseRendererInfos2 = new CharacterModel.RendererInfo[]
			{
				baseRendererInfos[0],
				new CharacterModel.RendererInfo
				{
					renderer = gameObject.GetComponentInChildren<MeshRenderer>(),
					defaultMaterial = material2,
					defaultShadowCastingMode = ShadowCastingMode.On,
					ignoreOverlays = true
				},
				new CharacterModel.RendererInfo
				{
					renderer = gameObject2.GetComponentInChildren<MeshRenderer>(),
					defaultMaterial = material2,
					defaultShadowCastingMode = ShadowCastingMode.On,
					ignoreOverlays = true
				}
			};
			componentInChildren.baseRendererInfos = baseRendererInfos2;
			//

            Prefabs.masterPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/LemurianBruiser/LemurianBruiserMaster.prefab").WaitForCompletion().InstantiateClone("DireseekerBossMaster", true);
			CharacterMaster component2 = Prefabs.masterPrefab.GetComponent<CharacterMaster>();
			component2.bodyPrefab = Prefabs.bodyPrefab;
			component2.isBoss = true;
			Prefabs.CreateAI();

			ContentAddition.AddBody(Prefabs.bodyPrefab);
			ContentAddition.AddMaster(Prefabs.masterPrefab);
		}

		private static void CreateAI()
		{
			foreach (AISkillDriver obj in Prefabs.masterPrefab.GetComponentsInChildren<AISkillDriver>())
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
			AISkillDriver aiskillDriver = Prefabs.masterPrefab.AddComponent<AISkillDriver>();
			aiskillDriver.customName = "Enrage";
			aiskillDriver.requireSkillReady = true;
			aiskillDriver.movementType = AISkillDriver.MovementType.Stop;
			aiskillDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
			aiskillDriver.selectionRequiresTargetLoS = false;
			aiskillDriver.activationRequiresAimConfirmation = false;
			aiskillDriver.activationRequiresTargetLoS = false;
			aiskillDriver.maxDistance = float.PositiveInfinity;
			aiskillDriver.minDistance = 0f;
			aiskillDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
			aiskillDriver.ignoreNodeGraph = false;
			aiskillDriver.moveInputScale = 1f;
			aiskillDriver.driverUpdateTimerOverride = -1f;
			aiskillDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
			aiskillDriver.minTargetHealthFraction = float.NegativeInfinity;
			aiskillDriver.maxTargetHealthFraction = float.PositiveInfinity;
			aiskillDriver.minUserHealthFraction = float.NegativeInfinity;
			aiskillDriver.maxUserHealthFraction = 0.2f;
			aiskillDriver.skillSlot = SkillSlot.Special;
			AISkillDriver aiskillDriver2 = Prefabs.masterPrefab.AddComponent<AISkillDriver>();
			aiskillDriver2.customName = "FlamePillar";
			aiskillDriver2.requireSkillReady = true;
			aiskillDriver2.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
			aiskillDriver2.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
			aiskillDriver2.selectionRequiresTargetLoS = true;
			aiskillDriver2.activationRequiresAimConfirmation = true;
			aiskillDriver2.activationRequiresTargetLoS = false;
			aiskillDriver2.maxDistance = 120f;
			aiskillDriver2.minDistance = 5f;
			aiskillDriver2.aimType = AISkillDriver.AimType.AtMoveTarget;
			aiskillDriver2.ignoreNodeGraph = false;
			aiskillDriver2.moveInputScale = 1f;
			aiskillDriver2.driverUpdateTimerOverride = -1f;
			aiskillDriver2.buttonPressType = AISkillDriver.ButtonPressType.Hold;
			aiskillDriver2.minTargetHealthFraction = float.NegativeInfinity;
			aiskillDriver2.maxTargetHealthFraction = float.PositiveInfinity;
			aiskillDriver2.minUserHealthFraction = float.NegativeInfinity;
			aiskillDriver2.maxUserHealthFraction = 0.75f;
			aiskillDriver2.skillSlot = SkillSlot.Utility;
			AISkillDriver aiskillDriver3 = Prefabs.masterPrefab.AddComponent<AISkillDriver>();
			aiskillDriver3.customName = "Flamethrower";
			aiskillDriver3.requireSkillReady = true;
			aiskillDriver3.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
			aiskillDriver3.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
			aiskillDriver3.selectionRequiresTargetLoS = true;
			aiskillDriver3.activationRequiresAimConfirmation = true;
			aiskillDriver3.activationRequiresTargetLoS = false;
			aiskillDriver3.maxDistance = 20f;
			aiskillDriver3.minDistance = 0f;
			aiskillDriver3.aimType = AISkillDriver.AimType.AtMoveTarget;
			aiskillDriver3.ignoreNodeGraph = false;
			aiskillDriver3.moveInputScale = 1f;
			aiskillDriver3.driverUpdateTimerOverride = -1f;
			aiskillDriver3.buttonPressType = AISkillDriver.ButtonPressType.Hold;
			aiskillDriver3.minTargetHealthFraction = float.NegativeInfinity;
			aiskillDriver3.maxTargetHealthFraction = float.PositiveInfinity;
			aiskillDriver3.minUserHealthFraction = float.NegativeInfinity;
			aiskillDriver3.maxUserHealthFraction = float.PositiveInfinity;
			aiskillDriver3.skillSlot = SkillSlot.Secondary;
			AISkillDriver aiskillDriver4 = Prefabs.masterPrefab.AddComponent<AISkillDriver>();
			aiskillDriver4.customName = "RunAndShoot";
			aiskillDriver4.requireSkillReady = true;
			aiskillDriver4.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
			aiskillDriver4.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
			aiskillDriver4.selectionRequiresTargetLoS = true;
			aiskillDriver4.activationRequiresAimConfirmation = true;
			aiskillDriver4.activationRequiresTargetLoS = false;
			aiskillDriver4.maxDistance = 50f;
			aiskillDriver4.minDistance = 0f;
			aiskillDriver4.aimType = AISkillDriver.AimType.AtMoveTarget;
			aiskillDriver4.ignoreNodeGraph = false;
			aiskillDriver4.moveInputScale = 1f;
			aiskillDriver4.driverUpdateTimerOverride = 2f;
			aiskillDriver4.buttonPressType = AISkillDriver.ButtonPressType.Hold;
			aiskillDriver4.minTargetHealthFraction = float.NegativeInfinity;
			aiskillDriver4.maxTargetHealthFraction = float.PositiveInfinity;
			aiskillDriver4.minUserHealthFraction = float.NegativeInfinity;
			aiskillDriver4.maxUserHealthFraction = float.PositiveInfinity;
			aiskillDriver4.skillSlot = SkillSlot.Primary;
			AISkillDriver aiskillDriver5 = Prefabs.masterPrefab.AddComponent<AISkillDriver>();
			aiskillDriver5.customName = "StopAndShoot";
			aiskillDriver5.requireSkillReady = true;
			aiskillDriver5.movementType = AISkillDriver.MovementType.Stop;
			aiskillDriver5.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
			aiskillDriver5.selectionRequiresTargetLoS = true;
			aiskillDriver5.activationRequiresAimConfirmation = true;
			aiskillDriver5.activationRequiresTargetLoS = false;
			aiskillDriver5.maxDistance = 100f;
			aiskillDriver5.minDistance = 50f;
			aiskillDriver5.aimType = AISkillDriver.AimType.AtMoveTarget;
			aiskillDriver5.ignoreNodeGraph = false;
			aiskillDriver5.moveInputScale = 1f;
			aiskillDriver5.driverUpdateTimerOverride = 2f;
			aiskillDriver5.buttonPressType = AISkillDriver.ButtonPressType.Hold;
			aiskillDriver5.minTargetHealthFraction = float.NegativeInfinity;
			aiskillDriver5.maxTargetHealthFraction = float.PositiveInfinity;
			aiskillDriver5.minUserHealthFraction = float.NegativeInfinity;
			aiskillDriver5.maxUserHealthFraction = float.PositiveInfinity;
			aiskillDriver5.skillSlot = SkillSlot.Primary;
			AISkillDriver aiskillDriver6 = Prefabs.masterPrefab.AddComponent<AISkillDriver>();
			aiskillDriver6.customName = "Chase";
			aiskillDriver6.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
			aiskillDriver6.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
			aiskillDriver6.activationRequiresAimConfirmation = false;
			aiskillDriver6.activationRequiresTargetLoS = false;
			aiskillDriver6.maxDistance = float.PositiveInfinity;
			aiskillDriver6.minDistance = 0f;
			aiskillDriver6.aimType = AISkillDriver.AimType.AtMoveTarget;
			aiskillDriver6.ignoreNodeGraph = false;
			aiskillDriver6.moveInputScale = 1f;
			aiskillDriver6.driverUpdateTimerOverride = -1f;
			aiskillDriver6.buttonPressType = AISkillDriver.ButtonPressType.Hold;
			aiskillDriver6.minTargetHealthFraction = float.NegativeInfinity;
			aiskillDriver6.maxTargetHealthFraction = float.PositiveInfinity;
			aiskillDriver6.minUserHealthFraction = float.NegativeInfinity;
			aiskillDriver6.maxUserHealthFraction = float.PositiveInfinity;
			aiskillDriver6.skillSlot = SkillSlot.None;
		}

		public static GameObject bodyPrefab;
		public static GameObject masterPrefab;

	}
}
