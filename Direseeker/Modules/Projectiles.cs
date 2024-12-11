using System;
using System.Collections.Generic;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace DireseekerMod.Modules
{
	public static class Projectiles
	{
		public static void CreateProjectiles()
		{
			Projectiles.fireballPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/ArchWispCannon").InstantiateClone("DireseekerBossFireball", true);
			Projectiles.fireballGroundPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/ArchWispGroundCannon").InstantiateClone("DireseekerBossGroundFireball", true);
			ProjectileController component = Projectiles.fireballPrefab.GetComponent<ProjectileController>();
			component.ghostPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/LemurianBigFireball").GetComponent<ProjectileController>().ghostPrefab;
			component.startSound = "Play_lemurianBruiser_m1_shoot";
			GameObject impactEffect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/LemurianBigFireball").GetComponent<ProjectileImpactExplosion>().impactEffect;
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

            Projectiles.fireballGroundPrefab.GetComponent<ProjectileController>().ghostPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/MagmaOrbProjectile").GetComponent<ProjectileController>().ghostPrefab;
			Projectiles.fireballGroundPrefab.GetComponent<ProjectileImpactExplosion>().impactEffect = impactEffect;
			Projectiles.fireTrailPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/FireTrail").InstantiateClone("DireseekerBossFireTrail", true);
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
