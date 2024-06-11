using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map_controller : MonoBehaviour
{
    public input_controller input_c;
    double[][] input;
    public LineRenderer line_r;
    public GameObject end;
    float sin(float ang) { return Mathf.Sin(ang * Mathf.PI / 180); }
    float cos(float ang) { return Mathf.Cos(ang * Mathf.PI / 180); }
    List<line> line_arr = new List<line>();
    const double eps = 1e-8, NAN = -1e8, inf = 1e8;
    public Vector2 center;
    int dcmp(double x)
    {
        if (Mathf.Abs((float)x) < eps)
        {
            return 0;
        }
        else return x < 0 ? -1 : 1;
    }
    struct Pt
    {
        public double x, y;
        public Pt(double _x = 0, double _y = 0)
        {
            x = _x;
            y = _y;
        }
        public Pt(Vector2 point)
        {
            x = point.x;
            y = point.y;
        }
        public static Pt operator +(Pt a, Pt b)
            => new Pt(a.x + b.x, a.y + b.y);
        public static Pt operator -(Pt a, Pt b)
            => new Pt(a.x - b.x, a.y - b.y);
        public static Pt operator *(Pt a, double b)
            => new Pt(a.x * b, a.y * b);
        public static double operator *(Pt a, Pt b)
             => a.x * b.x + a.y * b.y;
        public static double operator ^(Pt a, Pt b)
            => a.x * b.y - a.y * b.x;
        public float val()
        {
            return Mathf.Sqrt((float)(x * x + y * y));
        }
        public void cout()
        {
            Debug.Log("( " + x + ", " + y + " )");
        }
    }
    struct line
    {
        public Pt s, e, v;
        public line(Pt _s, Pt _e)
        {
            s = _s;
            e = _e;
            v = _e - _s;
        }
    }
    struct Circle
    {
        public Pt o; public float r;
        public Circle(Pt _o, float _r)
        {
            o = _o; r = _r;
        }
    }
    bool is_inter_c_l_new(line l, Circle cir)
    {
        Pt p1 = l.s, p2 = l.e, dp = p2 - p1;
        Pt poi_vec = cir.o - p1;
        double c = l.v * poi_vec;
        if (c <= 0) return poi_vec.val() < cir.r;
        double d = l.v.val();
        d *= d;
        if (c >= d) return (cir.o - p2).val() < cir.r;
        double r = c / d;
        Pt xx = new Pt(p1.x + (p2.x - p1.x) * r, p1.y + (p2.y - p1.y) * r);

        return (cir.o - xx).val() < cir.r;
    }
    Pt LLIntersect(line a, line b)
    {
        Pt p1 = a.s, p2 = a.e, q1 = b.s, q2 = b.e;
        double f1 = (p2 - p1) ^ (q1 - p1), f2 = (p2 - p1) ^ (p1 - q2), f;
        if (dcmp(f = f1 + f2) == 0)
        {
            return dcmp(f1) == 0 ? new Pt(NAN, NAN) : new Pt(inf, inf);
        }
        return q1 * (f2 / f) + q2 * (f1 / f);
    }
    int ori(Pt o, Pt a, Pt b)
    {
        double ret = (a - o) ^ (b - o);
        return ((ret > 0) ? 1 : 0) - ((ret < 0) ? 1 : 0);
    }
    bool is_inter_line(line a, line b)
    {
        Pt p1 = a.s, p2 = a.e, q1 = b.s, q2 = b.e;
        if (((p2 - p1) ^ (q2 - q1)) == 0)
        {
            if (ori(p1, p2, q1) != 0) return false;
            return ((p1 - q1) * (p2 - q1)) <= 0 ||
                   ((p1 - q2) * (p2 - q2)) <= 0 ||
                   ((q1 - p1) * (q2 - p1)) <= 0 ||
                   ((q1 - p2) * (q2 - p2) <= 0);
        }
        return (ori(p1, p2, q1) * ori(p1, p2, q2) <= 0) &&
              (ori(q1, q2, p1) * ori(q1, q2, p2) <= 0);
    }
    // Start is called before the first frame update
    void Start()
    {

        input = input_c.get_line();

        for (int i = 1; i < input.Length; i++)
        {
            line_arr.Add(new line(new Pt(input[i][0], input[i][1]), new Pt(input[i - 1][0], input[i - 1][1])));
        }
        line_init();
        Vector2[] endpos = input_c.getend();
        center = (endpos[0] + endpos[1]) / 2;
        Vector2 lr = (endpos[0] - endpos[1]);
        lr.y = Mathf.Abs(lr.y); lr.x = Mathf.Abs(lr.x);
        end.transform.position = center;
        end.transform.localScale = lr;
    }

    // Update is called once per frame
    void Update()
    {


    }
    public float dis(Vector2 point, float ang)
    {
        Pt s = new Pt(point);
        Vector2 v = new Vector2(cos(ang) * 10000, sin(ang) * 10000);
        Pt e = new Pt(s.x + v.x, s.y + v.y);
        line lin = new line(s, e);
        float ret = 1e5f;
        for (int i = 0; i < line_arr.Count; i++)
        {
            if (is_inter_line(line_arr[i], lin))
            {
                Pt inter = LLIntersect(line_arr[i], lin), dis_v = inter - s;
                ret = Mathf.Min(ret, dis_v.val());
            }
        }
        return ret;
    }
    public bool safe_point(Vector2 point, float r)
    {
        Pt now = new Pt(point);
        Circle cir = new Circle(now, r);
        for (int i = 0; i < line_arr.Count; i++)
        {
            if (is_inter_c_l_new(line_arr[i], cir))
            {
                return false;
            }
        }
        return true;
    }

    void line_init()
    {
        line_r.startWidth = 0.2f;
        line_r.endWidth = 0.2f;
        line_r.positionCount = input.Length;
        for (int i = 0; i < input.Length; i++)
        {
            line_r.SetPosition(i, new Vector2((float)input[i][0], (float)input[i][1]));
        }
    }

}
