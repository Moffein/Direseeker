using System;
using System.IO;
using System.Reflection;
using DireseekerMod.Components;
using DireseekerMod.States.Missions.DireseekerEncounter;
using EntityStates;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace DireseekerMod.Modules
{
	public static class Assets
	{
		public static void PopulateAssets()
		{
			bool flag = Assets.mainAssetBundle == null;
			if (flag)
			{
				using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Direseeker.direseeker"))
				{
					Assets.mainAssetBundle = AssetBundle.LoadFromStream(manifestResourceStream);
				}
			}

			Assets.bossPortrait = Assets.mainAssetBundle.LoadAsset<Sprite>("texDireseekerIcon").texture;
			Assets.charPortrait = Assets.mainAssetBundle.LoadAsset<Sprite>("texDireseekerPlayerIcon").texture;
			Assets.direseekerEncounter = Assets.mainAssetBundle.LoadAsset<GameObject>("BossEncounter");
			Assets.direseekerEncounter.AddComponent<NetworkIdentity>();
			Assets.direseekerEncounter.RegisterNetworkPrefab();    //Apparently this auto adds it to the contentpack?

			Assets.direseekerButton = Assets.mainAssetBundle.LoadAsset<GameObject>("DireseekerButton");
			Shader shader = LegacyResourcesAPI.Load<Shader>("Shaders/Deferred/hgstandard");
			Material material = Assets.direseekerButton.GetComponentInChildren<SkinnedMeshRenderer>().material;
			material.shader = shader;
			Assets.direseekerButton.AddComponent<DireseekerButtonController>();
			Assets.direseekerButton.AddComponent<NetworkIdentity>();
			Assets.direseekerButton.RegisterNetworkPrefab();	//Apparently this auto adds it to the contentpack?

			Assets.mainAssetBundle.LoadAsset<Material>("matPillarPrediction").shader = shader;
			Assets.flamePillarPredictionEffect = Assets.LoadEffect("FlamePillarPredictionEffect", "");
		}

		public static void UpdateAssets()
		{
			GameObject gameObject = Assets.direseekerEncounter.transform.GetChild(0).gameObject;
			gameObject.AddComponent<NetworkIdentity>();

			//Should fix the encounter not showing a healthbar in MP?
			gameObject.RegisterNetworkPrefab();

			ScriptedCombatEncounter scriptedCombatEncounter = gameObject.AddComponent<ScriptedCombatEncounter>();
			Assets.direseekerEncounter.transform.GetChild(0).GetChild(1).Translate(0f, 1f, 0f);
			scriptedCombatEncounter.spawns = new ScriptedCombatEncounter.SpawnInfo[]
			{
				new ScriptedCombatEncounter.SpawnInfo
				{
					explicitSpawnPosition = Assets.direseekerEncounter.transform.GetChild(0).GetChild(0),
					spawnCard = SpawnCards.bossSpawnCard
				}
			};
			scriptedCombatEncounter.randomizeSeed = false;
			scriptedCombatEncounter.teamIndex = TeamIndex.Monster;
			scriptedCombatEncounter.spawnOnStart = false;
			scriptedCombatEncounter.grantUniqueBonusScaling = true;
			BossGroup bossGroup = gameObject.AddComponent<BossGroup>();
			bossGroup.bossDropChance = 1f;
			bossGroup.dropPosition = Assets.direseekerEncounter.transform.GetChild(0).GetChild(1);
			bossGroup.forceTier3Reward = true;
			bossGroup.scaleRewardsByPlayerCount = true;
			bossGroup.shouldDisplayHealthBarOnHud = true;
			CombatSquad combatSquad = gameObject.AddComponent<CombatSquad>();
			EntityStateMachine entityStateMachine = gameObject.AddComponent<EntityStateMachine>();
			entityStateMachine.initialStateType = new SerializableEntityStateType(typeof(Listening));
			entityStateMachine.mainStateType = new SerializableEntityStateType(typeof(Listening));
		}
		
		private static GameObject LoadEffect(string resourceName, string soundName)
		{
			GameObject gameObject = Assets.mainAssetBundle.LoadAsset<GameObject>(resourceName);
			gameObject.AddComponent<DestroyOnTimer>().duration = 12f;
			gameObject.AddComponent<NetworkIdentity>();
			gameObject.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
			EffectComponent effectComponent = gameObject.AddComponent<EffectComponent>();
			effectComponent.applyScale = false;
			effectComponent.effectIndex = EffectIndex.Invalid;
			effectComponent.parentToReferencedTransform = true;
			effectComponent.positionAtReferencedTransform = true;
			effectComponent.soundName = soundName;
			ContentAddition.AddEffect(gameObject);
			return gameObject;
		}

		public static AssetBundle mainAssetBundle;
		public static Texture bossPortrait;
		public static Texture charPortrait;
		public static GameObject flamePillarPredictionEffect;
		public static GameObject direseekerEncounter;
		public static GameObject direseekerButton;
	}
}
