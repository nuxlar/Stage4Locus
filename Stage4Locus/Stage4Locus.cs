using BepInEx;
using RoR2;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AddressableAssets;

namespace Stage4Locus
{
  [BepInPlugin("com.Nuxlar.Stage4Locus", "Stage4Locus", "0.9.1")]

  public class Stage4Locus : BaseUnityPlugin
  {
    // SceneDefs
    SceneDef locusDef = Addressables.LoadAssetAsync<SceneDef>("RoR2/DLC1/voidstage/voidstage.asset").WaitForCompletion();
    SceneDef abyssalDef = Addressables.LoadAssetAsync<SceneDef>("RoR2/Base/dampcavesimple/dampcavesimple.asset").WaitForCompletion();
    // SceneCollections
    static SceneCollection stage4SceneCollection = Addressables.LoadAssetAsync<SceneCollection>("RoR2/Base/SceneGroups/sgStage4.asset").WaitForCompletion();
    static SceneCollection stage5SceneCollection = Addressables.LoadAssetAsync<SceneCollection>("RoR2/Base/SceneGroups/sgStage5.asset").WaitForCompletion();
    // DCCS
    static DirectorCardCategorySelection locusInteractables = Addressables.LoadAssetAsync<DirectorCardCategorySelection>("RoR2/DLC1/voidstage/dccsVoidStageInteractables.asset").WaitForCompletion();
    static DirectorCardCategorySelection locusMonsters = Addressables.LoadAssetAsync<DirectorCardCategorySelection>("RoR2/DLC1/voidstage/dccsVoidStageMonsters.asset").WaitForCompletion();
    static DirectorCardCategorySelection abyssalMonsters = Addressables.LoadAssetAsync<DirectorCardCategorySelection>("RoR2/Base/dampcave/dccsDampCaveMonstersDLC1.asset").WaitForCompletion();
    static DirectorCardCategorySelection abyssalInteractables = Addressables.LoadAssetAsync<DirectorCardCategorySelection>("RoR2/Base/dampcave/dccsDampCaveInteractablesDLC1.asset").WaitForCompletion();
    // GameObjects
    static GameObject newtStatue = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/NewtStatue/NewtStatue.prefab").WaitForCompletion();
    // SpawnCards 
    static SpawnCard goldChest = Addressables.LoadAssetAsync<SpawnCard>("RoR2/Base/GoldChest/iscGoldChest.asset").WaitForCompletion();
    static SpawnCard teleporter = Addressables.LoadAssetAsync<SpawnCard>("RoR2/Base/Teleporters/iscTeleporter.asset").WaitForCompletion();
    static SpawnCard imp = Addressables.LoadAssetAsync<SpawnCard>("RoR2/Base/Imp/cscImp.asset").WaitForCompletion();
    static SpawnCard lemurian = Addressables.LoadAssetAsync<SpawnCard>("RoR2/Base/Lemurian/cscLemurian.asset").WaitForCompletion();
    static SpawnCard vulture = Addressables.LoadAssetAsync<SpawnCard>("RoR2/Base/Vulture/cscVulture.asset").WaitForCompletion();
    static SpawnCard greaterWisp = Addressables.LoadAssetAsync<SpawnCard>("RoR2/Base/GreaterWisp/cscGreaterWisp.asset").WaitForCompletion();
    static SpawnCard parent = Addressables.LoadAssetAsync<SpawnCard>("RoR2/Base/Parent/cscParent.asset").WaitForCompletion();
    static SpawnCard jailer = Addressables.LoadAssetAsync<SpawnCard>("RoR2/DLC1/VoidJailer/cscVoidJailer.asset").WaitForCompletion();
    static SpawnCard impBoss = Addressables.LoadAssetAsync<SpawnCard>("RoR2/Base/ImpBoss/cscImpBoss.asset").WaitForCompletion();
    static SpawnCard grovetender = Addressables.LoadAssetAsync<SpawnCard>("RoR2/Base/Gravekeeper/cscGravekeeper.asset").WaitForCompletion();
    static SpawnCard devastator = Addressables.LoadAssetAsync<SpawnCard>("RoR2/DLC1/VoidMegaCrab/cscVoidMegaCrab.asset").WaitForCompletion();
    static SpawnCard scav = Addressables.LoadAssetAsync<SpawnCard>("RoR2/Base/Scav/cscScav.asset").WaitForCompletion();

    public void Awake()
    {
      On.RoR2.VoidStageMissionController.Start += VoidStageMissionControllerStart;
      On.RoR2.ClassicStageInfo.Start += ClassicStageInfoStart;
      On.RoR2.CombatDirector.Awake += CombatDirectorAwake;
      EditSceneDef();
      EditMonsters();
    }

    private void EditSceneDef()
    {
      locusInteractables.categories = abyssalInteractables.categories;
      locusDef.stageOrder = abyssalDef.stageOrder;
      locusDef.bossTrack = abyssalDef.bossTrack;
      locusDef.isOfflineScene = abyssalDef.isOfflineScene;
      locusDef.sceneType = SceneType.Stage;
      locusDef.blockOrbitalSkills = false;
      locusDef.destinationsGroup = abyssalDef.destinationsGroup;
    }

    private void AddLocusToStage4Collection()
    {
      List<SceneCollection.SceneEntry> sceneEntries = stage4SceneCollection._sceneEntries.ToList();
      sceneEntries.Add(new SceneCollection.SceneEntry { sceneDef = locusDef, weightMinusOne = 0 });
      stage4SceneCollection._sceneEntries = sceneEntries.ToArray();
    }

    private void EditMonsters()
    {
      jailer.directorCreditCost = 300; // vanilla 450
      devastator.directorCreditCost = grovetender.directorCreditCost;
      DirectorCardCategorySelection.Category basicMonsters = new DirectorCardCategorySelection.Category();
      basicMonsters.name = "Basic Monsters";
      basicMonsters.cards = new DirectorCard[] { new DirectorCard() { spawnCard = imp, selectionWeight = 2 }, new DirectorCard() { spawnCard = lemurian, selectionWeight = 2 }, new DirectorCard() { spawnCard = vulture, selectionWeight = 1 } };
      basicMonsters.selectionWeight = 3;
      DirectorCardCategorySelection.Category minibosses = new DirectorCardCategorySelection.Category();
      minibosses.name = "Minibosses";
      minibosses.cards = new DirectorCard[] { new DirectorCard() { spawnCard = greaterWisp, selectionWeight = 1 }, new DirectorCard() { spawnCard = parent, selectionWeight = 1 }, new DirectorCard() { spawnCard = jailer, selectionWeight = 1 } };
      minibosses.selectionWeight = 2;
      DirectorCardCategorySelection.Category champions = new DirectorCardCategorySelection.Category();
      champions.name = "Champions";
      champions.cards = new DirectorCard[] { new DirectorCard() { spawnCard = impBoss, selectionWeight = 1 }, new DirectorCard() { spawnCard = grovetender, selectionWeight = 1 }, new DirectorCard() { spawnCard = devastator, selectionWeight = 1 } };
      champions.selectionWeight = 2;
      DirectorCardCategorySelection.Category special = new DirectorCardCategorySelection.Category();
      special.name = "Special";
      special.cards = new DirectorCard[] { new DirectorCard() { spawnCard = scav, selectionWeight = 1 } };
      special.selectionWeight = 1;
      DirectorCardCategorySelection.Category[] newCategories = { basicMonsters, minibosses, champions, special };
      locusMonsters.categories = newCategories;
    }

    private void ClassicStageInfoStart(On.RoR2.ClassicStageInfo.orig_Start orig, RoR2.ClassicStageInfo self)
    {
      if (SceneCatalog.GetSceneDefFromSceneName("voidstage") == SceneCatalog.currentSceneDef)
        self.sceneDirectorInteractibleCredits = 400;
      orig(self);
    }

    private void CombatDirectorAwake(On.RoR2.CombatDirector.orig_Awake orig, RoR2.CombatDirector self)
    {
      if (!NetworkServer.active)
        return;
      if (SceneCatalog.GetSceneDefFromSceneName("voidstage") == SceneCatalog.currentSceneDef)
        self.teamIndex = TeamIndex.Monster;
      orig(self);
    }

    private void VoidStageMissionControllerStart(On.RoR2.VoidStageMissionController.orig_Start orig, RoR2.VoidStageMissionController self)
    {
      if (NetworkServer.active)
      {
        self.batteryCount = 0;
        DirectorCore instance = DirectorCore.instance;
        DirectorPlacementRule placementRule = new DirectorPlacementRule();
        placementRule.placementMode = DirectorPlacementRule.PlacementMode.Random;
        Xoroshiro128Plus rng = RoR2Application.rng;
        DirectorSpawnRequest directorSpawnRequest = new DirectorSpawnRequest(teleporter, placementRule, rng);
        instance.TrySpawnObject(directorSpawnRequest);

        DirectorPlacementRule placementRule2 = new DirectorPlacementRule();
        placementRule2.placementMode = DirectorPlacementRule.PlacementMode.Direct;
        placementRule2.position = new Vector3(-213.5f, 44.8f, 65);
        DirectorSpawnRequest directorSpawnRequest2 = new DirectorSpawnRequest(goldChest, placementRule2, rng);
        directorSpawnRequest2.onSpawnedServer += (Action<SpawnCard.SpawnResult>)(spawnResult =>
        {
          spawnResult.spawnedInstance.transform.eulerAngles = new Vector3(0.0f, 80f, 0.0f);
          spawnResult.spawnedInstance.GetComponent<PurchaseInteraction>().Networkcost = Run.instance.GetDifficultyScaledCost(400);
        });
        instance.TrySpawnObject(directorSpawnRequest2);

        GameObject bortal = Instantiate<GameObject>(newtStatue, new Vector3(154.5f, 45.6f, -145), Quaternion.identity);
        NetworkServer.Spawn(bortal);
      }
      orig(self);
    }
  }
}