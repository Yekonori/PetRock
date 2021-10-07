using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ViewVolumeBlender : MonoBehaviour
{
    [SerializeField] bool debugMode = false;

    private List<AViewVolume> activeViewVolumes = new List<AViewVolume>();
    private Dictionary<AView, List<AViewVolume>> volumesPerViews = new Dictionary<AView, List<AViewVolume>>();

    public static ViewVolumeBlender instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        Blend();
    }

    void Blend()
    {
        activeViewVolumes = SortMyList(activeViewVolumes);

        Dictionary<int, float> weightSum = new Dictionary<int, float>();
        Dictionary<int, float> availableWeight = new Dictionary<int, float>();
        float remainingWeight = 1f;

        List<int> listPrio = new List<int>();
        foreach (AViewVolume volume in activeViewVolumes)
        {
            if (!listPrio.Contains(volume.priority))
                listPrio.Add(volume.priority);
            volume.view.weight = 0f;
        }

        foreach (int prio in listPrio)
        {
            float maxWeight = 0;
            weightSum[prio] = 0f;
            foreach (AViewVolume volume in activeViewVolumes)
            {
                if (volume.priority == prio) {
                    weightSum[prio] += volume.ComputeSelfWeight();
                    if (maxWeight < volume.ComputeSelfWeight())
                    {
                        maxWeight = volume.ComputeSelfWeight();
                    }
                }
            }
            availableWeight[prio] = remainingWeight * maxWeight;
            remainingWeight *= (1 - maxWeight);
        }

        foreach (AViewVolume volume in activeViewVolumes)
        {
            volume.view.weight = Mathf.Max(volume.view.weight, volume.ComputeSelfWeight() * availableWeight[volume.priority] / weightSum[volume.priority]);
        }
    }

    public void AddVolume(AViewVolume volume)
    {
        activeViewVolumes.Add(volume);
        if (!volumesPerViews.ContainsKey(volume.view))
        {
            volume.view.SetActive(true);
            volumesPerViews[volume.view] = new List<AViewVolume>();
        }
        volumesPerViews[volume.view].Add(volume);

        Blend();
    }

    public void RemoveVolume(AViewVolume volume)
    {
        activeViewVolumes.Remove(volume);
        volumesPerViews[volume.view].Remove(volume);
        if (volumesPerViews[volume.view].Count == 0)
        {
            volume.view.SetActive(false);
            volumesPerViews.Remove(volume.view);
        }
    }

    private List<AViewVolume> SortMyList(List<AViewVolume> liste)
    {
        return liste.OrderBy(vol => -vol.priority).ToList();
    }


    private void OnGUI()
    {
        if (!debugMode) return;

        GUILayout.BeginVertical();
        foreach (AViewVolume volume in activeViewVolumes)
        {
            GUILayout.Label("Volume Name : " + volume.name);
            
        }
        GUILayout.EndVertical();
    }

    //public static void Label(Rect position, string text);
}
