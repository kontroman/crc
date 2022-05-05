using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GetByViewID : MonoBehaviour
{
    public static GetByViewID Instance { get; private set; }

    private List<PhotonView> views = new List<PhotonView>();

    [SerializeField]
    private float timeToUpdate = 1f;
    private float timer;

    private void Start()
    {
        if (Instance != null) return;
        else Instance = this;

        timer = timeToUpdate;
        SetupViews();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            timer = timeToUpdate;
            SetupViews();
        }
    }

    private void SetupViews()
    {
        views.Clear();

        views = FindObjectsOfType<PhotonView>().Select(_pv => _pv.GetComponent<PhotonView>()).ToList();
    }

    public PhotonView GetById(int viewID)
    {
        return views.Find(x => x.ViewID == viewID);
    }
}
