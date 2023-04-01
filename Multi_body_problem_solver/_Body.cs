using UnityEngine;
using UnityEngine.UI;
public class _Body : MonoBehaviour
{
    public Body self;
    GameObject display;
    public int Number=-999;
    // Start is called before the first frame update
    public void FlushData(int num, Body data=null) {
        if (data != null) {
            self = data;
        }
        this.transform.SetParent(GameObject.Find("Bodies").GetComponent<Transform>(), false);
        this.transform.localPosition = new Vector3((float)self.postion[0], (float)self.postion[1], -(float)self.postion[2]);
        Number = num;
    }
    void OnDestroy()
    {
        Destroy(display);
    }
    public void CreateDisplayer() {
        display = Object.Instantiate(GameObject.Find("INFO_TEMPLATE"));
        display.transform.SetParent(GameObject.Find("UI2").GetComponent<Transform>(), false);
        UpdataDisplayer(Number);
    }
    public void UpdataDisplayer(int num) {
        display.GetComponent<Displayer>().Number = num;
        DataDisplay(self, new double[3] { 0, 0, 0 });
    }
    // Update is called once per frame
    public void DataDisplay(Body data, double[] AVec)
    {
        display.GetComponent<Displayer>().Flush(data.postion,data.vec,AVec,data.mass);
    }
    void Update()
    {
        if (Number != -999) {
            if (display != null){
                DataDisplay(self, Av);
            }
        }
    }
    double[] Av = new double[3] { 0, 0, 0 };
    public void Sim(Body[] Bodies) {
        double[] r;double mul=0;Av=new double[3] { 0,0,0};
        for (int i = 0; i < Bodies.Length; i++) {
            if (i != Number) {
                r = new double[3] { Bodies[i].postion[0] - Bodies[Number].postion[0], Bodies[i].postion[1] - Bodies[Number].postion[1], Bodies[i].postion[2] - Bodies[Number].postion[2] };
                mul = Bodies[i].mass / System.Math.Pow(r[0] * r[0] + r[1] * r[1] + r[2] * r[2], 3 / 2)*Body.G*System.Math.Pow(10,GameObject.Find("Slider").GetComponent<Slider>().value);
                Av[0] += mul * r[0]; Av[1] += mul * r[1]; Av[2] += mul * r[2];
            }
        }
        self = new Body(new double[7] { Bodies[Number].mass ,
            Bodies[Number].postion[0] + Bodies[Number].vec[0] * Time.deltaTime, Bodies[Number].postion[1] + Bodies[Number].vec[1] * Time.deltaTime , Bodies[Number].postion[2] + Bodies[Number].vec[2] * Time.deltaTime ,
            Bodies[Number].vec[0] + Av[0] * Time.deltaTime,Bodies[Number].vec[1]+Av[1]*Time.deltaTime,Bodies[Number].vec[2]+Av[2]*Time.deltaTime});
        this.transform.localPosition = new Vector3((float)self.postion[0], (float)self.postion[1], -(float)self.postion[2]);
    }
}
