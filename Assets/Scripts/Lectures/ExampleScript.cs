
using UnityEngine;

public abstract class Vehicle
{
    public void VehicleStuff()
    {
        //Stuff a vehicle can do
    }
}

public class Car : Vehicle
{
    public void CarStuff()
    {
        //Stuff a car can do
    }
}

public class Boat : Vehicle
{
    public void BoatStuff()
    {
        //Stuff a boat can do
    }
}

public class ExampleScript : MonoBehaviour
{
    
}
