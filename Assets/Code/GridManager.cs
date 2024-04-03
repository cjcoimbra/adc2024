using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetGridPosition {
    public bool isPassable;
    public int code;
}

public class GridManager : MonoBehaviour
{
    private int[,] firstLevelGrid = new int[,] {
        {2,2,2,2,2,8,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,7,0,0,2,8,2,1,9,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,2},
        {2,0,0,0,2,2,2,0,2,2,2,0,7,2,0,0,2,2,2,2,2,2,2,2,0,2},
        {2,0,7,0,9,0,0,0,0,0,2,0,0,2,0,7,2,2,2,9,7,0,0,2,0,2},
        {2,0,0,0,2,0,0,0,0,0,2,0,0,2,0,0,2,2,0,3,0,0,0,3,0,2},
        {2,3,2,2,2,0,0,7,0,0,2,2,3,2,2,3,2,2,0,2,0,0,0,2,0,2},
        {2,0,7,0,3,0,0,0,0,0,3,0,0,0,0,0,2,2,0,2,2,3,2,2,0,2},
        {2,3,2,2,2,0,0,7,0,0,2,2,2,3,2,2,2,2,0,2,0,0,0,2,0,2},
        {2,0,0,0,2,0,0,0,0,0,2,0,0,0,0,0,2,2,0,9,0,7,0,2,0,2},
        {2,0,7,0,2,0,0,0,0,0,2,0,0,7,0,0,2,2,0,2,7,0,7,2,0,2},
        {2,0,0,0,2,3,2,2,2,3,2,0,0,0,0,7,3,0,7,2,2,2,2,2,0,2},
        {2,0,0,0,2,0,2,8,2,0,9,0,0,7,0,0,2,2,0,2,0,0,0,6,0,2},
        {2,2,2,2,2,0,2,8,2,0,2,2,2,2,2,2,2,2,0,2,0,0,0,2,0,2},
        {2,0,0,0,0,0,2,8,2,0,9,0,7,0,7,0,2,2,0,2,2,3,2,2,0,2},
        {2,0,2,2,2,2,2,8,2,0,2,0,0,0,0,0,2,2,0,2,2,0,2,2,0,2},
        {2,0,2,8,8,8,8,8,2,3,2,0,0,0,0,0,2,9,0,2,2,7,2,2,7,2},
        {2,0,2,8,8,8,8,8,2,0,0,0,0,0,0,0,2,2,0,2,0,0,0,2,0,2},
        {2,3,2,2,2,2,2,2,2,0,0,7,0,0,7,0,2,2,0,2,0,2,0,2,0,2},
        {2,0,0,0,0,0,7,0,2,0,0,0,0,0,0,0,3,0,0,2,0,2,0,2,0,2},
        {9,0,0,0,7,0,0,0,2,0,0,7,0,0,0,0,2,2,0,2,3,2,3,2,0,2},
        {2,7,0,0,0,7,0,0,9,0,0,0,0,0,0,0,2,2,0,0,7,0,0,0,0,2},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2}
    };

    private int[,] originalGrid = new int[,] {
        {2,2,2,2,2,8,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,7,0,0,2,8,2,1,9,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,2},
        {2,0,0,0,2,2,2,0,2,2,2,0,7,2,0,0,2,2,2,2,2,2,2,2,0,2},
        {2,0,7,0,9,0,0,0,0,0,2,0,0,2,0,7,2,2,2,9,7,0,0,2,0,2},
        {2,0,0,0,2,0,0,0,0,0,2,0,0,2,0,0,2,2,0,3,0,0,0,3,0,2},
        {2,3,2,2,2,0,0,7,0,0,2,2,3,2,2,3,2,2,0,2,0,0,0,2,0,2},
        {2,0,7,0,3,0,0,0,0,0,3,0,0,0,0,0,2,2,0,2,2,3,2,2,0,2},
        {2,3,2,2,2,0,0,7,0,0,2,2,2,3,2,2,2,2,0,2,0,0,0,2,0,2},
        {2,0,0,0,2,0,0,0,0,0,2,0,0,0,0,0,2,2,0,9,0,7,0,2,0,2},
        {2,0,7,0,2,0,0,0,0,0,2,0,0,7,0,0,2,2,0,2,7,0,7,2,0,2},
        {2,0,0,0,2,3,2,2,2,3,2,0,0,0,0,7,3,0,7,2,2,2,2,2,0,2},
        {2,0,0,0,2,0,2,8,2,0,9,0,0,7,0,0,2,2,0,2,0,0,0,6,0,2},
        {2,2,2,2,2,0,2,8,2,0,2,2,2,2,2,2,2,2,0,2,0,0,0,2,0,2},
        {2,0,0,0,0,0,2,8,2,0,9,0,7,0,7,0,2,2,0,2,2,3,2,2,0,2},
        {2,0,2,2,2,2,2,8,2,0,2,0,0,0,0,0,2,2,0,2,2,0,2,2,0,2},
        {2,0,2,8,8,8,8,8,2,3,2,0,0,0,0,0,2,9,0,2,2,7,2,2,7,2},
        {2,0,2,8,8,8,8,8,2,0,0,0,0,0,0,0,2,2,0,2,0,0,0,2,0,2},
        {2,3,2,2,2,2,2,2,2,0,0,7,0,0,7,0,2,2,0,2,0,2,0,2,0,2},
        {2,0,0,0,0,0,7,0,2,0,0,0,0,0,0,0,3,0,0,2,0,2,0,2,0,2},
        {9,0,0,0,7,0,0,0,2,0,0,7,0,0,0,0,2,2,0,2,3,2,3,2,0,2},
        {2,7,0,0,0,7,0,0,9,0,0,0,0,0,0,0,2,2,0,0,7,0,0,0,0,2},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2}
    };

    public GameObject wallPrefab;
    public GameObject[] randomWallPrefabs;
    public GameObject floorPrefab;
    public GameObject monsterPrefab;
    public GameObject ceilingPrefab;
    public GameObject doorPrefab;
    private List<GameObject> monsters;
    private Vector3 currentPartyPosition;
    private int medkitWallIndex = 0;
    private int terminalWallIndex = 1;
    void Start() {
        createGrid();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void ReportPartyTurnOver(Vector3 partyPosition) {
        currentPartyPosition = partyPosition;
        StartCoroutine(DelayedMoveMonsters(partyPosition));
    }

    IEnumerator DelayedMoveMonsters(Vector3 partyPosition) {
        for ( int i = 0; i < monsters.Count; i++ ) {
            yield return new WaitForSeconds(0.1f);
            if (i >= monsters.Count) break;
            monsters[i].GetComponent<MonsterController>().ReportPartyDidMove(partyPosition);
        }
    }

    public TargetGridPosition IsPassable(Vector3 desiredPosition) {     
        int[] nonPassableCodes = {2, 3, 6, 9, 7};
        TargetGridPosition targetGridPosition = new TargetGridPosition();
        int code = firstLevelGrid[(int)desiredPosition.x, (int)desiredPosition.z];
        targetGridPosition.isPassable = !nonPassableCodes.Contains(code);
        targetGridPosition.code = code;
        return targetGridPosition;
    }

    public void ReportOpenedDoor(Vector3 doorGridPosition) {
        int x = (int)doorGridPosition.x;
        int z = (int)doorGridPosition.z;
        if (x >= 0 && x < firstLevelGrid.GetLength(0) && z >= 0 && z < firstLevelGrid.GetLength(1)) {
            firstLevelGrid[x, z] = 4;
        }
    }

    private void createGrid() {
        monsters = new List<GameObject>();
        for (int mapWidth = 0; mapWidth < firstLevelGrid.GetLength(0); mapWidth++) {
            for (int mapDepth = 0; mapDepth < firstLevelGrid.GetLength(1); mapDepth++) {
                if (firstLevelGrid[mapWidth, mapDepth] == 2) {
                    int randomWallPrefabIndex = Random.RandomRange(0,20);
                    GameObject sortedWallPrefab = randomWallPrefabs[randomWallPrefabIndex];
                    GameObject.Instantiate(sortedWallPrefab, new Vector3(mapWidth, 1, mapDepth), sortedWallPrefab.transform.rotation);
                } else if (firstLevelGrid[mapWidth, mapDepth] == 9) {  
                    GameObject wall = GameObject.Instantiate(wallPrefab, new Vector3(mapWidth, 1, mapDepth), wallPrefab.transform.rotation);
                    wall.transform.GetChild(medkitWallIndex).gameObject.SetActive(true);
                } else if (firstLevelGrid[mapWidth, mapDepth] == 6) {  
                    GameObject wall = GameObject.Instantiate(wallPrefab, new Vector3(mapWidth, 1, mapDepth), wallPrefab.transform.rotation);
                    wall.transform.GetChild(terminalWallIndex).gameObject.SetActive(true);
                } else if (firstLevelGrid[mapWidth, mapDepth] == 3) {  
                    GameObject.Instantiate(doorPrefab, new Vector3(mapWidth, 1, mapDepth), wallPrefab.transform.rotation);
                } else if (firstLevelGrid[mapWidth, mapDepth] == 1) {
                    GameObject.Find("Party").GetComponent<GridMovement>().SetInitialPosition(new Vector3(mapWidth, 1, mapDepth)); 
                } else if (firstLevelGrid[mapWidth, mapDepth] == 7) {  
                    GameObject monster = GameObject.Instantiate(monsterPrefab, new Vector3(mapWidth, 1, mapDepth), monsterPrefab.transform.rotation);
                    monsters.Add(monster);
                }
                GameObject.Instantiate(floorPrefab, new Vector3(mapWidth, 0, mapDepth), floorPrefab.transform.rotation);
                GameObject.Instantiate(ceilingPrefab, new Vector3(mapWidth, 2, mapDepth), ceilingPrefab.transform.rotation); 
            }    
        }
    }

    public Vector3 GetEmptyGridPositionAround(Vector3 pivot) {
        List<Vector3> emptyGridPositions = new List<Vector3>();
        if (pivot.x > 0) {
            Vector3 target = new Vector3(pivot.x - 1, 1, pivot.z);
            if (IsPassable(target).isPassable && target != currentPartyPosition) {
                emptyGridPositions.Add(target);
            } else if (target == currentPartyPosition) {
                return new Vector3(-2, -2, -2);
            }
        }
        if (pivot.x < (firstLevelGrid.GetLength(0) - 1)) {
            Vector3 target = new Vector3(pivot.x + 1, 1, pivot.z);
            if (IsPassable(target).isPassable && target != currentPartyPosition) {
                emptyGridPositions.Add(target);
            } else if (target == currentPartyPosition) {
                return new Vector3(-2, -2, -2);
            }
        }
        if (pivot.z > 0) {
            Vector3 target = new Vector3(pivot.x, 1, pivot.z - 1);
            if (IsPassable(target).isPassable && target != currentPartyPosition) {
                emptyGridPositions.Add(target);
            } else if (target == currentPartyPosition) {
                return new Vector3(-2, -2, -2);
            }
        }
        if (pivot.z < (firstLevelGrid.GetLength(1) - 1)) {
            Vector3 target = new Vector3(pivot.x, 1, pivot.z + 1);
            if (IsPassable(target).isPassable && target != currentPartyPosition) {
                emptyGridPositions.Add(target);
            } else if (target == currentPartyPosition) {
                return new Vector3(-2, -2, -2);
            }
        }
        if (emptyGridPositions.Count > 0) {
            int randomGridPositionToMove = Random.Range(0, emptyGridPositions.Count);
            return SetGridPositionOccupiedByMonster(emptyGridPositions[randomGridPositionToMove], pivot);
        }
        
        return new Vector3(-1, -1, -1);
    }

    private Vector3 SetGridPositionOccupiedByMonster(Vector3 target, Vector3 origin) {
        firstLevelGrid[(int)target.x, (int)target.z] = 7;
        firstLevelGrid[(int)origin.x, (int)origin.z] = 0;

        if (originalGrid[(int)origin.x, (int)origin.z] == 1) {
            firstLevelGrid[(int)origin.x, (int)origin.z] = 1;
        }
        return target;
    }

    public void ForceMonstersToFaceParty(float partyRotation) {
        for (int i = 0; i < monsters.Count; i++ ) {
            monsters[i].GetComponent<MonsterController>().FaceParty(partyRotation);
        }
    }

    public void ReportMonsterDied(Vector3 monsterGridPosition) {
        for (int i = 0; i < monsters.Count; i++ ) {
            if (monsters[i].transform.position == monsterGridPosition) {
                monsters.RemoveAt(i);
                firstLevelGrid[(int)monsterGridPosition.x, (int)monsterGridPosition.z] = 0;
                return;
            }
        }
    }
}
