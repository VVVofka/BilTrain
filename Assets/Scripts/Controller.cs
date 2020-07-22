﻿using System.Collections;
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
    BallAim ballaim;
    BallCue ballcue;
    StudyProcess studyProcess;

    //Luze luze;
    //[SerializeField] private GameObject luzeAimPoint;
    public GameObject ballAim;
    public GameObject ballCue;
    public GameObject ballVirt;

    [SerializeField] private GameObject aimCenter;  // center
    [SerializeField] private GameObject aimLeft;    // left
    [SerializeField] private GameObject aimRight;   // right
    [SerializeField] private GameObject bounds;   // far point of arc
    [SerializeField] private Camera plcamera;   // 
    [SerializeField] private GameObject luzeLeft;    // left
    [SerializeField] private GameObject luzeRight;   // right
    [SerializeField] private bool showVirtBall;   // right

    float xmax, zmax;
    TrueAim selectAim = TrueAim.none;

    bool inupdate = false;

    public Controller() {
    } // ////////////////////////////////////////////////////////////////////////////////

    void Start() {
        inupdate = true;
        ballaim = new BallAim();
        ballcue = new BallCue();
        studyProcess = new StudyProcess();
        studyProcess.on_aim += OnReactOnChoose;

        mode = GameMode.waitSetAimBall;
        xmax = luzeRight.transform.position.x;
        zmax = luzeLeft.transform.position.z;
        //luze = new Luze(luzeSelect);
        if(!showVirtBall)
            ballVirt.transform.position = new Vector3(ballVirt.transform.position.x, -100f, ballVirt.transform.position.z);
        inupdate = false;
    } // ////////////////////////////////////////////////////////////////////////////////
    void OnReactOnChoose(TrueAim trueAim) {
        if(selectAim == trueAim) {

        } else {

        }
    } // ////////////////////////////////////////////////////////////////////////////////
    void Update() {
        if(inupdate)
            return;
        inupdate = true;
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
            } else if(Input.GetKeyDown(KeyCode.Escape)) {
                studyProcess.Close();
            } else if(Input.GetKeyDown(KeyCode.A)) {
                studyProcess.SetRes(TrueAim.left);
            } else if(Input.GetKeyDown(KeyCode.C)) {
                studyProcess.SetRes(TrueAim.center);
            } else if(Input.GetKeyDown(KeyCode.D)) {
                studyProcess.SetRes(TrueAim.right);
            }
            break;
        default:
            break;
        }
        inupdate = false;
    } // ///////////////////////////////////////////////////////////////////////////////////
    //                  MODES
    bool waitSetAimBall() {
        ballaim.setRnd();   // normal, ballAim.transform.localScale.x
        //ballaim.setDeg(17.5f, 3.218f);         // TODO ОООООООООООООООООООООООООООООООООО
        ballaim.dbg("Aim ");
        d2p pluze = Field.luzeCornerAim(ballaim.curRad);
        d2p paim = d2p.rotate(pluze, (3f / 4f) * Mathf.PI + ballaim.curRad, ballaim.curDist);
        if(isOutRange(paim))
            return true;
        //paim.Set(13.42005f, 2.700449f);              // TODO ЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖ
        paim.setObj(ballAim);

        ballcue.setRnd(); // ballCue.transform.localScale.x 
        //ballcue.setVal(0.5f, 3.7341f);         // TODO ОООООООООООООООООООООООООООООООО
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

        float dkcue = 0.25f * studyProcess.dkcue;
        d2p pselout = d2p.addDist(paim, psel, Field.BallD * dkcue);
        pselout.setObj(aimLeft);
        d2p pselin = d2p.addDist(paim, psel, -Field.BallD * dkcue);
        pselin.setObj(aimRight);

        // Camera
        d2p ptarget = pvir;
        d2p pcam =  getPCameraHoriz(ptarget, pcue);

        d2p pbnd = new d2p(bounds); //  far point arc
        float wfar = pcam.dist(pbnd);
        float wnear = pcam.dist(pcue);
        float h = plcamera.transform.position.y;

        float degcamnear = d2p.rad2deg(Mathf.Atan2(h, wnear));
        float degcamfar = d2p.rad2deg(Mathf.Atan2(h - bounds.transform.position.y, wfar));
        float degcamavg = (degcamnear + degcamfar) / 2;
        Debug.Log("degcamavg " + degcamavg);

        float aCueTarget = pcue.rad(ptarget);
        float agCueCam = d2p.rad2deg(aCueTarget - Mathf.PI / 2);

        Quaternion rotation = Quaternion.Euler(degcamavg, agCueCam, 0);
        plcamera.transform.SetPositionAndRotation(
            new Vector3(pcam.x, plcamera.transform.position.y, pcam.z),
            rotation);

        float sectorHor = getHorSector(pcam, pcue, paim, pluze, Mathf.PI - aCueTarget);
        float sectorVert = degcamnear - degcamfar;
        float sectorMax = Mathf.Max(sectorVert, sectorHor);
        plcamera.fieldOfView = 1.05f * sectorMax;

        //paim.dbg("Aim ");
        //pcue.dbg("Cue ");
        //p.dbg("aim:" + ballaim.curDeg.ToString() + " dist:" + ballaim.curDist.ToString() + "  " + " k:" + ballcue.curK);
        mode = GameMode.waitChoice;
        return false;
    } // ///////////////////// EHD MODES ///////////////////////////////////////////////////
    bool waitSetAimBallbak() {
        ballaim.setRnd();   // normal, ballAim.transform.localScale.x
        //ballaim.setDeg(17.5f, 3.218f);         // TODO ОООООООООООООООООООООООООООООООООО
        ballaim.dbg("Aim ");
        d2p pluze = Field.luzeCornerAim(ballaim.curRad);
        d2p paim = d2p.rotate(pluze, (3f / 4f) * Mathf.PI + ballaim.curRad, ballaim.curDist);
        if(isOutRange(paim))
            return true;
        //paim.Set(13.42005f, 2.700449f);              // TODO ЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖ
        paim.setObj(ballAim);

        ballcue.setRnd(); // ballCue.transform.localScale.x 
        //ballcue.setVal(0.5f, 3.7341f);         // TODO ОООООООООООООООООООООООООООООООО
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
        d2p ptarget = pvir;
        d2p pcam =  getPCameraHoriz(ptarget, pcue);

        d2p pbnd = new d2p(bounds); //  far point arc
        float wfar = pcam.dist(pbnd);
        float wnear = pcam.dist(pcue);
        float h = plcamera.transform.position.y;

        float degcamnear = d2p.rad2deg(Mathf.Atan2(h, wnear));
        float degcamfar = d2p.rad2deg(Mathf.Atan2(h - bounds.transform.position.y, wfar));
        float degcamavg = (degcamnear + degcamfar) / 2;
        Debug.Log("degcamavg " + degcamavg);

        float aCueTarget = pcue.rad(ptarget);
        float agCueCam = d2p.rad2deg(aCueTarget - Mathf.PI / 2);

        Quaternion rotation = Quaternion.Euler(degcamavg, agCueCam, 0);
        plcamera.transform.SetPositionAndRotation(
            new Vector3(pcam.x, plcamera.transform.position.y, pcam.z),
            rotation);

        float sectorHor = getHorSector(pcam, pcue, paim, pluze, Mathf.PI - aCueTarget);
        float sectorVert = degcamnear - degcamfar;
        float sectorMax = Mathf.Max(sectorVert, sectorHor);
        plcamera.fieldOfView = 1.05f * sectorMax;

        //paim.dbg("Aim ");
        //pcue.dbg("Cue ");
        //p.dbg("aim:" + ballaim.curDeg.ToString() + " dist:" + ballaim.curDist.ToString() + "  " + " k:" + ballcue.curK);
        mode = GameMode.waitChoice;
        return false;
    } // ///////////////////// EHD MODES ///////////////////////////////////////////////////
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
        } else {
            Debug.Break();
        }
        return null;
    } // //////////////////////////////////////////////////////////////////////////////////////////////////
    void getAngle(d2p cam, d2p pnt, float diam, out float min, out float max) {
        float dist = cam.dist(pnt);
        float alfa = Mathf.Atan2(diam / 2, dist);
        float beta = Mathf.PI - cam.rad(pnt);
        beta = d2p.normrad(beta);
        //if(beta > Mathf.PI)
        //beta -= Mathf.PI;
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
    bool isOutRange(d2p p) {
        float xr = p.x + Field.BallR;
        float zr = p.z + Field.BallR;
        //return !(xr <= xmax && zr <= zmax && xr >= -xmax && zr >= -zmax);
        return !(xr < xmax && zr < zmax && xr > -xmax && zr > -zmax);
    } // /////////////////////////////////////////////////////////////////////////////////

} // ************************************************************************************
