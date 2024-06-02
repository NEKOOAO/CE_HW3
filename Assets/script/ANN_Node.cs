using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANN_Node {
    public List<double> w,yi,nw;
    public double theata = -1,y,delta;
    public int id,len;
    public bool test_err = false;
    public double err_val;


    public ANN_Node( int _id,int _len){
        id = _id;
        w = new List<double>();
        yi = new List<double>();
        nw = new List<double>();
        len = _len;
        for (int i = 0; i < len+1; i++)
        {
            nw.Add(Random.Range(-10f, 10f));
            yi.Add(0);
            w.Add(0);
        }
    }
    public void reset()
    {
        for (int i = 0; i < len + 1; i++)
        {
            nw[i]=(Random.Range(-0.5f, 0.5f));
        }
    }


    public double cac_output( List<double> y_in){
        double v = 0;
        w[0] = nw[0];
        v += w[0] * -1;
        for(int i = 1,len = w.Count; i < len; i++){
            w[i] = nw[i];
            v += w[i] * y_in[i-1];
            yi[i] = y_in[i-1];
        }
        y = sigmoid(ref v);
        return y;
    }
    private double sigmoid (ref double v){
        double k = Mathf.Exp((float)v);
        double t = k / (1 + k);
        
        if (t != t)
        {
            return 1;
        }
        
        return t;
    }
    public void change_w(List<double> w_list){
        for (int i = 0; i < len + 1; i++)
        {
            nw[i] = w_list[i];
        }
    }
}
