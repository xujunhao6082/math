using UnityEngine;
using UnityEngine.UI;
public class Displayer : MonoBehaviour
{
    public int Number;
    public void Flush(double[] P,double[] V,double[] AV,double Mass) {
        this.transform.Find("Pos").GetComponent<Text>().text = "Pos:" + System.Math.Floor(P[0]*100)/100 + "," + System.Math.Floor(P[1] * 100) / 100 + "," + System.Math.Floor(P[2] * 100) / 100;
        this.transform.Find("Vec").GetComponent<Text>().text = "Vec:" + System.Math.Floor(V[0] * 100) / 100 + "," + System.Math.Floor(V[1] * 100) / 100 + "," + System.Math.Floor(V[2] * 100) / 100;
        this.transform.Find("AVec").GetComponent<Text>().text = "AVec:" + System.Math.Floor(AV[0] * 100) / 100 + "," + System.Math.Floor(AV[1] * 100) / 100 + "," + System.Math.Floor(AV[2] * 100) / 100;
        this.transform.Find("No.").GetComponent<Text>().text = "No.:" + (Number+1)+" Mass:"+Mass;
        this.transform.localPosition = new Vector3(-50.5f,  -( Number-GameObject.Find("Bodies").GetComponent<BodiesManager>().phase) * 45f-10);
    }
}
