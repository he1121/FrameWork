using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GeneratorType
{
    Find,
    Bind
}

public enum ParseType
{
    Name,
    Tag,
}
public class GeneratorConfig
{
    public static string FindComponentGeneratorPath = Application.dataPath + "/UIFrameWork/Scripts/FindComponent";
    public static string BindComponentGeneratorPath = Application.dataPath + "/UIFrameWork/Scripts/BindComponent";
    public static string WindowGeneratorPath = Application.dataPath + "/UIFrameWork/Scripts/Window";
    
    public static string OBJDATALIST_KEY = "objDataList";
    
    public static GeneratorType GeneratorType = GeneratorType.Bind;
    
    public static ParseType ParseType = ParseType.Tag;

    public static string[] TAGArr =
    {
        "Image",
        "RawImage",
        "Text",
        "Button",
        "Slider",
        "Dropdown",
        "InputField",
        "Canvas",
        "Panel",
        "ScrollRect",
        "Toggle",
    };
}
