using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PalletFiller))]
[CanEditMultipleObjects]
public class PalletFillerAeditor : Editor
{
    GameObject myPrefab;
    int index = 0;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PalletFiller filler = (PalletFiller)target;
        myPrefab = filler.prefab;
        if (GUILayout.Button("Fill Field"))
        {
        if (filler.active)
        {
            return;
        }
        if (!filler.active)
        {
                //for (int i = 0; i < filler.palletHolderClone.lenght; i++)
                //{

                //}
                //filler.palletHolder[index];

                //GameObject pellet = PrefabUtility.InstantiatePrefab(myPrefab) as GameObject;
                //pellet.transform.position = filler.palletHolderClone[index].transform.position;
                //pellet.transform.parent = filler.palletHolder.transform;
                //index++;

                //if(index == filler.palletHolderClone.lenght)
                //{
                //    index = 0;
                //}
                filler.active = true;
                filler.hCell = (int)Vector3.Distance(new Vector3(filler.topRight.transform.position.x, 0, 0), new Vector3(filler.bottomLeft.transform.position.x, 0, 0));
                filler.vCell = (int)Vector3.Distance(new Vector3(0, 0, filler.topRight.transform.position.z), new Vector3(0, 0, filler.bottomLeft.transform.position.z));

                for (int i = 0; i < filler.hCell; i++)
                {
                    for (int j = 0; j < filler.vCell; j++)
                    {
                        if (Physics.CheckSphere(new Vector3(filler.bottomLeft.transform.position.x + i, filler.bottomLeft.transform.position.y, filler.bottomLeft.transform.position.z + j), 1f)) ;
                        {
                            GameObject pellet = PrefabUtility.InstantiatePrefab(myPrefab) as GameObject;
                            pellet.transform.position = new Vector3(filler.bottomLeft.transform.position.x + i, filler.bottomLeft.transform.position.y, filler.bottomLeft.transform.position.z + j);
                            pellet.transform.parent = filler.palletHolder.transform;
                            //GameObject pellet = Instantiate(prefab, new Vector3(bottomLeft.transform.position.x + i, bottomLeft.transform.position.y, bottomLeft.transform.position.z + j), Quaternion.identity, palletHolder.transform) as GameObject;
                        }
                    }
                }
            }
        }
    }
}
