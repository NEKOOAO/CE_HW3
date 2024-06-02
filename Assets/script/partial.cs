using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using  data = System.Collections.Generic.List<System.Collections.Generic.List<System.Collections.Generic.List<double>>>;
public struct Pt
{
    public data val;
    public Pt(data val){
        this.val = val;
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
    double w = 0.6, c1 = 0.7, c2 = 0.3;
    int[] net_stru;
    data stru;
    Pt now,my_best,V;
    double best_val = 0;
    // Start is called before the first frame update
    public partial(){
        net_stru = Global.net_stru;
        stru = new data();
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
        my_best = new Pt(new data(stru));
        V = new Pt(new data(stru));
    }
    public void update_pos(double now_reward){
        if (now_reward > best_val){
            my_best.val = new data(now.val);
        }
        //v(t+1) = w*v(t)+c1*(mybest - now)*Random(0,1) + c2*(gbest - now)*Random(0,1)
        Pt new_v = w * V + c1 * (my_best - now) * Random.Range(0, 1) + c2*(PSO.global_best-now)*Random.Range(0,1);
        V = new_v;
        set_min_max(ref V, -PSO.maxv, PSO.maxv);
        now += V;
    }
    void set_min_max(ref Pt x,double min,double max)
    {
        for (int i = 1, len_net = Global.net_stru.Length; i < len_net; i++)
            for (int j = 0; j < Global.net_stru[i]; j++)
                for (int k = 0; k < Global.net_stru[i - 1] + 1; k++)
                    x.val[i][j][k] =Mathf.Min( Mathf.Max((float)x.val[i][j][k], (float)min),(float)max);
    }

}
