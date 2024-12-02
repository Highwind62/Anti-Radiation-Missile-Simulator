using UnityEngine;
using System;

public class GridIntersect : MonoBehaviour
{
    public double Pr { get; set; } // Received power, in watts
    public double Pt { get; set; } // Transmitted power, in watts
    public double Lambda { get; set; } // Transmitted wavelength, in meters
    public double S { get; set; } // Power per unit area at a distance R, in meters
    public double R { get; set; } // Distance from transmitter to receiver, in meters
    public float creationTime;

    void Awake() {
        this.creationTime = Time.time;
    }

    public GridIntersect(double pt, double lambda, double r)
    {
        Pt = pt;
        Lambda = lambda;
        R = r;
    }

    public void SetPower(double power) {
        Pr = power;
    }

    public double GetPower() {
        return Pr;
    }


    // Calculate S = Pt / (4 * π * R^2)
    public double CalculatePowerPerUnitArea()
    {
        return Pt / (4 * Math.PI * Math.Pow(R, 2));
    }

    // Calculate Pr = (S * λ^2) / (4 * π)
    public double CalculateReceivedPower()
    {
        double S = CalculatePowerPerUnitArea(); 
        return (S * Math.Pow(Lambda, 2)) / (4 * Math.PI);
    }
}
