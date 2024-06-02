using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class ANN : MonoBehaviour
{ 
    private static System.Random rng = new System.Random();  
    public input_controller input_Controller;
    List<List<ANN_Node>> node_arr = new List<List<ANN_Node>>();
    //ANN_Node[][] node_arr_2 = new ANN_Node[][] { };
    List<List<double>> y_arr = new List<List<double>>();
    public GameObject text;
    int[] net_stru;
    double[] input = new double[4] { 0,0,0,0};
    double[][] all_input;
    public bool tr = false;
    public int ntype = 0, totype = 0;
    void Start()
    {
        net_stru = Global.net_stru;
        for(int i = 0, len_net = net_stru.Length; i < len_net; i++)
        {
            List<ANN_Node> ann_nodes = new List<ANN_Node>();
            List<double> dou = new List<double>();
            for(int j = 0; j < net_stru[i]; j++)
            {
                    ann_nodes.Add(new ANN_Node(j,net_stru[i-1]));

                dou.Add(0);
            }
            node_arr.Add(ann_nodes);
            y_arr.Add(dou);
        }
    }
    int time = 0,during_step;
    bool start = false;
    public car_controller _car;
    // Update is called once per frame
    void Update()
    {
        if ( start)
        {
            time++;
            if (time > 1)
            {
                time = 0;
                if (!_car.fail && !_car.success && during_step < 100)
                {
                    double action = cac_output(_car.dis_p);
                    _car.change_pos(action);
                }
                else
                {
                    //cac reward
                }
            }
        }

    }
    public void simulate(){
        // init val 
        /*
         init car ,start , step
         */
        //if get reward ¦^¶Ç
        
    }
    double forward()
    {
        for(int i = 0; i < net_stru[0]; i++)
        {
            y_arr[0][i] = input[i];
        }
        for(int i = 1,len = net_stru.Length; i < len; i++){
            for(int j = 0; j < net_stru[i]; j++){
                y_arr[i][j] = node_arr[i][j].cac_output( y_arr[i-1]);
            }
        }
        return y_arr[net_stru.Length-1][0];
    }

    void print_node(int i,int j){
        Debug.Log("node " + i + " , " + j);
        ANN_Node tmp = node_arr[i][j];
        Debug.Log("id = " + tmp.id);
        Debug.Log("y = " + tmp.y);
        for(int t = 0; t < tmp.w.Count; t++)
        {
            Debug.Log( "w (" + i + "," + j + ") (" + (i-1) + "," + t + " ) =" + tmp.w[t]);
        }
    }
    public double cac_output(float[] _input){
        for(int i = 0; i < _input.Length; i++){
            input[i] = _input[i];
        }
        double ret = forward();
        ret *= 80;
        ret -= 40;
        return ret; 
    }



    public void change_w(Pt part){
        for (int i = 1, len_net = Global.net_stru.Length; i < len_net; i++)
            for (int j = 0; j < Global.net_stru[i]; j++)
            {
                node_arr[i][j].change_w(part.val[i][j]);
            }
    }

   
}
