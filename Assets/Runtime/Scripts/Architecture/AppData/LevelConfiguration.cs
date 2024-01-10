using App.Content.UI;
using SimpleComponents.UI;
using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public sealed class LevelConfiguration
    {
        [SerializeField] private Dialogue _storageDialogs;
        [SerializeField] private Dialogue _storageDialogeWithTip;
        [SerializeField] private Dialogue _scarecrowDialogs;
        [SerializeField] private SCSlideShow _cutScene;
        [SerializeField] private float _dayTimeRange;

        public SCSlideShow CutScene => _cutScene;
        public Dialogue ScarecrowDialogs => _scarecrowDialogs;
        public Dialogue StorageDialogs => _storageDialogs;
        public Dialogue StorageDialogeWithTip => _storageDialogeWithTip;
        public float DayTimeRange => _dayTimeRange;
    }
}