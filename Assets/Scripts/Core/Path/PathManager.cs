using Tower;
using UnityEngine;
using UnitySingleton;
using UnityEngine.Pool;
using System.Collections;

namespace Core.PathHandler
{
    public class PathManager : MonoSingleton<PathManager>
    {
        private IObjectPool<Path> lineRendersPool;

        [SerializeField, Disable] private PathDataScriptable pathData;
        private LineRenderer hintLine;

        // todo for when playing online we need photon pool maybe? https://doc.photonengine.com/pun/current/gameplay/instantiation
        private void Init()
        {
            lineRendersPool = new ObjectPool<Path>(OnCreateRenderers, OnGetRenderers, OnReleaseRenderers, OnDestroyRenderers, true, pathData.PoolSize, pathData.MaxPoolSize);
            transform.position = Vector3.zero;
            hintLine = Instantiate(pathData.HintLine);
            hintLine.enabled = false;
            hintLine.transform.position = Vector3.zero;
        }

        public IEnumerator GetPathData()
        {
            yield return StartCoroutine(Util.HelperCoroutine.LoadDataFromResources("Scriptable/PathData",
                (data) =>
                {
                    pathData = data as PathDataScriptable;
                    Init();
                }));
        }

        /// <summary>
        /// Get and set hint line renderer
        /// </summary>
        /// <param name="origin"> origin of path in world coordinate</param>
        public void GetHintLine(Vector3 origin)
        {
            hintLine.enabled = true;
            hintLine.SetPosition(0, new(origin.x, pathData.OffsetY, origin.z));
            hintLine.SetPosition(1, new(origin.x, pathData.OffsetY, origin.z));
        }

        /// <summary>
        /// Update the enpoint of line renderer
        /// </summary>
        /// <param name="currentPosition">current position in world coordinate</param>
        /// <param name="isDrawablePath">whether the final path is drawable or not</param>
        public void UpdateHintLine(Vector3 currentPosition, bool isDrawablePath)
        {
            hintLine.SetPosition(1, new(currentPosition.x, pathData.OffsetY, currentPosition.z));
            if (isDrawablePath)
            {
                //TODO: Set the path color to blue
                hintLine.material = pathData.ValidHintMaterial;
            }
            else
            {
                //TODO: Set the path color to black or red
                hintLine.material = pathData.InValidMaterial;
            }
        }

        /// <summary>
        /// Remove the Hint Line
        /// </summary>
        public void RemoveHintLine()
        {
            hintLine.SetPosition(0, Vector3.zero);
            hintLine.SetPosition(1, Vector3.zero);
            hintLine.enabled = false;
        }

        /// <summary>
        /// Create the Path
        /// </summary>
        /// <param name="from">starting point in world coordinate</param>
        /// <param name="to">ending point in world Coordinate</param>
        /// <returns></returns>
        public Path CreatePath(Vector3 from, Vector3 to, TowerBase tower)
        {
            Path path = lineRendersPool.Get();
            Material pathMat = pathData.ValidHintMaterial;
            for (int i = 0; i < pathData.PathMaterial.Length; i++)
            {
                Util.PathVisual visual = pathData.PathMaterial[i];
                if (visual.owner == tower.TowerOwner)
                {
                    pathMat = visual.material;
                    break;
                }
            }
            path.DrawPath(true, new(to.x, pathData.OffsetY, to.z), new(from.x, pathData.OffsetY, from.z), tower, pathMat);
            return path;
        }

        /// <summary>
        /// Remove the path between tower
        /// </summary>
        /// <param name="path">Which line renderer?</param>
        /// <returns></returns>
        public Path RemovePath(Path path)
        {
            lineRendersPool.Release(path);
            return path;
        }

        #region ObjectPoolingMethods

        private Path OnCreateRenderers()
        {
            return Instantiate(pathData.PathRendererPrefab, Vector3.zero, Quaternion.identity);
        }

        private void OnGetRenderers(Path path)
        {
            path.gameObject.SetActive(true);
            path.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        private void OnReleaseRenderers(Path path)
        {
            path.gameObject.SetActive(false);
        }

        private void OnDestroyRenderers(Path path)
        {
            Destroy(path.gameObject);
        }

        #endregion ObjectPoolingMethods
    }
}