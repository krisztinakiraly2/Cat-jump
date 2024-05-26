using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Game : MonoBehaviour
{
    public ControlGame ControlGame;
    public GameObject[][] playfield;
    public ArrayList clouds;
    public ArrayList beds;
    public static float Height;
    bool repeatBeds = false;
    bool repeatClouds = false;

    float shift_const = 108;
    float height_const = 250;
    float currendMaxHeigth = 1100;
    float threeItemRowStartingXCoord = 200;
    float fourItemRowStartingXCoord = -50;
    float coloumnDistance = 500;
    int menu = 6;
    int playfieldLength = 6;
    const int cloud = 0;
    const int bed = cloud+1;
    int startingCounfOfExtras = 4;
    float extraBedsStartingXCoord = 2520;
    float extraCloudsStartingXCoord = 2230;
    float extraItemsStartingYCoord = 920;

    void Start()
    {
        playfield = new GameObject[playfieldLength][];
            
        playfield[0] = new GameObject[3];
        playfield[1] = new GameObject[4];
        playfield[2] = new GameObject[3];
        playfield[3] = new GameObject[4];
        playfield[4] = new GameObject[3];

        Vector3 v = new Vector3(threeItemRowStartingXCoord + shift_const, currendMaxHeigth, 0);

        for (int i = 0; i < playfieldLength-1; ++i)
        {
            for (int j = 0; j < playfield[i].Length; ++j)
            {
                playfield[i][j] = new GameObject();

                if ((i == (playfield.Length-2) && j == 1) || (i==0 &&  j==2) || (i==1 && j==2) || (i==2 && j==1) || (i==3 && j==1))
                    playfield[i][j] = createBed(v,true);
                else
                {
                    if (cloud == Random.Range(cloud, bed+1))
                        playfield[i][j] = createCloud(v,true);
                    else
                        playfield[i][j] = createBed(v, true);
                }

                v.x += coloumnDistance;
            }

            if(i%2 != 0)
                v.x = threeItemRowStartingXCoord + shift_const;
            else
                v.x = fourItemRowStartingXCoord + shift_const;

            v.y -= ControllCat.Jump_Height;

        }

        v = new Vector3(extraCloudsStartingXCoord, extraItemsStartingYCoord, 0);
        clouds = new();
        beds = new();

        for(int i = 0;i < startingCounfOfExtras; ++i)
        {
            GameObject obj = createCloud(v, false);
            clouds.Add(obj);
        }

        v.x = extraBedsStartingXCoord;

        for (int i = 0; i < startingCounfOfExtras; ++i)
        {
            GameObject obj = createBed(v, false);
            beds.Add(obj);
        }

        Height = height_const;

    }

    GameObject createCloud(Vector3 v, bool hasParent)
    {
        GameObject o = GameObject.Instantiate(GameObject.FindWithTag("Cloud"), v, Quaternion.identity);
        o.name = "Cloud";
        o.layer = menu;

        if(hasParent)
            o.transform.parent = this.gameObject.transform;

        return o;
    }

    GameObject createBed(Vector3 v, bool hasParent)
    {
        GameObject o = GameObject.Instantiate(GameObject.FindWithTag("Bed"), v, Quaternion.identity);
        o.name = "Bed";
        o.layer = menu;

        if (hasParent)
            o.transform.parent = this.gameObject.transform;

        return o;
    }

    void Update()
    {
        MovePlayingField();
    }

    void MovePlayingField()
    {
        if (ControllCat.MoveFieldDown)
        {
            foreach (var row in playfield)
            {
                if (row != null && row.Length > 0)
                {
                    foreach (var item in row)
                    {
                        if (item != null)
                        {
                            item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y - ControllCat.Jump_Height+10, item.transform.position.z);
                        }
                    }
                }
            }

            ControllCat.MoveFieldDown = false;
            refreshPlayingField();
        }
    }

    void randomizePatterns(int[] array, int length)
    {
        if(playfield[0][0].name=="Bed" && array[0]==bed && array[2]==bed)
        {
            if(repeatBeds)
            {
                array[0]=cloud; array[2]=cloud;
                array[1]=bed;

                if(array.Length>3)
                    array[3]=bed;

                repeatBeds = false;
            }
            else
                repeatBeds = true;
        }

        if(playfield[0][0].name=="Cloud" && array[0]==cloud && array[2]==bed)
        {
            if(repeatClouds)
            {
                array[0]=bed; array[2]=bed;
                array[1]=cloud;

                if(array.Length>3)
                    array[3]=cloud;

                repeatClouds = false;
            }
            else
                repeatClouds = true;
        }

        int bedCount = 0;
        for (int i = 0; i < length; i++)
        {
            if (array[i] == bed)
            {
                ++bedCount;
            }
            else
                array[i] = cloud;

        }

        while (bedCount < Random.Range(2,4))
        {
            int place = Random.Range(0,array.Length);
            if (array[place] != bed)
            {
                array[place] = bed;
                ++bedCount;
            }
        }
    }

    void fixPath(int[] array)
    {
        for(int i=0; i<playfield[0].Length; ++i)
        {
            if(playfield[0][i].name == "Bed")
            {
                if(playfield[0].Length==3)
                {
                    if(array[i]!=bed && array[i+1]!=bed)
                        array[i]=bed;
                }
                else
                {
                    switch(i)
                    {
                        case 1: 
                        case 2: if(array[i-1]!=bed && array[i]!=bed)
                                    array[i]=bed; break;
                        case 0: if(array[i]!=bed)
                                    array[i] = bed; break;
                        case 3: if(array[i-1]!=bed)
                                    array[i-1] = bed; break;
                    }
                }
            }
        }
    }

    void emptyRow5(bool[] row5takens)
    {
        Vector3 vect = new Vector3(extraCloudsStartingXCoord, extraItemsStartingYCoord, 0);
        
        for (int i = 0; i < playfield[5].Length; i++)
        {
            if (!row5takens[i] && playfield[5][i] != null)
            {
                if(playfield[5][i].name == "Cloud")
                {
                    vect.x = extraCloudsStartingXCoord;
                    playfield[5][i].transform.position = vect;
                    clouds.Add(playfield[5][i]);
                }
                else
                {
                    vect.x = extraBedsStartingXCoord;
                    playfield[5][i].transform.position = vect;
                    beds.Add(playfield[5][i]);
                }

                playfield[5][i].transform.parent = null;
                playfield[5][i] = null;
            }
        }

        playfield[5] = null;
    }

    void generatePath(int[] array, int length)
    {
        ArrayList pos = new ArrayList();
        
        for(int j = 0; j < playfield[0].Length; ++j)
        {
            if (playfield[0][j].name == "Bed" && pos.Count<=length)
                pos.Add(j);
        }

        if (pos.Count == 0)
            Debug.LogWarning("No bed. Oh noo");
        else
        {
            if (pos.Count == 1)
            {
                for (int j = 0; j < playfield[0].Length; ++j)
                {
                    if (playfield[0][j].name == "Bed")
                    {
                        int num = 0;
                        if (j < length - 1)
                            num = j + Random.Range(0, 2);
                        else
                            num = length - 1;
                        array[num] = bed;
                    }
                }
            }
            else
            {
                int Count = Random.Range(1, pos.Count+1);

                for (int i = 0;i < Count; ++i) 
                { 
                    int place = Random.Range(0, pos.Count);
                    if ((int)pos[place]<array.Length)
                        array[(int)pos[place]] = bed;
                    pos.RemoveAt(place);
                }
                
            }
        }
    }

    void refreshPlayingField()
    {
        int length = (playfield[0].Length == 4) ? 3 : 4;
        float startingXCoord = (length==4) ? fourItemRowStartingXCoord : threeItemRowStartingXCoord;
        int[] array = new int[length];
        bool[] row5takens = new bool[playfield[4].Length];

        generatePath(array, length);
        fixPath(array);
        randomizePatterns(array, length);

        playfield[5] = playfield[4];
        playfield[4] = playfield[3];
        playfield[3] = playfield[2];
        playfield[2] = playfield[1];
        playfield[1] = playfield[0];

        playfield[0] = new GameObject[length];
        Vector3 v = new Vector3(startingXCoord+shift_const, currendMaxHeigth,0);

        for(int i = 0;i < length;i++)
        {
            if (array[i] == cloud)
            {
                bool noMore = false;
                for (int j = 0; j < playfield[5].Length; j++)
                {
                    if (playfield[5][j] != null && row5takens[j]==false)
                    {
                        if (playfield[5][j].name == "Cloud")
                        {
                            playfield[0][i] = playfield[5][j];
                            row5takens[j] = true;
                            playfield[0][i].transform.position = v;
                            break;
                        }
                    }

                    if (j == playfield[5].Length-1)
                    {
                        noMore = true;
                    }
                }

                if (noMore)
                {
                    bool resolved = false;

                    if (clouds.Count > 0)
                    {
                        playfield[0][i] = (GameObject)clouds[clouds.Count - 1];
                        playfield[0][i].transform.parent = this.gameObject.transform;
                        clouds.Remove(playfield[0][i]);
                        resolved = true;
                    }
                    else
                    {
                        playfield[0][i] = createCloud(v,true);
                        resolved=true;
                    }

                    if (!resolved)
                        Debug.Log($"Oh no still no cloud in coloumn {i}");

                    playfield[0][i].transform.position = v;
                }
            }
            else
            {
                bool noMore = false;
                for (int j = 0; j < playfield[5].Length; j++)
                {
                    if (playfield[5][j] != null && row5takens[j] == false)
                    {
                        if (playfield[5][j].name == "Bed")
                        {
                            playfield[0][i] = playfield[5][j];
                            playfield[0][i].transform.position = v;
                            row5takens[j] = true;
                            break;
                        }
                    }

                    if (j == playfield[5].Length-1)
                    {
                        noMore = true;
                    }

                }

                if (noMore)
                {
                    bool resolved = false;
                    if (beds.Count > 0)
                    {
                        playfield[0][i] = (GameObject)beds[beds.Count - 1];
                        beds.Remove(playfield[0][i]);
                        playfield[0][i].transform.parent = this.gameObject.transform;
                        resolved = true;
                    }
                    else
                    {
                        playfield[0][i] = createBed(v,true);
                        resolved=true;
                    }

                    if (!resolved)
                        Debug.Log($"Oh no still no cloud in coloumn {i}");

                    playfield[0][i].transform.position = v;
                }
            }

            v.x += coloumnDistance;
        }

        emptyRow5(row5takens);
    }

    void Move()
    {
        ControlGame.MoveParentDown();
    }

    public float getLeftColoumnPos(bool isLeft)
    {
        float l = (playfield[4].Length == 4) ? 3 : 4;
        float X = (l == 3) ? threeItemRowStartingXCoord : fourItemRowStartingXCoord;

        if (isLeft)
            X += shift_const+2;
        else
            X += coloumnDistance * (l - 1) + shift_const + 3;

        return X;
    }
}
