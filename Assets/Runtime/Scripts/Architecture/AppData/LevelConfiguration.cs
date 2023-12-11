using SimpleComponents.UI;
using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public sealed class LevelConfiguration
    {
        [SerializeField] private SCSlideShow _storageDialogs;
        [SerializeField] private SCSlideShow _storageDialogeWithTip;
        [SerializeField] private SCSlideShow _scarecrowDialogs;
        [SerializeField] private SCSlideShow _cutScene;
        [SerializeField] private float _dayTimeRange;

        public SCSlideShow CutScene => _cutScene;
        public SCSlideShow ScarecrowDialogs => _scarecrowDialogs;
        public SCSlideShow StorageDialogs => _storageDialogs;
        public float DayTimeRange => _dayTimeRange;
        public SCSlideShow StorageDialogeWithTip => _storageDialogeWithTip;
    }
}