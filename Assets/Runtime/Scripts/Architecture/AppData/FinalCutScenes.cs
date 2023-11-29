using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public class FinalCutScenes
    {
        [SerializeField] private SlideShow _goodEnd;
        [SerializeField] private SlideShow _almostBadEnd;
        [SerializeField] private SlideShow _badEnd;
        [SerializeField] private SlideShow _escapeFinal;

        public SlideShow GoodEnd => _goodEnd;
        public SlideShow AlmostBadEnd => _almostBadEnd;
        public SlideShow BadEnd => _badEnd;
        public SlideShow EscapeFinal => _escapeFinal; 
    }
}