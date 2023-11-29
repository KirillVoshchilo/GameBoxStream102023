using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public class LevelConfiguration
    {
        [SerializeField] private SlideShow _storageDialogs;
        [SerializeField] private SlideShow _storageDialogeWithTip;
        [SerializeField] private SlideShow _scarecrowDialogs;
        [SerializeField] private SlideShow _cutScene;
        [SerializeField] private float _dayTimeRange;

        public SlideShow CutScene => _cutScene;
        public SlideShow ScarecrowDialogs => _scarecrowDialogs;
        public SlideShow StorageDialogs => _storageDialogs;
        public float DayTimeRange => _dayTimeRange;
        public SlideShow StorageDialogeWithTip  => _storageDialogeWithTip; 
    }
}