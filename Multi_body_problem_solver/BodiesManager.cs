using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
public class Body
{
    public const double G = 6.6740831*0.00000000001;
    public double[] postion;
    public double[] vec;
    public double mass;
    public Body(double[] datas)
    {
        mass = datas[0];
        postion = new double[3] { datas[1], datas[2], datas[3] };
        if (datas.Length >= 7) { vec = new double[3] { datas[4], datas[5], datas[6] }; } else { vec = new double[3] { 0, 0, 0 }; }
    }
}
public class BodiesManager : MonoBehaviour
{
    static string[] Commands = new string[5]{"help","addbody","delbodies","setbody","clear"};
    static string[] SetTypes = new string[3] { "mass", "pos", "vec" };
    const string help1 = "Unit:mass:kg*10^(Mass magnification factor);\nx(R),y(G),z(B) :m;vx,vy,vz:m/s\nPress W,A,S,D to move on xy\nPress Q,E to mov on z-axis\nPress Up,Down to use history commands when you enter command\nPS:I can\'t do anything if you notice high GPU usage,I have reduced the Image Quality to the lowest. (Fuking Unity)\n";
    const string help2 = "Help of Command:<>->Arguments;[]->optional\n help:get help\n addbody:add body\n  Grammer:addbody <mass x y z[ vx vy vz]> \n delbodies:delete bodies\n  Grammar:delbodies <body1num[ body2num body3num ...]>\n setbody:set body's data\n  Grammar:setbody <settype bodynum datas...>\n   settype:mass,pos,vec\n clear:clear console screen";
    public Hashtable BodiesTable = new Hashtable();
    public Hashtable Bodies = new Hashtable();
    public bool Simming = false;
    public bool Simed = false;
    void Start()
    {
        BodiesTable.Clear();
        Bodies.Clear();
        Res();
        GameObject.Find("Console").GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 9999, 300);
    }
    void AddBody(double[] DATA) {
        BodiesTable.Add(BodiesTable.Count, new Body(DATA));
        Bodies.Add(Bodies.Count,Object.Instantiate(GameObject.Find("BODY_TEMPLATE")));
        ((GameObject)Bodies[Bodies.Count - 1]).GetComponent<_Body>().FlushData( Bodies.Count - 1, (Body)BodiesTable[BodiesTable.Count - 1]);
        ((GameObject)Bodies[Bodies.Count - 1]).GetComponent<_Body>().CreateDisplayer();
        return;
    }
    bool DelBody (int key){
        if (BodiesTable.ContainsKey(key-1)){
            BodiesTable.Remove(key-1);
            Destroy((GameObject)Bodies[key-1]);
            Bodies.Remove(key-1);
            for (int i = key; i < Bodies.Count+1; i++) {
                ((GameObject)Bodies[i]).GetComponent<_Body>().FlushData(i - 1);
                ((GameObject)Bodies[i]).GetComponent<_Body>().UpdataDisplayer(i - 1);
                Bodies.Add(i-1, Bodies[i]);
                Bodies.Remove(i);
                BodiesTable.Add(i - 1, BodiesTable[i]);
                BodiesTable.Remove(i);
            }
            return true;
        }
        else{
            return false;
        }
    }
    public void Res()
    {
        Simming = false;
        Simed = false;
        for (int i = 0; i < Bodies.Count; i++){
            ((GameObject)Bodies[i]).GetComponent<_Body>().FlushData(i, (Body)BodiesTable[i]);
        }
    }
    public float phase;
    void Update()
    {
        if (Bodies.Count >= 5) {
            GameObject.Find("Scrollbar").GetComponent<Scrollbar>().size = 5f / Bodies.Count;
            phase = (Bodies.Count - 5) * GameObject.Find("Scrollbar").GetComponent<Scrollbar>().value;
        } else {
            GameObject.Find("Scrollbar").GetComponent<Scrollbar>().size = 1;
            phase = 0;
        }
        if (Simming) {
            GameObject.Find("SST").GetComponent<Text>().text = "STOP";
            Body[] Bodies_array = new Body[Bodies.Count];
            for (int i = 0; i < Bodies.Count; i++){
                Bodies_array[i] = ((GameObject)Bodies[i]).GetComponent<_Body>().self;
            }
            for (int i = 0; i < Bodies.Count; i++){
                ((GameObject)Bodies[i]).GetComponent<_Body>().Sim(Bodies_array);
            }
        }
        else{
            GameObject.Find("SST").GetComponent<Text>().text = "START";
        }
        if (Simed == false) {
            for (int i = 0; i < Bodies.Count; i++){
                ((GameObject)Bodies[i]).GetComponent<TrailRenderer>().Clear();
            }
        }
        GameObject.Find("RESET").GetComponent<Button>().interactable = Simed;
        GameObject.Find("Slider").GetComponent<Slider>().interactable = Simed == false;
    }
    public void COutScroll() {
        if (10 / GameObject.Find("OutputSBar").GetComponent<Scrollbar>().size > 10){
            GameObject.Find("Output").transform.localPosition = new Vector3(1, -160 * (1 / GameObject.Find("OutputSBar").GetComponent<Scrollbar>().size - 1) * (GameObject.Find("OutputSBar").GetComponent<Scrollbar>().value - 1));
        }
        else{
            GameObject.Find("Output").transform.localPosition = new Vector3(1, 0);
        }
    }
    public void simSS() {
        Simming = Simming == false;
        Simed = true;
    }
    public void SyncNum() {
        GameObject.Find("N").GetComponent<Text>().text = ((int)GameObject.Find("Slider").GetComponent<Slider>().value).ToString();
    }
    public void ConsoleInput() {
        string INPUTDATA = GameObject.Find("CommandInput").GetComponent<InputField>().text;
        GameObject.Find("CommandInput").GetComponent<InputField>().text = "";
        MatchCollection COMMAND = Regex.Matches(INPUTDATA, @"\s{0}[^\s]+\s{0}");
        string[] DATA = new string[COMMAND.Count];
        int i = 0;
        foreach (Match t in COMMAND){
            DATA[i] = t.Value;
            i++;
        }
        switch (System.Array.IndexOf(Commands, DATA[0])) {
            case 0:
                PrintOnOutput(help1+help2);
                break;
            case 1:
                if ((DATA.Length < 5) || ((DATA.Length > 5) && (DATA.Length < 8))||(DATA.Length>8)) {
                    PrintOnOutput("Format Error");
                    break;
                }
                double[] bodydatas = new double[DATA.Length - 1];
                for (i = 1; i < DATA.Length; i++){
                    if (double.TryParse(DATA[i], out double bodydata) == false){
                        PrintOnOutput("The Arg " + i + " Not a Number");
                        break;
                    }
                    else{
                        bodydatas[i - 1] = bodydata;
                    }
                }
                AddBody(bodydatas);
                PrintOnOutput("Add Body Success");
                break;
            case 2:
                int[] keys = new int[DATA.Length - 1];
                for (i = 1; i < DATA.Length; i++) {
                    if (int.TryParse(DATA[i], out int key) == false){
                        PrintOnOutput("The Arg "+i+" Not a Number");
                        break;
                    }
                    else {
                        keys[i-1] = key;
                    }
                }
                System.Array.Sort(keys);
                System.Array.Reverse(keys);
                int failed=0;
                foreach (int key in keys) {
                    if (DelBody(key)==false) {
                        failed+=1;
                    }
                }
                if (failed == 0){
                    PrintOnOutput("Delete Bodies Success," + (DATA.Length - 1) + (((DATA.Length - 1) > 1) ? " bodies" : " body") + " deleted");
                }
                else {
                    PrintOnOutput("Delete Bodies Failed," + (DATA.Length - 1 - failed) + (((DATA.Length - 1 - failed) > 1) ? " bodies" : " body") + " deleted");
                }
                break;
            case 3:
                if (System.Array.IndexOf(SetTypes, DATA[1])==-1) {
                    PrintOnOutput("SetType Not Exist");
                    break;
                }
                if (DATA.Length != 4&& DATA[1] == SetTypes[0]){
                    PrintOnOutput("Format Error");
                    break;
                }
                if (int.TryParse(DATA[2], out int skey)==false) {
                    PrintOnOutput("The Arg 2 Not a Number");
                    break;
                }
                if (BodiesTable.ContainsKey(skey - 1)==false) {
                    PrintOnOutput("The Body Not Exist");
                    break;
                }
                Body data = (Body)BodiesTable[skey - 1];
                if (double.TryParse(DATA[3], out double x) == false){
                    PrintOnOutput("The Arg 3 Not a Number");
                    break;
                }
                if (DATA[1] == SetTypes[0]){
                    data.mass = x;
                    PrintOnOutput("Set Mass Success");
                }
                else {
                    if (DATA.Length != 6){
                        PrintOnOutput("Format Error");
                        break;
                    }
                    if (double.TryParse(DATA[4], out double y) == false){
                        PrintOnOutput("The Arg 4 Not a Number");
                        break;
                    }
                    if (double.TryParse(DATA[5], out double z) == false){
                        PrintOnOutput("The Arg 5 Not a Number");
                        break;
                    }
                    if (DATA[1] == SetTypes[1]){
                        data.postion = new double[3] { x, y, z };
                        PrintOnOutput("Set Pos Success");
                    }
                    if (DATA[1] == SetTypes[2]){
                        data.vec = new double[3] { x, y, z };
                        PrintOnOutput("Set Vec Success");
                    }
                }
                ((GameObject)Bodies[skey - 1]).GetComponent<_Body>().FlushData(skey - 1, data);
                BodiesTable[skey - 1] = data;
                break;
            case 4:
                GameObject.Find("Output").GetComponent<Text>().text = "";
                setTextScale();
                break;
            default:
                PrintOnOutput("Unknown Command");
                break;
        }
        GameObject.Find("Camera").GetComponent<View>().key += 1;
        GameObject.Find("Camera").GetComponent<View>().history.Add(INPUTDATA);
    }
    void PrintOnOutput(string text) {
        GameObject.Find("Output").GetComponent<Text>().text = GameObject.Find("Output").GetComponent<Text>().text + text + "\n";
        setTextScale();
        COutScroll();
    }
    void setTextScale() {
        MatchCollection SenS = Regex.Matches(GameObject.Find("Output").GetComponent<Text>().text, @"\n{0}[^\n]+\n{0}");
        int[] hep = new int[SenS.Count];
        int i = 0;
        int L;
        Font font=Font.CreateDynamicFontFromOSFont("Arial",14);
        foreach (Match sen in SenS) {
            hep[i] = 1;
            L = 0;
            font.RequestCharactersInTexture(sen.Value);
            foreach (char c in sen.Value){
                font.GetCharacterInfo(c,out CharacterInfo info);
                L += info.advance;
                if (L >= 300) {
                    hep[i] += 1;
                    L = info.advance;
                }
            }
            i++;
        }
        int he = 0;
        foreach (int p in hep) {
            he += p;
        }
        if (he > 10){
            GameObject.Find("Output").GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 16 * he);
            GameObject.Find("OutputSBar").GetComponent<Scrollbar>().size = 10f / he;
        }
        else{
            GameObject.Find("Output").GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 160);
            GameObject.Find("OutputSBar").GetComponent<Scrollbar>().size = 1;
        }
    }
}