using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class input_controller : MonoBehaviour
{
    double[][] input = new double[][] { }, input_6D = new double[][] { };
    double[][] map_line = new double[][] { };

    Vector3 initpos = new Vector3();
    Vector2[] end = new Vector2[2] {new Vector2(),new Vector2() };
    // Start is called before the first frame update
    void Awake()
    {
        set_map();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void set_map()
    {
        string[] line;
        string[] line_arr;
        int len;
        string txt = File.ReadAllText("./Assets/line.txt");
        TextAsset ttxt = Resources.Load("line") as TextAsset;
        line = txt.Split('\n');
        line_arr = line[0].Split(",");
        for (int i = 0; i < 3; i++) initpos[i] = float.Parse(line_arr[i]);
        for (int t = 0; t < 2; t++)
        {
            line_arr = line[1 + t].Split(",");
            for (int i = 0; i < 2; i++)
            {
                end[t][i] = float.Parse(line_arr[i]);
            }
        }
        len = int.Parse(line[3]);
        Array.Resize(ref map_line, len);
        //Continue to read until you reach end of file
        for (int i = 0; i < len; i++)
        {
            line_arr = line[i + 4].Split(",");
            Array.Resize(ref map_line[i], line_arr.Length);
            for (int j = 0; j < line_arr.Length; j++)
            {
                map_line[i][j] = double.Parse(line_arr[j]);
            }
        }
    }



    public double[][] get_line()
    {

        return map_line;
    }
    public double[][] getinput()
    {
        return input;
    }
    public Vector3 get_initpos()
    {
        return initpos;
    }
    public Vector2[] getend()
    {
        return end;
    }
    public double[][] getinput6D (){
        return input_6D;
    }
}
