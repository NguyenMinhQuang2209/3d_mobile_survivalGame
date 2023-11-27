using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FarmTool : MonoBehaviour
{
    public FarmToolName farmToolName;
    [SerializeField] private int durationTime = 1;
    private int currentUseTime = 0;
    public void BaseUseFarmTool()
    {
        UseFarmTool();
    }
    public virtual void UseFarmTool()
    {

    }
    public int GetDurationTime()
    {
        return durationTime;
    }
    public int GetCurrentUseTime()
    {
        return currentUseTime;
    }
    public FarmToolName GetFarmToolName() { return farmToolName; }
}
