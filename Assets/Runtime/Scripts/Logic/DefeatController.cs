using App.Content.Player;
using VContainer;

public class DefeatController
{
    private readonly HeatData _heatData;
    private readonly LevelTimer _levelTimer;

    public bool IsEnable
    {
        set
        {
            if (value)
                _heatData.OnHeatChanged.AddListener(OnHeatChanged);
            else _heatData.OnHeatChanged.RemoveListener(OnHeatChanged);
        }
    }

    [Inject]
    public DefeatController(PlayerEntity playerEntity,
        LevelTimer levelTimer)
    {
        _heatData = playerEntity.Get<HeatData>();
        _levelTimer = levelTimer;
    }

    private void OnHeatChanged(float obj)
    {
        if (obj <= 0)
        {
            _levelTimer.StopTimer();
        }
    }
}
