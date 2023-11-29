using App.Architecture;
using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content.Entities;
using App.Content.UI.WorldCanvases;
using App.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class ScarecrowData
{
    [SerializeField] private Transform _interactionIconTransform;
    [SerializeField] private GameObject _modelParent;
    [SerializeField] private GameObject _editorTemporaryMesh;

    private readonly float _interactTime = 0;
    private readonly InteractionComp _interactableComp = new();
    private WorldCanvasStorage _worldCanvasStorage;
    private UIController _uiController;
    private IAppInputSystem _appInputSystem;
    private GameObject _scarecrowModel;

    public WorldCanvasStorage WorldCanvasStorage { get => _worldCanvasStorage; set => _worldCanvasStorage = value; }
    public IAppInputSystem AppInputSystem { get => _appInputSystem; set => _appInputSystem = value; }
    public InteractionComp InteractableComp => _interactableComp;
    public UIController UIController { get => _uiController; set => _uiController = value; }
    public Vector3 InteractionIconPosition => _interactionIconTransform.position;
    public InteractIcon InteractIcon => _worldCanvasStorage.InteractIcon;
    public float InteractTime => _interactTime;
    public GameObject ScarecrowModel { get => _scarecrowModel; set => _scarecrowModel = value; }
    public GameObject ModelParent => _modelParent;
    public GameObject EditorTemporaryMesh  => _editorTemporaryMesh; 
}
