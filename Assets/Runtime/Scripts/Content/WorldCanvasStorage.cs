using App.Content.UI;
using UnityEngine;

namespace App.Content
{
    public sealed class WorldCanvasStorage : MonoBehaviour
    {
        [SerializeField] private InteractIcon _interactIcon;

        public InteractIcon InteractIcon => _interactIcon;
    }
}