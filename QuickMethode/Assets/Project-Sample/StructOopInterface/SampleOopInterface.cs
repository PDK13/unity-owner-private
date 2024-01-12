using UnityEngine;

public class SampleOopInterface : MonoBehaviour, ISampleClass
{
    public string Class { get; set; }

    private void Start()
    {
        SetStart();
    }

    public void SetStart()
    {
        ISampleStudent StudentA = new SampleObjectMover();
        StudentA.SetName("Adam");
        StudentA.SetLearn();
        //
        ISampleStudent StudentB = new SampleObjectMover("Bob");
        StudentB.SetPlay();
        //
        ISampleTeacher Teacher = new SampleObjectMover();
        Teacher.SetName("Eve");
        Teacher.SetTeach();
    }
}

//

public interface ISampleClass
{
    //All varible and methode in interface class must appear in child implements class with public!

    string Class { get; set; }

    void SetStart();
}

//

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

    //Both

    public string SetName(string Name)
    {
        this.Name = Name;
        return Name;
    } //Got the same methode, both Student and Teacher can call this!

    //Student

    public void SetLearn()
    {
        Debug.LogFormat("[Sample] Student {0} is learning!", Name);
    } //Got the difference methode, only Student can call this!

    public void SetPlay()
    {
        Debug.LogFormat("[Sample] Student {0} is playing!", Name);
    }

    //Teacher

    public void SetTeach()
    {
        Debug.LogFormat("[Sample] Teacher {0} is teaching!", Name);
    } //Got the difference methode, only Teacher can call this!
}

public interface ISampleStudent
{
    string Name { get; set; }

    string SetName(string Name);

    void SetLearn();

    void SetPlay();
}

public interface ISampleTeacher
{
    string Name { get; set; }

    string SetName(string Name);

    void SetTeach();
}