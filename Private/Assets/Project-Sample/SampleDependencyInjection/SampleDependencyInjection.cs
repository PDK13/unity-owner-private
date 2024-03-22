using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleDependencyInjection : MonoBehaviour //In edit, still not done!!
{
    private SampleDICar CarA;
    private SampleDICar CarB;

    private void Start()
    {
        CarA.SetWheel(new SampleDIWheel01());
        CarA.SetBattery(new SampleDIBattery02());
        CarA.Wheel.SetMove();
        //
        CarB.SetWheel(new SampleDIWheel02());
        CarB.SetBattery(new SampleDIBattery01());
        CarB.Battery.SetCharge();
    }
}

//

public class SampleDICar
{
    //Main data for all object in game!

    public SampleDIWheel Wheel;
    public SampleDIBattery Battery;

    public SampleDICar(SampleDIWheel Wheel, SampleDIBattery Battery)
    {
        this.Wheel = Wheel;
        this.Battery = Battery;
    }

    public void SetWheel(SampleDIWheel Wheel)
    {
        this.Wheel = Wheel;
    }

    public void SetBattery(SampleDIBattery Battery)
    {
        this.Battery = Battery;
    }
} //Base class that shouldn't change!

//

public class SampleDIWheel 
{
    public virtual void SetMove() { }
} //Base class that shouldn't change!

public class SampleDIWheel01 : SampleDIWheel { } //Child class that used by other!

public class SampleDIWheel02 : SampleDIWheel { } //Child class that used by other!

//

public class SampleDIBattery 
{
    public virtual void SetCharge() { }
} //Base class that shouldn't change!

public class SampleDIBattery01 : SampleDIBattery { } //Child class that used by other!

public class SampleDIBattery02 : SampleDIBattery { } //Child class that used by other!

//========================== ???

public interface SampleIService
{
    void Serve();
}

public class Service1 : SampleIService
{
    public void Serve()
    {
        Console.WriteLine("Service One");
    }
}

public class Service2 : SampleIService
{
    public void Serve()
    {
        Console.WriteLine("Service Two");
    }
}

public class SampleClient
{
    private SampleIService _service;

    public SampleClient(SampleIService service)
    {
        this._service = service;
    }

    public void Start()
    {
        Console.WriteLine("Service Started");
        this._service.Serve();
        //To Do: Some Stuff
    }
}

class SampleProgram
{
    static void Main(string[] args)
    {
        //Creating object with Dependency Injection
        SampleClient client = new SampleClient(new Service1());

        //Start the service
        client.Start();

        Console.ReadKey();
    }
}