using System;
using System.Threading;
using System.Threading.Tasks;
using ImagineBeingGood.Configuration;
using SiraUtil.Affinity;
using SiraUtil.Attributes;
using SiraUtil.Logging;
using SiraUtil.Zenject;
using SongDetailsCache;
using SongDetailsCache.Structs;
using UnityEngine;

namespace ImagineBeingGood.Affinity_Patches
{
    [Bind(Location.GameCore)]
    internal class LevelFinished : IAffinity, IAsyncInitializable
    {
        private SiraLog _siraLog;
        private readonly PluginConfig _config;
        private readonly GameplayCoreSceneSetupData _gameplayCoreSceneSetupData;
        private static SongDetails? _songDetails;
        
        public LevelFinished(SiraLog siraLog, PluginConfig config, GameplayCoreSceneSetupData gameplayCoreSceneSetupData)
        {
            _siraLog = siraLog;
            _config = config;
            _gameplayCoreSceneSetupData = gameplayCoreSceneSetupData;
        }

        public async Task InitializeAsync(CancellationToken token)
        {
            if (_songDetails != null) return;
            _songDetails = await SongDetails.Init();
        }
        
        [AffinityPrefix]
        [AffinityPatch(typeof(StandardLevelScenesTransitionSetupDataSO), nameof(StandardLevelScenesTransitionSetupDataSO.Finish))]
        internal bool CheckCriteria(LevelCompletionResults levelCompletionResults)
        {
            if (_songDetails == null) return true;
            var acc = levelCompletionResults.totalCutScore / (float)levelCompletionResults.maxCutScore / 100f;
            if (acc < _config.TargetAccuracy) return true;

            var diffBeatmap = _gameplayCoreSceneSetupData.difficultyBeatmap;
            if (!_songDetails.songs.FindByHash(diffBeatmap.level.levelID.Replace("custom_level_", ""), out var song)) return true;
            if (!song.GetDifficulty(out var diff, (MapDifficulty)diffBeatmap.difficulty, (MapCharacteristic)GetCharacteristic(diffBeatmap))) return true;
            if (diff.stars < _config.TargetStarDifficulty) return true;

            var rand = new System.Random();
            
            if (rand.NextDouble() <= _config.Chance)
                Application.Quit(0);
            return false;
        }

        private static int GetCharacteristic(IDifficultyBeatmap diffBeatmap)
        {
            var so = diffBeatmap.parentDifficultyBeatmapSet?.beatmapCharacteristic.sortingOrder;
            if (so == null || so > 4) return 0;
            if (so == 3) 
                so = 4;
            else if (so == 4) 
                so = 4;

            return (int)so + 1;
        }
    }
}