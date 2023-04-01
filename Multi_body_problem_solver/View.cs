using System.Collections;
using UnityEngine;
public class View : MonoBehaviour
{
    public float tmpX = 0f;
    public float tmpY = 0f;
    bool last = false;
    bool open = false;
    public int key = 0;
    public ArrayList history = new ArrayList();
    public string writting;
    void Update()
    {
        if (GameObject.Find("CommandInput").GetComponent<UnityEngine.UI.InputField>().isFocused) {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (key > 0) {
                    if (key == history.Count) {
                        writting = GameObject.Find("CommandInput").GetComponent<UnityEngine.UI.InputField>().text;
                    }
                    GameObject.Find("CommandInput").GetComponent<UnityEngine.UI.InputField>().text = (string)history[--key];
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (key < history.Count) {
                    if (++key == history.Count){
                        GameObject.Find("CommandInput").GetComponent<UnityEngine.UI.InputField>().text = writting;
                    }
                    else {
                        GameObject.Find("CommandInput").GetComponent<UnityEngine.UI.InputField>().text = (string)history[key];
                    }
                }
            }
        }
        else{
            if (Input.GetMouseButton(1)|| Input.GetMouseButton(0))
            {
                tmpX = tmpX + Input.GetAxis("Mouse X");
                tmpY = tmpY + Input.GetAxis("Mouse Y");
            }
            this.transform.rotation = Quaternion.Euler(-tmpY * 4, tmpX * 4, 0);
            if (Input.GetKey("w")){
                transform.Translate(new Vector3(0, 1f, 0) * Time.deltaTime, Space.World);
                GameObject.Find("Cent").transform.transform.Translate(new Vector3(0, 1f, 0) * Time.deltaTime, Space.World);
            }
            if (Input.GetKey("s")){
                transform.Translate(new Vector3(0, -1f, 0) * Time.deltaTime, Space.World);
                GameObject.Find("Cent").transform.transform.Translate(new Vector3(0, -1f, 0) * Time.deltaTime, Space.World);
            }
            if (Input.GetKey("a")){
                transform.Translate(new Vector3(-1f, 0, 0) * Time.deltaTime, Space.World);
                GameObject.Find("Cent").transform.transform.Translate(new Vector3(-1f, 0, 0) * Time.deltaTime, Space.World);
            }
            if (Input.GetKey("d")){
                transform.Translate(new Vector3(1f, 0, 0) * Time.deltaTime, Space.World);
                GameObject.Find("Cent").transform.transform.Translate(new Vector3(1f, 0, 0) * Time.deltaTime, Space.World);
            }
            if (Input.GetKey("q")){
                transform.Translate(new Vector3(0, 0, 1f) * Time.deltaTime, Space.World);
                GameObject.Find("Cent").transform.transform.Translate(new Vector3(0, 0, 1f) * Time.deltaTime, Space.World);
            }
            if (Input.GetKey("e")){
                transform.Translate(new Vector3(0, 0, -1f) * Time.deltaTime, Space.World);
                GameObject.Find("Cent").transform.Translate(new Vector3(0, 0, -1f) * Time.deltaTime, Space.World);
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0){
                if (Camera.main.fieldOfView <= 160)
                    Camera.main.fieldOfView += 2;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0){
                if (Camera.main.fieldOfView > 2)
                    Camera.main.fieldOfView -= 2;
            }
            if (GameObject.Find("Bodies").GetComponent<BodiesManager>().Simed){
                if (open){
                    open=false;
                }
            }
            if (Input.GetKey(KeyCode.F1)){
                last = true;
            }else{
                if ((GameObject.Find("Bodies").GetComponent<BodiesManager>().Simed == false) && last){
                    if (open){
                        open = false;
                    }
                    else{
                        open = true;
                    }
                }
                last = false;
            }
            if (open){
                GameObject.Find("Console").GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 300);
            }
            else {
                GameObject.Find("Console").GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 9999, 300);
            }
        }
    }
    public void OnCloseGame()
    {
        Application.Quit();
    }
    public void OCConsole() {
        open = open == false;
    }
}
