using Photon.Pun;
using System;
using Tower;
using UnityEngine;

namespace Core.PathHandler
{
    public class Path : MonoBehaviourPunCallbacks,IPunObservable
    {
        private LineRenderer lineRenderer;
        [field: SerializeField, Disable] public string ID { get; private set; }

        [field: SerializeField, Disable] public TowerBase TowerPathOwner { get; private set; }

        [Serializable]
        public struct Data
        {
            public bool useWorldSpace;
            public Vector3 start;
            public Vector3 end;
            public TowerBase creatorTower;
            public Material pathMat;

            public Data(bool useWorldSpace, Vector3 start, Vector3 end, TowerBase creatorTower, Material pathMat)
            {
                this.useWorldSpace = useWorldSpace;
                this.start = start;
                this.end = end;
                this.creatorTower = creatorTower;
                this.pathMat = pathMat;
            }
        }

        //[SerializeField] private int pathCleanerInterval = 10;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void Start()
        {
            System.Guid guid = System.Guid.NewGuid();
            ID = guid.ToString()[..8];
        }

        public void DrawPath(bool useWorldSpace, Vector3 start, Vector3 end, TowerBase pathCreatorTower, Material pathMat)
        {
            Debug.Log("Start : " + start + " End : " + end + " Creator : " + pathCreatorTower);
            photonView.RPC("RPC_DrawPath", RpcTarget.All, useWorldSpace, start, end, pathCreatorTower.gameObject.GetPhotonView().ViewID,
             new Vector3(pathMat.GetColor("_MainColor").r, pathMat.GetColor("_MainColor").g, pathMat.GetColor("_MainColor").b));
            // RPC_DrawPath(useWorldSpace, start, end, pathCreatorTower, pathMat);
        }
        [PunRPC]
        private void RPC_DrawPath(bool useWorldSpace, Vector3 start, Vector3 end, int pathCreatorTower, Vector3 pathMat)
        {
            lineRenderer.useWorldSpace = useWorldSpace;
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
            GenerateCollider(start, end);
            TowerPathOwner = PhotonView.Find(pathCreatorTower).GetComponent<TowerBase>();
            Color color = new Color(pathMat.x, pathMat.y, pathMat.z);
            lineRenderer.material.SetColor("_BaseColor", color);
        }

        private void GenerateCollider(Vector3 from, Vector3 to)
        {
            BoxCollider box = GetComponentInChildren<BoxCollider>();
            Vector3 dir = to - from;
            box.transform.position = (from + to) / 2;
            box.transform.forward = dir.normalized;
            box.size = new(0.1f, 0.1f, dir.magnitude);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            // throw new NotImplementedException();
        }

        // public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        // {
        //     if (stream.IsWriting)
        //     {
        //         stream.SendNext(new Data(true, lineRenderer.GetPosition(0), lineRenderer.GetPosition(1), TowerPathOwner, lineRenderer.material));
        //     }
        //     else
        //     {
        //         Data data = (Data)stream.ReceiveNext();
        //         this.lineRenderer.useWorldSpace = data.useWorldSpace;
        //         this.lineRenderer.SetPosition(0, data.start);
        //         this.lineRenderer.SetPosition(1, data.end);
        //         this.TowerPathOwner = data.creatorTower;
        //         this.lineRenderer.material = data.pathMat;
        //     }
        // }

        /*    private void Update()
            {
                if (Time.frameCount % pathCleanerInterval == 0)
                {
                    PathCheck();
                }
            }*/

        /*    private void PathCheck()
            {
                int index = ConnectedManager.FindPathIndex(GetComponent<LineRenderer>());
                if (index != -1)
                {
                    return;
                }
                PathManager.Instance.RemovePath(GetComponent<LineRenderer>());
            }*/
    }
}