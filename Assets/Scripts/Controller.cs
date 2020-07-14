using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode {
    waitSetAimBall,
    waitSetCueBall,
    waitChoice
} // ******************************************************************************************
public class Controller : MonoBehaviour {
    static public GameMode mode;
    //Luze luze;
    //[SerializeField] private GameObject luzeAimPoint;
    public GameObject ballAim;
    public GameObject ballCue;
    public GameObject ballVirt;
    BallAim ballaim = new BallAim();
    BallCue ballcue = new BallCue();

    [SerializeField] private GameObject aimCenter;  // center
    [SerializeField] private GameObject aimLeft;    // left
    [SerializeField] private GameObject aimRight;   // right
    [SerializeField] private GameObject bounds;   // far point of arc
    [SerializeField] private Camera plcamera;   // 
    [SerializeField] private GameObject luzeLeft;    // left
    [SerializeField] private GameObject luzeRight;   // right
    [SerializeField] private bool showVirtBall;   // right

    float xmax, zmax;
    void Start() {
        mode = GameMode.waitSetAimBall;
        xmax = luzeRight.transform.position.x;
        zmax = luzeLeft.transform.position.z;
        //luze = new Luze(luzeSelect);
        if(!showVirtBall)
            ballVirt.transform.position = new Vector3(ballVirt.transform.position.x, -100f, ballVirt.transform.position.z);
    } // ////////////////////////////////////////////////////////////////////////////////
    void Update() {
        switch(mode) {
        case GameMode.waitSetAimBall:
            if(ballAim != null) {
                int cnt = 0;
                while(cnt < 64 && waitSetAimBall())
                    cnt++;
            }
            break;
        case GameMode.waitChoice:
            if(Input.GetKeyDown(KeyCode.Space)) {
                mode = GameMode.waitSetAimBall;
            }
            break;
        default:
            break;
        }
    } // ///////////////////////////////////////////////////////////////////////////////////
    //                  MODES
    bool waitSetAimBall() {
        ballaim.setRnd();   // normal, ballAim.transform.localScale.x
        ballaim.setDeg(12.5f, 2.622728f);         // TODO ЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖ
        ballaim.dbg("Aim ");
        d2p pluze = Field.luzeCornerAim(ballaim.curRad);
        //float normal = (3f / 4f) * Mathf.PI;    // luzeAimPoint.transform.rotation.eulerAngles.y;
        d2p paim = d2p.rotate(pluze, (3f / 4f) * Mathf.PI + ballaim.curRad, ballaim.curDist);
        if(isOutRange(paim))
            return true;
        //paim.Set(13.42005f, 2.700449f);              // TODO ЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖ
        paim.setObj(ballAim);

        ballcue.setRnd(); // ballCue.transform.localScale.x 
        ballcue.setVal(0.625f, 4.151007f);         // TODO ЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖ
        ballcue.dbg("Cue ");

        // Virtual ball
        float virdist = pluze.dist(paim) + Field.BallD;
        d2p pvir = d2p.setDist(pluze, paim, virdist);
        if(isOutRange(pvir))
            return true;
        pvir.setObj(ballVirt);

        // Cue ball
        float alfa = Mathf.Asin(ballcue.curK * ballAim.transform.localScale.x / Field.BallD);
        float beta = Mathf.Asin((paim.z - pvir.z) / Field.BallD);
        float rad = alfa - beta - Mathf.PI;

        d2p pcue = pvir.CreateDp(rad, ballcue.curDist);
        if(isOutRange(pcue))
            return true;
        //pcue.Set(11.83899f, 7.477563f);                   // TODO ЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖ
        pcue.setObj(ballCue);

        // Marks
        float gama = alfa - beta;
        float dsel = Field.BallD * Mathf.Cos(alfa);
        d2p psel = pvir.CreateDp(gama, dsel);
        psel.setObj(aimCenter);

        d2p pselout = d2p.addDist(paim, psel, Field.BallD / 16);
        pselout.setObj(aimLeft);
        d2p pselin = d2p.addDist(paim, psel, -Field.BallD / 16);
        pselin.setObj(aimRight);

        // Camera
        d2p pcam =  getPCameraHoriz(pvir, pcue);
        //float aCueAim = pcue.rad(pvir);

        //d2p viewcentr = new d2p((pcue.x + pbnd.x)/2, (pcue.z + pbnd.z)/2);
        d2p pbnd = new d2p(bounds); //  far point arc
        float wfar = pcam.dist(pbnd);
        float wnear = pcam.dist(pcue);
        float h = plcamera.transform.position.y;
        float degcamnear = d2p.rad2deg(Mathf.Atan2(h, wnear));
        float degcamfar = d2p.rad2deg( Mathf.Atan2(h - bounds.transform.position.y, wfar));

        //d2p pluz = new d2p(luzeAimPoint);
        float sectorHor = getHorSector(pcam, pcue, paim, pluze, Field.BallD, Mathf.PI - aCueAim);
        Quaternion rotation = Quaternion.Euler((degcamfar+degcamnear)/2, agCueCam, 0);
        plcamera.transform.SetPositionAndRotation(
            new Vector3(pcam.x, plcamera.transform.position.y, pcam.z),
            rotation);

        float sectorVert = degcamnear - degcamfar;
        float sectorMax = Mathf.Max(sectorVert, sectorHor);
        plcamera.fieldOfView = 1.05f * sectorMax;

        paim.dbg("Aim ");
        pcue.dbg("Cue ");
        //p.dbg("aim:" + ballaim.curDeg.ToString() + " dist:" + ballaim.curDist.ToString() + "  " + " k:" + ballcue.curK);
        mode = GameMode.waitChoice;
        return false;
    } // ///////////////////// EHD MODES ///////////////////////////////////////////////////
    d2p getPCameraHoriz(d2p ptarget, d2p pcue) {
        //float agCueCam = d2p.rad2deg(aCueAim - Mathf.PI / 2);
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
        }else if(alfa > -Mathf.PI && alfa < -Mathf.PI / 2){ 
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
        } else {
            Debug.Break();
        }
        return null;
    } // //////////////////////////////////////////////////////////////////////////////////////////////////
    void getAngle(d2p cam, d2p pnt, float diam, out float min, out float max) {
        float dist = cam.dist(pnt);
        float alfa = Mathf.Atan2(Field.BallD / 2, dist);
        float beta = Mathf.PI - cam.rad(pnt);
        min = Mathf.Min(beta - alfa, beta + alfa);
        max = Mathf.Max(beta - alfa, beta + alfa);
    } // //////////////////////////////////////////////////////////////////////////////////
    float getHorSector(d2p cam, d2p cue, d2p aim, d2p luze, float diam, float agCueAim) {
        float min, max, curmin, curmax;
        getAngle(cam, cue, diam, out min, out max);
        getAngle(cam, aim, diam, out curmin, out curmax);
        min = Mathf.Min(min, curmin);
        max = Mathf.Max(max, curmax);
        getAngle(cam, luze, 2 * diam, out curmin, out curmax);
        min = Mathf.Min(min, curmin);
        max = Mathf.Max(max, curmax);
        float maxd = d2p.rad2deg( Mathf.Max(agCueAim - min, max - agCueAim));
        return 2 * maxd;
    } // //////////////////////////////////////////////////////////////////////////////////
    bool isOutRange(d2p p) {
        float xr = p.x + Field.BallR;
        float zr = p.z + Field.BallR;
        return !(xr <= xmax && zr <= zmax && xr >= -xmax && zr >= -zmax);
        //return !(xr < xmax && zr < zmax && xr > -xmax && zr > -zmax);
    } // /////////////////////////////////////////////////////////////////////////////////

} // ************************************************************************************
