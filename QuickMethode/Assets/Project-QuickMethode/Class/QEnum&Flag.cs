using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QEnum
{
    public static int GetChoice<EnumType>(EnumType Choice)
    {
        //Simple: (int)EnumType
        return (int)Convert.ChangeType(Choice, typeof(int));
    }

    public static EnumType GetChoice<EnumType>(int Index)
    {
        //Simple: (EnumType)Index
        return (EnumType)Enum.ToObject(typeof(EnumType), Index);
    }

    public static List<string> GetListName<EnumType>(bool Fixed = true)
    {
        if (Fixed)
        {
            List<string> ListName = Enum.GetNames(typeof(EnumType)).ToList();
            for (int i = 0; i < ListName.Count; i++)
            {
                if (ListName[i][0].Equals('_'))
                {
                    ListName[i] = ListName[i].Remove(0, 1);
                }
                //
                ListName[i] = ListName[i].Replace("_", " ");
            }
            return ListName;
        }

        return Enum.GetNames(typeof(EnumType)).ToList();
    }

    public static List<int> GetListIndex<EnumType>()
    {
        return Enum.GetValues(typeof(EnumType)).Cast<int>().ToList();
    }

    public static List<int> GetListIndex<EnumType>(params EnumType[] Value)
    {
        List<int> Index = new List<int>();
        for (int i = 0; i < Value.Length; i++)
        {
            Index.Add((int)Convert.ChangeType(Value[i], typeof(int)));
        }

        return Index;
    }

    public static string GetName<EnumType>(int Index)
    {
        return Enum.GetName(typeof(EnumType), Index);
    }
}

public class QFlag
{
    //NOTE:
    //Bit       : "1 << 3" mean "0100" or "8"
    //Bit |     : 1 | 0 = 1 + 0 = 1
    //Bit &     : 1 & 0 = 1 * 0 = 0
    //Bit ~     : Revert Bit, like ~8 = ~0100 = 1011 + 1 = 1100 = -9 (?)
    //Add       : "Flag = Flag.A | Flag.B | Flag.C"
    //Remove    : "Flag &= ~Flag.A"
    //Exist     : "(Flag & Flag.A) == Flag.A" or "Flag.HasFlag(Flag.A)"
    //Emty      : "Alpha == 0"

    public static List<int> GetBit<EnumType>()
    {
        List<int> Index = QEnum.GetListIndex<EnumType>();
        for (int i = 0; i < Index.Count; i++)
        {
            if (i == 0)
            {
                Index[i] = 1;
            }
            else
            {
                Index[i] = Index[i - 1] * 2;
            }
        }

        return Index;
    }

    public static int GetChoice<EnumType>(params EnumType[] Choice)
    {
        int Sum32 = 0;
        foreach (EnumType Value in Choice)
        {
            int Value32 = (int)Convert.ChangeType(Value, typeof(int));
            Sum32 += Value32;
        }
        return Sum32;
    }

    public static int GetAdd<EnumType>(EnumType Current, params EnumType[] Choice)
    {
        int Sum32 = GetChoice(Current);
        foreach (EnumType Value in Choice)
        {
            if (GetExist(Current, Value))
            {
                continue;
            }

            int Value32 = (int)Convert.ChangeType(Value, typeof(int));
            Sum32 += Value32;
        }
        return Sum32;
    }

    public static int GetRemove<EnumType>(EnumType Current, params EnumType[] Choice)
    {
        int Sum32 = GetChoice(Current);
        foreach (EnumType Value in Choice)
        {
            if (!GetExist(Current, Value))
            {
                continue;
            }

            int Value32 = (int)Convert.ChangeType(Value, typeof(int));
            Sum32 -= Value32;
        }
        return Sum32;
    }

    public static bool GetExist<EnumType>(EnumType Current, params EnumType[] Check)
    {
        return (GetChoice(Current) & GetChoice(Check)) == GetChoice(Check);
    }

    public static bool GetEmty<EnumType>(EnumType Current)
    {
        return GetChoice(Current) == 0;
    }
}

public enum Opption { Yes = 1, No = 0 }

public enum Direction { None, Up, Down, Left, Right, }

public enum DirectionX { None = 0, Left = -1, Right = 1, }

public enum DirectionY { None = 0, Up = 1, Down = -1, }

public enum Axis { Up, Right, Forward, }

[Flags]
public enum Coordinates
{
    X = 1 << 0, //001 = 1
    Y = 1 << 1, //010 = 2
    Z = 1 << 2, //100 = 4
}