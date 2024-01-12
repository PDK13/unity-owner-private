using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleOopInterface : MonoBehaviour
{
    private void Start()
    {
        ISampleStudent StudentA = new SampleObjectMover();
        StudentA.SetName("Adam");
        StudentA.SetLearn();
        //
        ISampleStudent StudentB = new SampleObjectMover("Bob");
        StudentB.SetLearn();
        //
        ISampleTeacher Teacher = new SampleObjectMover();
        Teacher.SetName("Eve");
        Teacher.SetTeach();
    }
}

public class SampleObjectMover : ISampleStudent, ISampleTeacher
{
    public string Name { get; set; } //Got the same varible, both Student and Teacher can use this!

    public SampleObjectMover()
    {

    }

    public SampleObjectMover(string name)
    {
        Name = name;
    }

    //

    public string SetName(string Name)
    {
        this.Name = Name;
        return Name;
    } //Got the same methode, both Student and Teacher can call this!

    public void SetLearn()
    {
        Debug.LogFormat("[Sample] Student {0} is learning!", Name);
    } //Got the difference methode, only Student can call this!

    public void SetTeach()
    {
        Debug.LogFormat("[Sample] Teacher {0} is teaching!", Name);
    } //Got the difference methode, only Teacher can call this!
}

public interface ISampleStudent
{
    string Name { get; set; } //This varible must public!

    string SetName(string Name); 

    void SetLearn(); 
}

public interface ISampleTeacher
{
    string Name { get; set; }

    string SetName(string Name);

    void SetTeach();
}