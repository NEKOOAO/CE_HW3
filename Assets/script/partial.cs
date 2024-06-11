using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using data = System.Collections.Generic.List<System.Collections.Generic.List<System.Collections.Generic.List<double>>>;

public struct Pt
{
    public data val;

    public Pt(data val){
        data stru;

        stru = new data();
        stru.Add(new List<List<double>>());
        for (int i = 1, len_net = Global.net_stru.Length; i < len_net; i++)
        {
            stru.Add(new List<List<double>>());
            for (int j = 0; j < Global.net_stru[i]; j++)
            {
                List<double> dou = new List<double>();
                for (int k = 0; k < Global.net_stru[i - 1] + 1; k++)
                {
                    dou.Add(val[i][j][k]);
                }
                stru[i].Add(dou);
            }
        }
        this.val = stru;
    }
    public static Pt operator +(Pt l, Pt r)
    {
        Pt ret = new Pt(l.val);
        for (int i = 1, len_net = Global.net_stru.Length; i < len_net; i++)
            for (int j = 0; j < Global.net_stru[i]; j++)
                for (int k = 0; k < Global.net_stru[i - 1] + 1; k++)
                    ret.val[i][j][k] += r.val[i][j][k];
        return ret;
    }
    public static Pt operator -(Pt l, Pt r)
    {
        Pt ret = new Pt(l.val);
        for (int i = 1, len_net = Global.net_stru.Length; i < len_net; i++)
            for (int j = 0; j < Global.net_stru[i]; j++)
                for (int k = 0; k < Global.net_stru[i - 1] + 1; k++)
                    ret.val[i][j][k] -= r.val[i][j][k];
        return ret;
    }
    public static Pt operator *(Pt l, double r)
    {
        Pt ret = new Pt(l.val);
        for (int i = 1, len_net = Global.net_stru.Length; i < len_net; i++)
            for (int j = 0; j < Global.net_stru[i]; j++)
                for (int k = 0; k < Global.net_stru[i - 1] + 1; k++)
                    ret.val[i][j][k] *= r;
        return ret;
    }
    public static Pt operator *( double r, Pt l)
    {
        Pt ret = new Pt(l.val);
        for (int i = 1, len_net = Global.net_stru.Length; i < len_net; i++)
            for (int j = 0; j < Global.net_stru[i]; j++)
                for (int k = 0; k < Global.net_stru[i - 1] + 1; k++)
                    ret.val[i][j][k] *= r;
        return ret;
    }
   

}
public class partial
{
    

    double w = 0.5, c1 = 0.4, c2 = 0.6;
    int[] net_stru;
    data stru;
    public Pt now,my_best,V;
    double best_val = -10000;
    public partial(){
        net_stru = Global.net_stru;
        stru = new data();
        stru.Add(new List<List<double>>());
        for (int i = 1, len_net = net_stru.Length; i < len_net; i++)
        {
            stru.Add(new List<List<double>>());
            for (int j = 0; j < net_stru[i]; j++)
            {
                List<double> dou = new List<double>();
                for (int k = 0; k < net_stru[i - 1] + 1; k++)
                {
                    dou.Add(0);
                }
                stru[i].Add(dou);
            }
        }
        now = new Pt(new data( stru));
        V = new Pt(new data(stru));
        for (int i = 1, len_net = net_stru.Length; i < len_net; i++)
        {
            for (int j = 0; j < net_stru[i]; j++)
            {
                for (int k = 0; k < net_stru[i - 1] + 1; k++)
                {
                    now.val[i][j][k] = Random.Range(-2.5f, 2.5f);
                }
            }
        }
        my_best = new Pt(new data(now.val));

    }
    public void update_pos(double now_reward){
        if (now_reward > best_val){
            my_best.val = new data(now.val);
            best_val = now_reward;
        }
        Pt new_v = w * V + c1 * (my_best - now)+ c2*(PSO.global_best-now);
        V = new  Pt( new_v.val);
        set_min_max(ref V, -PSO.maxv, PSO.maxv);
        now = now+ V;
    }
    void set_min_max(ref Pt x,double min,double max)
    {
        for (int i = 1, len_net = Global.net_stru.Length; i < len_net; i++)
            for (int j = 0; j < Global.net_stru[i]; j++)
                for (int k = 0; k < Global.net_stru[i - 1] + 1; k++)
                    x.val[i][j][k] =Mathf.Min( Mathf.Max((float)x.val[i][j][k], (float)min),(float)max);
    }
}
