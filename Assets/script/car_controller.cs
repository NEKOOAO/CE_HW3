using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class car_controller : MonoBehaviour
{
    public map_controller map;
    public GameObject trace_obj;
    public input_controller input_c;
    public TextMeshProUGUI[] dis_out;

    public bool cac_finish = true, success = false;
    Car car;
    int tim = 0;
    public bool car_start = false, car_reset = false, tmp = true, fail = false;
    Vector3 vec;
    public float[] dis_p;
    List<float> ang_his = new List<float>();
    List<GameObject> trace_arr = new List<GameObject>();
    [SerializeField] Vector2 target_point;
    [SerializeField] float alert_dis = 3.5f;
    struct Car
    {
        float x, y, car_ang, b;
        public float[] dis, dang;
        map_controller map;
        public Car(Vector3 vec, ref map_controller _map)
        {
            this.map = _map;
            x = vec.x; y = vec.y;
            car_ang = vec.z;

            b = 6;
            dis = new float[3] { 0, 0, 0 };
            dang = new float[3] { 0, -45, 45 };
            update_dis();
        }
        public void newpos(float wheel_ang)
        {
            float new_x, new_y, new_car_ang;
            new_x = x + cos(car_ang + wheel_ang) + sin(car_ang) * sin(wheel_ang);
            new_y = y + sin(car_ang + wheel_ang) - cos(car_ang) * sin(wheel_ang);
            new_car_ang = car_ang - asin(2 * sin(wheel_ang) / (b));
            if (new_car_ang > 270) new_car_ang -= 360;
            else if (new_car_ang < -90) new_car_ang += 360;
            x = new_x; y = new_y; car_ang = new_car_ang;
            update_dis();
            //Debug.Log(x+" "+y+" "+car_ang);
        }
        void update_dis()
        {
            for (int i = 0; i < 3; i++)
            {
                dis[i] = map.dis(now_pos(), car_ang + dang[i]);
            }
        }
        public bool is_safe()
        {
            return map.safe_point(now_pos(), (b) / 2);
        }
        public Vector2 now_pos()
        {
            return new Vector2(x, y);
        }
        public void test_pos(Vector2 pos)
        {
            x = pos.x;
            y = pos.y;
        }
        float sin(float ang) { return Mathf.Sin(ang * Mathf.PI / 180); }
        float cos(float ang) { return Mathf.Cos(ang * Mathf.PI / 180); }
        float asin(float val) { return Mathf.Asin(val) / Mathf.PI * 180; }
        public float get_ang() { return car_ang; }
    }
    // Start is called before the first frame update
    void Start()
    {
        vec = input_c.get_initpos();
        car = new Car(vec, ref map);

    }

    // Update is called once per frame
    void Update()
    {
    }
    Vector2 oldpos, newpos;
    public void change_pos(double wheel_deg)
    {
        oldpos = car.now_pos();
        cac_finish = false;
        car.newpos((float)wheel_deg);
        ang_his.Add((float)wheel_deg);
        newpos = car.now_pos();
        transform.position = car.now_pos();
        transform.rotation = Quaternion.Euler(0, 0, car.get_ang() - 90);
        trace_arr.Add(Instantiate(trace_obj, car.now_pos(), new Quaternion()));
        dis_p = car.dis;
        dis_out[0].text = "" + car.dis[2];
        dis_out[1].text = "" + car.dis[0];
        dis_out[2].text = "" + car.dis[1];
        //Debug.Log(car.dis[0]+", "+ car.dis[1] + ", "+ car.dis[2]);
        if (car.is_safe() == false)
        {
            fail = true;
            // Debug.Log("err" + car.now_pos());
            car_start = false;
            tmp = true;
        }
        cac_finish = true;
    }

    public float cac_reward()
    {
        Vector2 add(Vector2 l, Vector2 r)
        {
            return new Vector2(l.x + r.x, l.y + r.y);
        }
        Vector2 sub(Vector2 l, Vector2 r)
        {
            return new Vector2(l.x - r.x, l.y - r.y);
        }
        float olddis, newdis, reward = 0;
        olddis = sub(target_point, oldpos).magnitude;
        newdis = sub(target_point, newpos).magnitude;

        foreach (var i in dis_p)
        {
            if (i < alert_dis)
            {
                reward -= 1f;
            }
        }
        if (success) reward += 10;
        if (fail) reward -= 20;
        return reward;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //save();
        success = true;
        // Debug.Log("hit");
        car_start = false;
        tmp = true;
    }

    public void init()
    {
        for (int i = 0; i < trace_arr.Count; i++)
        {
            Destroy(trace_arr[i]);
        }
        trace_arr.Clear();
        car = new Car(vec, ref map);
        success = false;
        fail = false;
        target_point = map.center;
    }


    public void start_car()
    {
        car_start = true;
    }
    public int train_ret()
    {
        if (car_start == false)
        {
            if (success) return 1;
            return 0;
        }
        return -1;
    }
    public void resucess()
    {
        success = false;
    }

    public float[] get_senser()
    {
        return dis_p;
    }
}
