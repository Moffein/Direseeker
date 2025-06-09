using System;
using System.Collections.Generic;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace DireseekerMod.Modules
{
	public static class Projectiles
	{
		public static void CreateProjectiles()
		{
			Projectiles.fireballPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Junk/ArchWisp/ArchWispCannon.prefab").WaitForCompletion().InstantiateClone("DireseekerBossFireball", true);
			Projectiles.fireballGroundPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Junk/ArchWisp/ArchWispGroundCannon.prefab").WaitForCompletion().InstantiateClone("DireseekerBossGroundFireball", true);
			ProjectileController component = Projectiles.fireballPrefab.GetComponent<ProjectileController>();
			component.ghostPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/LemurianBruiser/LemurianBigFireball.prefab").WaitForCompletion().GetComponent<ProjectileController>().ghostPrefab;
			component.startSound = "Play_lemurianBruiser_m1_shoot";
			GameObject impactEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/LemurianBruiser/LemurianBigFireball.prefab").WaitForCompletion().GetComponent<ProjectileImpactExplosion>().impactEffect;
			ProjectileImpactExplosion component2 = Projectiles.fireballPrefab.GetComponent<ProjectileImpactExplosion>();
			component2.childrenProjectilePrefab = Projectiles.fireballGroundPrefab;
			component2.GetComponent<ProjectileImpactExplosion>().impactEffect = impactEffect;
			component2.falloffModel = BlastAttack.FalloffModel.SweetSpot;
			component2.blastDamageCoefficient = 1f;
			component2.blastProcCoefficient = 1f;

			ProjectileDamage fireballDamage = fireballPrefab.GetComponent<ProjectileDamage>();
			fireballDamage.damageType.damageSource = DamageSource.Primary;
			ProjectileDamage fireballGroundDamage = fireballGroundPrefab.GetComponent<ProjectileDamage>();
            fireballGroundDamage.damageType.damageSource = DamageSource.Primary;

            Projectiles.fireballGroundPrefab.GetComponent<ProjectileController>().ghostPrefab = Addressables.LoadAssetAsync<GameObject>(RoR2BepInExPack.GameAssetPaths.RoR2_Base_MagmaWorm.MagmaOrbGhost_prefab).WaitForCompletion();
			Projectiles.fireballGroundPrefab.GetComponent<ProjectileImpactExplosion>().impactEffect = impactEffect;
			Projectiles.fireTrailPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/FireTrail.prefab").WaitForCompletion().InstantiateClone("DireseekerBossFireTrail", true);
			Projectiles.fireTrailPrefab.AddComponent<NetworkIdentity>();
			Projectiles.fireballGroundPrefab.GetComponent<ProjectileDamageTrail>().trailPrefab = Projectiles.fireTrailPrefab;

			ContentAddition.AddProjectile(Projectiles.fireballPrefab);
			ContentAddition.AddProjectile(Projectiles.fireballGroundPrefab);
		}

		public static GameObject fireballPrefab;
		public static GameObject fireballGroundPrefab;
		public static GameObject fireTrailPrefab;
		public static GameObject fireSegmentPrefab;
	}
}
