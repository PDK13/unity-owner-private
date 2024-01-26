using System;
using System.Collections.Generic;
using UnityEngine;

public class IsometricDataFile
{
    private const string KEY_WORLD_NAME = "#WORLD-NAME";
    private const string KEY_WORLD_COMMAND = "#WORLD-COMMAND";

    //WORLD BLOCK
    private const string KEY_WORLD_BLOCK = "#WORLD-BLOCK";
    private const string KEY_WORLD_BLOCK_END = "#WORLD-BLOCK-END";
    //WORLD BLOCK

    private const string KEY_WORLD_END = "#WORLD-END";

    //BLOCK
    private const string KEY_BLOCK_INIT = "#BLOCK-INIT";
    private const string KEY_BLOCK_MOVE = "#BLOCK-MOVE";
    private const string KEY_BLOCK_ACTION = "#BLOCK-ACTION";
    private const string KEY_BLOCK_EVENT = "#BLOCK-EVENT";
    private const string KEY_BLOCK_EVENT_GET = "#BLOCK-EVENT_GET";
    private const string KEY_BLOCK_TELEPORT = "#BLOCK-TELEPORT";
    private const string KEY_BLOCK_END = "#BLOCK-END";
    //BLOCK

    #region Fild Write

    public static void SetFileWrite(IsometricManager Manager, string Path)
    {
        QDataFile FileIO = new QDataFile();

        SetFileWrite(Manager, FileIO);

        FileIO.SetWriteStart(Path);
    }

    private static void SetFileWrite(IsometricManager Manager, QDataFile FileIO)
    {
        Manager.World.Current.SetWorldOrder();
        //
        List<IsometricDataFileBlock> WorldBlocks = new List<IsometricDataFileBlock>();
        for (int i = 0; i < Manager.World.Current.PosH.Count; i++)
        {
            for (int j = 0; j < Manager.World.Current.PosH[i].Block.Count; j++)
            {
                WorldBlocks.Add(new IsometricDataFileBlock(Manager.World.Current.PosH[i].Block[j].PosPrimary, Manager.World.Current.PosH[i].Block[j].Name, (Manager.World.Current.PosH[i].Block[j].Data)));
            }
        }
        //
        //===============================NAME!!
        FileIO.SetWriteAdd(KEY_WORLD_NAME);
        FileIO.SetWriteAdd((Manager.World.Current.Name != "") ? Manager.World.Current.Name : "...");
        //===============================NAME!!
        //
        //===============================COMMAND!!
        FileIO.SetWriteAdd();
        FileIO.SetWriteAdd(KEY_WORLD_COMMAND);
        FileIO.SetWriteAdd(Manager.World.Current.Command.Count);
        for (int i = 0; i < Manager.World.Current.Command.Count; i++)
        {
            FileIO.SetWriteAdd(Manager.World.Current.Command[i]);
        }
        //===============================COMMAND!!
        //
        //===============================BLOCK!!
        FileIO.SetWriteAdd();
        FileIO.SetWriteAdd(KEY_WORLD_BLOCK);
        //
        //BLOCK START!!
        for (int BlockIndex = 0; BlockIndex < WorldBlocks.Count; BlockIndex++)
        {
            FileIO.SetWriteAdd();
            //
            FileIO.SetWriteAdd(WorldBlocks[BlockIndex].PosPrimary.Encypt);
            FileIO.SetWriteAdd(WorldBlocks[BlockIndex].Name);
            //
            //BLOCK START!!
            //
            if (WorldBlocks[BlockIndex].Data.Init.DataExist)
            {
                FileIO.SetWriteAdd(KEY_BLOCK_INIT);
                FileIO.SetWriteAdd(WorldBlocks[BlockIndex].Data.Init.Data.Count);
                for (int DataIndex = 0; DataIndex < WorldBlocks[BlockIndex].Data.Init.Data.Count; DataIndex++)
                {
                    FileIO.SetWriteAdd(WorldBlocks[BlockIndex].Data.Init.Data[DataIndex]);
                }
            }
            //
            if (WorldBlocks[BlockIndex].Data.Move.Data.Count > 0)
            {
                FileIO.SetWriteAdd(KEY_BLOCK_MOVE);
                FileIO.SetWriteAdd(WorldBlocks[BlockIndex].Data.Move.Type);
                FileIO.SetWriteAdd(WorldBlocks[BlockIndex].Data.Move.Data.Count);
                for (int DataIndex = 0; DataIndex < WorldBlocks[BlockIndex].Data.Move.Data.Count; DataIndex++)
                {
                    FileIO.SetWriteAdd(WorldBlocks[BlockIndex].Data.Move.Data[DataIndex].Encypt);
                }
            }
            //
            if (WorldBlocks[BlockIndex].Data.Action.Data.Count > 0)
            {
                FileIO.SetWriteAdd(KEY_BLOCK_ACTION);
                FileIO.SetWriteAdd(WorldBlocks[BlockIndex].Data.Action.Type);
                FileIO.SetWriteAdd(WorldBlocks[BlockIndex].Data.Action.Data.Count);
                for (int DataIndex = 0; DataIndex < WorldBlocks[BlockIndex].Data.Action.Data.Count; DataIndex++)
                {
                    FileIO.SetWriteAdd(WorldBlocks[BlockIndex].Data.Action.Data[DataIndex].Encypt);
                }
            }
            //
            if (WorldBlocks[BlockIndex].Data.Event.DataExist)
            {
                FileIO.SetWriteAdd(KEY_BLOCK_EVENT);
                FileIO.SetWriteAdd(WorldBlocks[BlockIndex].Data.Event.Identity.Count);
                for (int DataIndex = 0; DataIndex < WorldBlocks[BlockIndex].Data.Event.Identity.Count; DataIndex++)
                {
                    FileIO.SetWriteAdd(WorldBlocks[BlockIndex].Data.Event.Identity[DataIndex]);
                }
            }
            if (WorldBlocks[BlockIndex].Data.Event.DataGetExist)
            {
                FileIO.SetWriteAdd(KEY_BLOCK_EVENT_GET);
                FileIO.SetWriteAdd(WorldBlocks[BlockIndex].Data.Event.IdentityCheck.Count);
                for (int DataIndex = 0; DataIndex < WorldBlocks[BlockIndex].Data.Event.IdentityCheck.Count; DataIndex++)
                {
                    FileIO.SetWriteAdd(WorldBlocks[BlockIndex].Data.Event.IdentityCheck[DataIndex]);
                }
            }
            //
            if (WorldBlocks[BlockIndex].Data.Teleport.DataExist)
            {
                FileIO.SetWriteAdd(KEY_BLOCK_TELEPORT);
                FileIO.SetWriteAdd(WorldBlocks[BlockIndex].Data.Teleport.Encypt);
            }
            //
            //...
            //
            //BLOCK END!!
            //
            FileIO.SetWriteAdd(KEY_BLOCK_END);
        }
        FileIO.SetWriteAdd(KEY_WORLD_BLOCK_END);
        //===============================BLOCK!!
        //
        //===============================END!!
        FileIO.SetWriteAdd();
        FileIO.SetWriteAdd(KEY_WORLD_END);
        //===============================END!!
    }

    #endregion

    #region File Read

    public static void SetFileRead(IsometricManager Manager, string Path)
    {
        QDataFile FileIO = new QDataFile();

        FileIO.SetReadStart(Path);

        SetFileRead(Manager, FileIO);
    }

    public static void SetFileRead(IsometricManager Manager, TextAsset WorldFile)
    {
        QDataFile FileIO = new QDataFile();

        FileIO.SetReadStart(WorldFile);

        SetFileRead(Manager, FileIO);
    }

    private static void SetFileRead(IsometricManager Manager, QDataFile FileIO)
    {
        //
        //WORLD START!!
        //
        bool EndGroupWorld = false;
        do
        {
            switch (FileIO.GetReadAutoString())
            {
                case KEY_WORLD_NAME:
                    string MapName = FileIO.GetReadAutoString();
                    //NOTE: If world name read equas to world name exist in manager, the exist world will be overide!
                    Manager.World.Current = Manager.World.SetGenerate(MapName);
                    Manager.World.SetActive(MapName);
                    Manager.World.Current.SetWorldRemove(true);
                    break;
                case KEY_WORLD_COMMAND:
                    int CommandCount = FileIO.GetReadAutoInt();
                    //
                    Manager.World.Current.Command = new List<string>();
                    for (int CommandIndex = 0; CommandIndex < CommandCount; CommandIndex++)
                    {
                        Manager.World.Current.Command.Add(FileIO.GetReadAutoString());
                    }
                    break;
                case KEY_WORLD_BLOCK:
                    //WORLD BLOCK START!!
                    while (FileIO.GetReadAutoString() != KEY_WORLD_BLOCK_END)
                    {
                        IsometricVector PosPrimary = IsometricVector.GetDencypt(FileIO.GetReadAutoString());
                        string Name = FileIO.GetReadAutoString();
                        //
                        //BLOCK START!!
                        //
                        IsometricDataFileBlockData Data = new IsometricDataFileBlockData();
                        //
                        bool EndGroupBlock = false;
                        //
                        do
                        {
                            switch (FileIO.GetReadAutoString())
                            {
                                case KEY_BLOCK_INIT:
                                    Data.Init.Data = new List<string>();
                                    int InitCount = FileIO.GetReadAutoInt();
                                    for (int DataIndex = 0; DataIndex < InitCount; DataIndex++)
                                    {
                                        Data.Init.Data.Add(FileIO.GetReadAutoString());
                                    }
                                    break;
                                case KEY_BLOCK_MOVE:
                                    Data.Move = new IsometricDataMove
                                    {
                                        Type = FileIO.GetReadAutoEnum<DataBlockType>()
                                    };
                                    Data.Move.SetDataNew();
                                    int MoveCount = FileIO.GetReadAutoInt();
                                    for (int DataIndex = 0; DataIndex < MoveCount; DataIndex++)
                                    {
                                        Data.Move.SetDataAdd(IsometricDataBlockMoveSingle.GetDencypt(FileIO.GetReadAutoString()));
                                    }
                                    break;
                                case KEY_BLOCK_ACTION:
                                    Data.Action = new IsometricDataAction
                                    {
                                        Type = FileIO.GetReadAutoEnum<DataBlockType>()
                                    };
                                    Data.Action.SetDataNew();
                                    int ActionCount = FileIO.GetReadAutoInt();
                                    for (int DataIndex = 0; DataIndex < ActionCount; DataIndex++)
                                    {
                                        Data.Action.SetDataAdd(IsometricDataBlockActionSingle.GetDencypt(FileIO.GetReadAutoString()));
                                    }
                                    break;
                                case KEY_BLOCK_EVENT:
                                    Data.Event.Identity = new List<string>();
                                    int EventSetCount = FileIO.GetReadAutoInt();
                                    for (int DataIndex = 0; DataIndex < EventSetCount; DataIndex++)
                                    {
                                        Data.Event.Identity.Add(FileIO.GetReadAutoString());
                                    }
                                    break;
                                case KEY_BLOCK_EVENT_GET:
                                    Data.Event.IdentityCheck = new List<string>();
                                    int EventGetCount = FileIO.GetReadAutoInt();
                                    for (int DataIndex = 0; DataIndex < EventGetCount; DataIndex++)
                                    {
                                        Data.Event.IdentityCheck.Add(FileIO.GetReadAutoString());
                                    }
                                    break;
                                case KEY_BLOCK_TELEPORT:
                                    Data.Teleport = IsometricDataTeleport.GetDencypt(FileIO.GetReadAutoString());
                                    break;
                                case KEY_BLOCK_END:
                                    //
                                    //BLOCK END!!
                                    //
                                    EndGroupBlock = true;
                                    break;
                            }
                        }
                        while (!EndGroupBlock);
                        //
                        //BLOCK END!!
                        //
                        Manager.World.Current.SetBlockCreate(PosPrimary, Manager.List.GetList(Name), Data);
                    }
                    //WORLD BLOCK END!!
                    break;
                case KEY_WORLD_END:
                    //
                    //WORLD END!!
                    //
                    EndGroupWorld = true;
                    break;
            }
        }
        while (!EndGroupWorld);
        //
        //WORLD END!!
        //
        Manager.World.Current.onCreate?.Invoke();
    }

    #endregion
}

public class IsometricDataFileBlock
{
    public IsometricVector PosPrimary;
    public string Name;
    public IsometricDataFileBlockData Data = new IsometricDataFileBlockData();

    public IsometricDataFileBlock(IsometricVector Pos, string Name, IsometricDataFileBlockData Data)
    {
        PosPrimary = Pos;
        this.Name = Name;
        this.Data = Data;
    }
}

[Serializable]
public class IsometricDataFileBlockData
{
    public IsometricDataInit Init = new IsometricDataInit();
    public IsometricDataMove Move = new IsometricDataMove();
    public IsometricDataAction Action = new IsometricDataAction();
    public IsometricDataEvent Event = new IsometricDataEvent();
    public IsometricDataTeleport Teleport = new IsometricDataTeleport();
}