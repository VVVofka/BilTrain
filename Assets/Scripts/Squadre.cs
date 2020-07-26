using UnityEngine;

namespace Utils {
    public class Squdre {

        public Squdre(float a, float b, float c) {
            float D = b * b - 4 * a * c;
            if(D > 0) {
                cnt = 2;
                res1 = (-b + Mathf.Sqrt(D)) / (2 * a);
                res2 = (-b - Mathf.Sqrt(D)) / (2 * a);
            } else if(D == 0) {
                cnt = 1;
                res1 = res2 = -b / (2 * a);
            } else {
                cnt = 0;
                res1 = res2 = float.NaN;
            }
        }
        public int cnt { get; private set; }
        public float res1 { get; private set; }
        public float res2 { get; private set; }
    } // ********************************************************************
}