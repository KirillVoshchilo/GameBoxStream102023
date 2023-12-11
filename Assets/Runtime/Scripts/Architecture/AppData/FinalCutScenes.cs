using SimpleComponents.UI;
using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public sealed class FinalCutScenes
    {
        [SerializeField] private SCSlideShow _goodEnd;
        [SerializeField] private SCSlideShow _almostBadEnd;
        [SerializeField] private SCSlideShow _badEnd;
        [SerializeField] private SCSlideShow _escapeFinal;

        public SCSlideShow GoodEnd => _goodEnd;
        public SCSlideShow AlmostBadEnd => _almostBadEnd;
        public SCSlideShow BadEnd => _badEnd;
        public SCSlideShow EscapeFinal => _escapeFinal;
    }
}