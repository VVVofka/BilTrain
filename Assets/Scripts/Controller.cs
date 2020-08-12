using System.Collections.Generic;
using UnityEngine;

public enum GameMode {
    waitSetBalls,
    waitTakeAim,
    waitChoice,
    waitShowResult,
    waitExitShowResult
} // ******************************************************************************************
public class Controller : MonoBehaviour {
    private GameMode _mode;
    StudyProcess studyProcess = new StudyProcess();

    //[SerializeField] private GameObject luzeAimPoint;
    public GameObject ballAim;
    public GameObject ballCue;
    public GameObject ballVirt;

    public GameObject aimCenter;  // center
    public GameObject aimLeft;    // left
    public GameObject aimRight;   // right
    public GameObject bounds;   // far point of arc
    public Camera plcamera;   // 
    public GameObject luzeLeft;    // left
    public GameObject luzeRight;   // right
    public bool showVirtBall;   // right
    //[SerializeField] private bool showVirtBall;   // right

    float xmax, zmax;
    GameObject curTarg = null;

    GUIStyle style = new GUIStyle();
    bool inUpdate = false;

    void Start() {
        mode = GameMode.waitSetBalls;
        xmax = luzeRight.transform.position.x;
        zmax = luzeLeft.transform.position.z;
        if(!showVirtBall)
            ballVirt.transform.position = new Vector3(ballVirt.transform.position.x, -100f, ballVirt.transform.position.z);
        studyProcess.Create(ballAim, aimLeft, aimCenter, aimRight);

        style.fontSize = 24;
        //style.fontStyle = FontStyle.Bold;
        style.font = Font.CreateDynamicFontFromOSFont("Arial", style.fontSize);
        style.normal.textColor = new Color(1, 1, 0);
    } // ////////////////////////////////////////////////////////////////////////////////
    void Update() {
        //if(GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 25), "Выход")) 
        //    Application.Quit();
        if(studyProcess.initComplete == false)
            return;
        if(inUpdate)
            return;
        inUpdate = true;
        if(Input.GetKeyDown(KeyCode.I)) {
            studyProcess.info(sGameMode);
        } else {
            switch(mode) {
            case GameMode.waitSetBalls:
                if(ballAim != null) {
                    waitSetAimBall();
                    setCamera(ballAim);
                    mode = GameMode.waitTakeAim;
                }
                break;
            case GameMode.waitTakeAim:
                if(Input.GetKeyDown(KeyCode.Escape)) {
                    exitApp();
                } else if(Input.GetKeyDown(KeyCode.A)) {
                    if(studyProcess.targs.setSelect(-1)) {
                        setCamera(aimLeft);
                        mode = GameMode.waitChoice;
                    }
                } else if(Input.GetKeyDown(KeyCode.S)) {
                    if(studyProcess.targs.setSelect(0)) {
                        setCamera(aimCenter);
                        mode = GameMode.waitChoice;
                    }
                } else if(Input.GetKeyDown(KeyCode.D)) {
                        if(studyProcess.targs.setSelect(1)) {
                            setCamera(aimRight);
                            mode = GameMode.waitChoice;
                        }
                }
                break;
            case GameMode.waitChoice:
                if(Input.GetKeyDown(KeyCode.Space)) {
                    setCamera(ballAim);
                    mode = GameMode.waitTakeAim;
                } else if(Input.GetKeyDown(KeyCode.Escape)) {
                    exitApp();
                } else if(Input.GetKeyDown(KeyCode.A)) {
                    if(studyProcess.targs.setChoise(-1))
                        mode = GameMode.waitShowResult;
                    else if(studyProcess.targs.changePos)
                        setCamera(aimLeft);
                } else if(Input.GetKeyDown(KeyCode.S)) {
                    if(studyProcess.targs.setChoise(0))
                        mode = GameMode.waitShowResult;
                    else if(studyProcess.targs.changePos)
                        setCamera(aimCenter);
                } else if(Input.GetKeyDown(KeyCode.D)) {
                    if(studyProcess.targs.setChoise(1))
                        mode = GameMode.waitShowResult;
                    else if(studyProcess.targs.changePos)
                        setCamera(aimRight);
                }
                break;
            case GameMode.waitShowResult:
                studyProcess.saveRes();
                mode = GameMode.waitExitShowResult;
                break;
            case GameMode.waitExitShowResult:
                if(Input.GetKeyDown(KeyCode.Space) ||
                    Input.GetKeyDown(KeyCode.A) ||
                    Input.GetKeyDown(KeyCode.S) ||
                    Input.GetKeyDown(KeyCode.D)
                    ) {
                    mode = GameMode.waitSetBalls;
                } else if(Input.GetKeyDown(KeyCode.Escape)) {
                    exitApp();
                }
                break;
            default:
                break;
            }
        }
        inUpdate = false;
    } // ///////////////////////////////////////////////////////////////////////////////////
    void OnGUI() {
        {
            //GUI.Button(new Rect(50, 50, 195, 150), "Raise Integer");
            int fontsize = 24;
            float posX = 2;
            float posY = fontsize * 0.5f;
            float posW = plcamera.pixelWidth/2;
            float posH = fontsize * 1.1f;
            Rect rct = new Rect(posX, posY, posW, posH);
            string s = studyProcess.targs.truepos.ToString();
            GUI.Label(rct, s, style);
        }
        {
            int fontsize = 20;
            float posXL = 2;
            float posY = plcamera.pixelHeight - fontsize * 1.2f - 2;
            float posW = plcamera.pixelWidth/2;
            float posH = fontsize;
            Rect rctL = new Rect(posXL, posY, posW, posH);
            //GUIStyle styleL = new GUIStyle();
            //styleL.fontSize = fontsize;
            //style.fontStyle = FontStyle.Bold;
            //styleL.font = Font.CreateDynamicFontFromOSFont("Arial", fontsize);
            //styleL.normal.textColor = new Color(1, 1, 0);
            style.alignment = TextAnchor.LowerLeft;
            Topics topics = studyProcess.topics;
            Topic topic = topics.curTopic;
            string sL = "Topic: '" + topic.name + "'";
            sL += " №" + topics.ntopic + "(" + topics.Count + ") ";
            sL += topic.info;
            sL += "; Repeat lesson:" + topic.cntInStudyCur + "[" + topic.cntInStudyMax + "]";
            GUI.Label(rctL, sL, style);

            float posXR = plcamera.pixelWidth / 2;
            Rect rctR = new Rect(posXR, posY, posW - 2, posH);
            //GUIStyle styleR = new GUIStyle(styleL);
            style.alignment = TextAnchor.LowerRight;
            string sR = studyProcess.lesson.cntStuded + " remain:" + studyProcess.lesson.cntUnStuded;
            GUI.Label(rctR, sR, style);
        }
    } // /////////////////////////////////////////////////////////////////////////////////////
    //                  MODES
    void waitSetAimBall() {
        Layout lay = studyProcess.layout;

        lay.paim.setObj(ref ballAim);
        lay.pvir.setObj(ref ballVirt);
        lay.pcue.setObj(ref ballCue);

        float dkcue = studyProcess.dkcue(); // also set curAim
        int trg = studyProcess.targs. Preset(lay.pTarg, dkcue);
        Debug.Log(trg + " aim:" + lay.paim.x + "*" + lay.paim.z + " virt:" + lay.pvir.x + "*" + lay.pvir.z + " cue:" + lay.pcue.x + "*" + lay.pcue.z + "  targ:" + lay.pTarg.x + "*" + lay.pTarg.z);
    } // ///////////////////// EHD MODES ///////////////////////////////////////////////////
    void setCamera(GameObject gobj) {
        curTarg = gobj;
        d2p p = new d2p(gobj);
        setCamera(p);
    } // //////////////////////////////////////////////////////////////////////////////////////
    void setCamera(d2p ptarget) {
        if(studyProcess.layout == null)
            Debug.Break();
        Layout lay = studyProcess.layout;
        d2p pcam =  getPCameraHoriz(ptarget, lay.pcue);
        float camdist = pcam.dist(lay.pcue);
        if(camdist < Field.distCueCam)
            pcam = d2p.setDist(lay.pcue, pcam, Field.distCueCam);

        d2p pbnd = new d2p(bounds); //  far point arc
        float wfar = pcam.dist(pbnd);
        float wnear = pcam.dist(lay.pcue);
        float h = plcamera.transform.position.y;

        float degcamnear = d2p.rad2deg(Mathf.Atan2(h, wnear));
        float degcamfar = d2p.rad2deg(Mathf.Atan2(h - bounds.transform.position.y, wfar));
        float degcamavg = (degcamnear + degcamfar) / 2;

        float aCueTarget = lay.pcue.rad(ptarget);
        float agCueCam = d2p.rad2deg(aCueTarget - Mathf.PI / 2);

        Quaternion rotation = Quaternion.Euler(degcamavg, agCueCam, 0);
        plcamera.transform.SetPositionAndRotation(
            new Vector3(pcam.x, plcamera.transform.position.y, pcam.z),
            rotation);

        float sectorHor = getHorSector(pcam, lay.pcue, lay.paim, lay.pluze, Mathf.PI - aCueTarget);
        float sectorVert = degcamnear - degcamfar;
        float sectorMax = Mathf.Max(sectorVert, sectorHor);
        plcamera.fieldOfView = 1.05f * sectorMax;
    } // //////////////////////////////////////////////////////////////////////////////////////
    d2p getPCameraHoriz(d2p ptarget, d2p pcue) {
        float alfa = -ptarget.rad(pcue);
        if(alfa == 0)
            return new d2p(-xmax, pcue.z);
        else if(alfa == -Mathf.PI / 2)
            return new d2p(pcue.x, -zmax);
        else if(alfa > 0 && alfa < Mathf.PI / 2) {
            // y = ax + b
            float a = (ptarget.z - pcue.z) / (ptarget.x - pcue.x);  // >0
            float b = pcue.z - a * pcue.x;
            float z = b - a * xmax;
            float x = (-zmax - b) / a;
            d2p camshort = new d2p(-xmax, z);
            d2p camlong = new d2p(x, -zmax);
            if(camshort.dist(pcue) <= camlong.dist(pcue))
                return camshort;
            return camlong;
        } else if(alfa < 0 && alfa > -Mathf.PI / 2) {
            // y = ax + b
            float a = (ptarget.z - pcue.z) / (ptarget.x - pcue.x);  // <0
            float b = pcue.z - a * pcue.x;
            float z = b - a * xmax;
            float x = (zmax - b) / a;
            d2p camshort = new d2p(-xmax, z);
            d2p camlong = new d2p(x, zmax);
            if(camshort.dist(pcue) <= camlong.dist(pcue))
                return camshort;
            return camlong;
        } else if(alfa > Mathf.PI / 2 && alfa < Mathf.PI) {
            // y = ax + b
            float a = (pcue.z - ptarget.z) / (pcue.x - ptarget.x);  // <0
            float b = pcue.z - a * pcue.x;
            float z = b + a * xmax;
            float x = (-zmax - b) / a;
            d2p camshort = new d2p(xmax, z);
            d2p camlong = new d2p(x, -zmax);
            if(camshort.dist(pcue) <= camlong.dist(pcue))
                return camshort;
            return camlong;
        } else
            Debug.Break();
        return null;
    } // //////////////////////////////////////////////////////////////////////////////////////////////////
    void getAngle(d2p cam, d2p pnt, float diam, out float min, out float max) {
        float dist = cam.dist(pnt);
        float alfa = Mathf.Atan2(diam / 2, dist);
        float beta = Mathf.PI - cam.rad(pnt);
        beta = d2p.normrad(beta);
        min = Mathf.Min(beta - alfa, beta + alfa);
        max = Mathf.Max(beta - alfa, beta + alfa);
    } // //////////////////////////////////////////////////////////////////////////////////
    float getHorSector(d2p cam, d2p cue, d2p aim, d2p luze, float agCueTarget) {
        float min, max, curmin, curmax;
        getAngle(cam, cue, Field.BallD, out min, out max);
        getAngle(cam, aim, Field.BallD, out curmin, out curmax);
        min = Mathf.Min(min, curmin);
        max = Mathf.Max(max, curmax);
        getAngle(cam, luze, 1.2f * Field.BallD, out curmin, out curmax);
        min = Mathf.Min(min, curmin);
        max = Mathf.Max(max, curmax);
        agCueTarget = d2p.normrad(agCueTarget);
        float maxd = d2p.rad2deg(Mathf.Max(agCueTarget - min, max - agCueTarget));
        return 2 * maxd;
    } // //////////////////////////////////////////////////////////////////////////////////
    void exitApp() {
        studyProcess.Close();
        Application.Quit();
    } // /////////////////////////////////////////////////////////////////////////////////
    bool setRes(int select) {
        bool bSeriesSucess = false;
        Targs targs = studyProcess.targs;
        if(targs.selectLast == select) {
            bSeriesSucess = targs.setChoise(select);
            if(bSeriesSucess)
                mode = GameMode.waitShowResult;
            return bSeriesSucess;
        } else {

        }
        Targ targ = targs.targ;
        setCamera(targ.gobject);
        return bSeriesSucess;
    } // ///////////////////////////////////////////////////////////////////////////////////
    public string sGameMode {
        get {
            if(mode == GameMode.waitSetBalls)
                return "waitSetBalls";
            if(mode == GameMode.waitTakeAim)
                return "waitTakeAim";
            if(mode == GameMode.waitChoice)
                return "waitChoice";
            if(mode == GameMode.waitShowResult)
                return "waitShowResult";
            if(mode == GameMode.waitExitShowResult)
                return "waitExitShowResult";
            return "";
        }
    } // ///////////////////////////////////////////////////////////////////////////////////
    public GameMode mode {
        get => _mode;
        set {
            _mode = value;
            //UnityEngine.Debug.Log(sGameMode);
        }
    } // /////////////////////////////////////////////////////////////////////////
} // ************************************************************************************
