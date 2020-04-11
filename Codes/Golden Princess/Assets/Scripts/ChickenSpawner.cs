using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChickenSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _Chicken;
    [SerializeField] private GameObject _Floor;
    [SerializeField] private GameObject _ChickenParent;
    private static GameObject Chicken;
    private static GameObject ChickenParent;
    private static GameObject Floor;
    private static LayerMask BlocksMask;
    private static LayerMask CharacterMask;
    private static Vector3 floorPos;
    private static Vector3 floorSize;

    void Start()
    {
        BlocksMask = LayerMask.GetMask("Blocks");
        CharacterMask = LayerMask.GetMask("Character");
        Chicken = _Chicken;
        Floor = _Floor;
        ChickenParent = _ChickenParent;
        Tilemap floorTilemap = Floor.GetComponent<Tilemap>();
        floorPos = floorTilemap.localBounds.min;
        floorSize = floorTilemap.localBounds.size;
    }
    
    public static void CreateChicken()
    {
        var rndX = Random.Range(0f,floorSize.x);
        var rndY = Random.Range(0f,floorSize.y);
        Instantiate(Chicken,floorPos + new Vector3(rndX,rndY,-1f),Quaternion.identity,ChickenParent.transform);
    }
    bool Empty(Vector3 dir, bool boolChar)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position,dir,1f,BlocksMask);
        if (hit.collider != null) {
            return false;
        }
        if (boolChar) {
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position,dir,1f,CharacterMask);
            if (hit2.collider != null) {
                return false;
            }
        }
        return true;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Gihi")) {
            for (int i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).GetComponent<ChickenController>().enabled = true;
            }
        }
    }
}
