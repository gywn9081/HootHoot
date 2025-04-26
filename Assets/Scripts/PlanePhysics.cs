using System;
using System.Collections.Generic;
using UnityEngine;
namespace PhysicsButGood{
public class PlanePhysics : MonoBehaviour{
    public class LinearInterpolationTree{
        private List<(float, float)> tree;
        private int treeSize;
        public LinearInterpolationTree((float, float)[] values){
            if(values.Length < 2){
                //Maybe should be 1 but what do i know
                throw new Exception("Bad LinearInterpolationTree size.");
            }
            treeSize = 0;
            tree = new List<(float, float)>();
            foreach((float, float) v in values){
                tree.Add(v);
                treeSize++;
            }
            tree.Sort(Comparer<(float, float)>.Default);
        }
        public float getInterpolation(float value){
            int index;
            for(index = 0; index < treeSize - 1; index++){
                if((value > tree[index].Item1 && value < tree[index + 1].Item1) ||
                (index == 0 && value < tree[index].Item1)){
                    break;
                }
            }
            float m = (tree[index+1].Item2 - tree[index].Item2) / (tree[index+1].Item1 - tree[index].Item1);
            return value * m + tree[index].Item2 - m * tree[index].Item1;
        }

        public void addValue((float, float) value){
            tree.Add(value);
            tree.Sort(Comparer<(float, float)>.Default);
            return;
        }
    }

    private const float MaxThrust = 14670f; // Pounds
    private const float Afterburner = 25000f; // Pounds
    private const float WingArea = 608f; // ft2
    private const float SeaLevelPressure = 101325; // Pa
    private const float SeaLevelTemp = 288.15f; // Kelvin
    private const float GasConstantAir = 287.05f; // J/(kg·K)
    private const float LapseRate = 0.0065f; // K/m
    private const float Gravity = 9.80665f; // m/s²
    private const float PoundNewtonConversion = 4.44822f;
    private const float Ft2M2Conversion = 0.092903f;
    private const float MaxPitchSpeed = 25;
    private const float MaxRollSpeed = 25;
    private const float MaxYawSpeed = 25;
    private (float, float)[] ANGLE_CL_GRAPH = new (float, float)[]{
        (0.1f, 0),
        (0.68f, 10),
        (1.15f, 20),
        (1.2f, 22),
        (1.26f, 25),
        (1.34f, 30),
        (1.42f, 35),
        (1.5f, 37.5f),
        (1.6f, 40)
    };

    private (float, float)[] CL_DRAG_GRAPH = new (float, float)[]{
        (.06f, 0.05f),
        (.11f, 0.05f),
        (.17f, 0.0525f),
        (.25f, 0.0575f),
        (.3f, 0.07f),
        (.4f, 0.0925f),
        (.45f, 0.11f),
        (.475f, 0.1175f)
    };

    private LinearInterpolationTree AngleToCLTree;
    private LinearInterpolationTree CLToDragTree;

    public PlanePhysics(){
        AngleToCLTree = new LinearInterpolationTree(ANGLE_CL_GRAPH);
        CLToDragTree = new LinearInterpolationTree(CL_DRAG_GRAPH);
    }

    private float getDensity(float temp, float elevation){ // Celsius, meters
        temp = temp + 273.15f;

        float pressure = (float) Math.Pow(SeaLevelPressure * (1 - LapseRate * elevation / SeaLevelTemp), (Gravity / (GasConstantAir * LapseRate)));
        float density = pressure / (GasConstantAir * temp);

        return density;
    }


    private float getThrust(float input){

        // Normal thrust
        if(input < 1f){
            // Bound to 0.8
            if(input > 0.8f){
                input = 0.8f;
            }
            return MaxThrust * (input / 0.8f) * PoundNewtonConversion;
        }

        // Afterburner
        else{
            return Afterburner * PoundNewtonConversion;
        }
    }

    private float getLift(float angleOfAttack, float speed, float density){
        return 0.5f * AngleToCLTree.getInterpolation(angleOfAttack) * density * ((float) Math.Pow(speed, 2)) * WingArea * Ft2M2Conversion;
    }

    private float getDrag(float CL, float speed, float density){
        return 0.5f * CLToDragTree.getInterpolation(CL) * density * ((float) Math.Pow(speed, 2)) * WingArea * Ft2M2Conversion;
    }


    
    public Vector3 getForceVector(float angleOfAttack, float speed, float mass, float thrustPercent, float elevation){
        float density = getDensity(getTemperature(elevation), elevation);
    
        float thrust = getThrust(thrustPercent);

        float lift = getLift(angleOfAttack, speed, density);

        float drag = getDrag(AngleToCLTree.getInterpolation(angleOfAttack), speed, density);

        float gravity = mass * Gravity;

        Vector3 vectorSummation = new Vector3(0, lift - gravity, thrust - drag);

        return vectorSummation;
    }

    public float getTemperature(float elevation){
        return 15f - (0.0065f * elevation);
    }

    public Vector3 getRotationVector(float roll, float pitch, float yaw, float mass){
        return new Vector3(pitch * MaxPitchSpeed * mass, yaw * MaxYawSpeed, roll * MaxRollSpeed);
    }

}
}