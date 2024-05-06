using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class Game : MonoBehaviour
{
    public ControlGame ControlGame;
    public GameObject[][] playfield;
    public ArrayList clouds;
    public ArrayList beds;
    Randomizer randomizer;
    public static float Height;

    float shift_const = 108;
    float height_const = 250;
    float currendMaxHeigth = 1100;
    bool isNewRowCreated = false;
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
    bool firstIteration = true;

    void Start()
    {
        playfield = new GameObject[playfieldLength][];
            
        playfield[0] = new GameObject[3];
        playfield[1] = new GameObject[4];
        playfield[2] = new GameObject[3];
        playfield[3] = new GameObject[4];
        playfield[4] = new GameObject[3];

        randomizer = new Randomizer();
        Vector3 v = new Vector3(threeItemRowStartingXCoord + shift_const, currendMaxHeigth, 0);

        for (int i = 0; i < playfieldLength-1; ++i)
        {
            for (int j = 0; j < playfield[i].Length; ++j)
            {
                playfield[i][j] = new GameObject();

                if (i == (playfield.Length-2) && j == 1)
                    playfield[i][j] = createBed(v,true);
                else
                {
                    if (cloud == randomizer.Next(cloud, bed+1))
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
        if (Height == 0)
        {
            Height = height_const;
            isNewRowCreated = false;
        }

        // Todo: figure out why is this working properly
        if(Height == 10 && !isNewRowCreated)
        {
            newPlayingField();

            currendMaxHeigth += ControllCat.Jump_Height;

            isNewRowCreated = true;

            currendMaxHeigth -= ControllCat.Jump_Height;
        }
    }

    void newPlayingField()
    {
        int length = (playfield[0].Length == 4) ? 3 : 4;
        float startingXCoord = (length==4) ? fourItemRowStartingXCoord : threeItemRowStartingXCoord;

        int[] array = new int[length];
        bool[] row5takens = new bool[playfield[4].Length];
        for (int i = 0; i < length; i++)
        {
            array[i] = randomizer.Next(0,2);
        }
        
        playfield[5] = playfield[4];
        playfield[4] = playfield[3];
        playfield[3] = playfield[2];
        playfield[2] = playfield[1];
        playfield[1] = playfield[0];

        for (int i = 1; i < playfield[0].Length; i++)
            playfield[0][i] = null;

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

        Vector3 vect = new Vector3(extraCloudsStartingXCoord, extraItemsStartingYCoord, 0);

        // Todo: this is not working in the long run
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

    void Move()
    {
        ControlGame.MoveParentDown();
    }
}
