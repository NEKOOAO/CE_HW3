using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSO : MonoBehaviour
{
    public static Pt global_best,all_best;
    public static double maxv=2.5;
    bool done = false;
    const int all_bird = 60;
    partial[] birds = new partial[all_bird];
    float[] rewards = new float[all_bird];
    public ANN ann;
    public TMPro.TextMeshProUGUI times;

    bool train = false;
    // Update is called once per frame
    private void Start()
    {
        all_best = new Pt(new partial().now.val);
    }
    void Update()
    {
        if (done&&train)
        {
            next_step();
        }
    }
    int @try, bird_num,maxtime = 4;
    void next_step()
    {
        if (@try < maxtime)
        {
            if (bird_num < all_bird - 1)
            {
                //Debug.Log("time : " + time + ", bird : " + bird_num);
                bird_num++;
                ann.change_w(birds[bird_num].now);
                ann.simulate();
                //birds[bird_num].print_node();
                done = false;
                times.text ="time : "+tim+" try : "+ (@try+1).ToString()+" bird : "+(bird_num+1).ToString();
            }
            else
            {
                for (int i = 0; i < all_bird; i++)
                {
                    birds[i].update_pos(rewards[i]);
                }
                @try++;
                bird_num = -1;
            }
        }
        else
        {
            train_finish();
        }

    }
    public void start_train()
    {
        bird_num = -1;
        @try = 0;
        train = true;
        global_best_reward = -10000;
        sucess = false;
        for (int i = 0; i < all_bird; i++)
        {
            birds[i] = new partial();
        }
        done = true;
    }
    int tim = 1;
    public bool sucess = false;
    int max_time = 2;
    void train_finish()
    {
        if (tim < max_time)
        {
            if (all_best_reward < global_best_reward)
            {
                all_best = new Pt(global_best.val);
                all_best_reward = global_best_reward;
            }
            tim++;
            start_train();

        }
        else{
            train = false;
            times.text = "";
            tim = 1;
        }
        
    }
    public void sim()
    {
        ann.change_w(all_best);
        ann.simulate();
        done = false;
    }
    float global_best_reward = -10000,all_best_reward = -10000;

    public void set_reward(float reward)
    {
        if (!train) return;
        rewards[bird_num] = reward;
        if (reward > global_best_reward)
        {
            global_best =new Pt( birds[bird_num].now.val);
            global_best_reward = reward;
        }

        
        done = true;
    }
}
