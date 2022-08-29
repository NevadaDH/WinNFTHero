using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mao
{
    public class HexGrid : MonoBehaviour
    {
        public int width = 6;
        public int height = 6;

        public HexCell cellBluePrefab;

        public HexCell cellRedPrefab;

        public Text cellLabelPrefab;

        public bool showText = false;

        public bool buildOnAwake = false;

        Canvas gridCanvas;

        HexCell[] cells;

        private void Awake()
        {
            gridCanvas = GetComponentInChildren<Canvas>();

        }

        private void Start()
        {
            if (buildOnAwake)
            {
                BuildGrid(width, height,true);
            }
        }

      
        public void showCells(bool active)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i].showCell(active);
            }
        }

        public void BuildGrid(int width, int height,bool displayEnemy)
        {
            if (gridCanvas != null)
            {
                foreach (Transform child in gridCanvas.transform)
                {
                    Destroy(child.gameObject);
                }
            }

            if (cells != null)
            {
                for (int i = 0; i < cells.Length; i++)
                {
                    Destroy(cells[i].gameObject);
                }
                cells = null;
            }

            cells = new HexCell[width * height];
            int countHalf = Mathf.RoundToInt((width*height)*0.5f);
            for (int z = 0, i = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool isEnemy = displayEnemy ? i>=countHalf : false;
                    CreateCell(x, z, i++,isEnemy);
                }
            }
        }

        void CreateCell(int x, int z, int i,bool isEnemy)
        {
            Vector3 position;
            position.x = (x + (z * 0.5f - z / 2)) * (HexMetrics.innerRadius * 2f);
            position.y = 0f;
            position.z = z * (HexMetrics.outerRadius * 1.5f);

            //int countHalf = Mathf.RoundToInt((width*height)*0.5f);
            HexCell cell = cells[i] = Instantiate(isEnemy?cellRedPrefab:cellBluePrefab);
            cell.IsEnemy = isEnemy;
            cell.CanPutHero =  !isEnemy;
            cell.transform.SetParent(transform, false);
            cell.transform.localPosition = position;
            cell.Index = i;

            cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);

            if (showText && gridCanvas != null)
            {
                Text label = Instantiate(cellLabelPrefab);
                label.rectTransform.SetParent(gridCanvas.transform, false);
                label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);

                label.text = i.ToString();
            }
        }

        public Vector3 GetCellPosByIndex(int index){
            int x = index%width;
            int z = (index+1)/height;
            Vector3 position = new Vector3();
            position.x = (x + (z * 0.5f - z / 2)) * (HexMetrics.innerRadius * 2f) ;
            position.y = 0f;
            position.z = z * (HexMetrics.outerRadius * 1.5f);
            return position + transform.localPosition;
        }

        public HexCell GetCellByPos(Vector3 position)
        {
            position = transform.InverseTransformPoint(position);
            float x = position.x;
            float z = position.z;
            if(x < -HexMetrics.innerRadius || x > width * HexMetrics.innerRadius * 2f
                || z < -HexMetrics.innerRadius || z > height * HexMetrics.outerRadius * 1.5f){
                return null;
            }
            
            HexCoordinates coordinates = HexCoordinates.FromPosition(position);
            int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
            if (index < cells.Length && index >= 0)
            {
                HexCell cell = cells[index];
                return cell;
            }

            return null;
        }

        public HexCell GetCell(int index){
           if (index < cells.Length && index >= 0)
            {
                HexCell cell = cells[index];
                return cell;
            }
            return null;
        }

        public void PlayForceEffect(){
            Vacuity vacuity = gameObject.GetComponent<Vacuity>();
            if(vacuity!=null){
                vacuity.PlaySceneEffect();
            }
            
        }

        public void StopForceEffect(){
            Vacuity vacuity = gameObject.GetComponent<Vacuity>();
            if(vacuity!=null){
                vacuity.PauseSceneEffect();
            }
        }
    }

}