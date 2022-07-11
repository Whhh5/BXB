using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System.IO;

public class CommonFilePath : MiBaseClass
{
    public readonly static string dirChar = $"{Path.DirectorySeparatorChar}";


    public readonly static string PreComPath = "Prefab/Battle";
    public readonly string PreComSkillsPath = $"{PreComPath}{dirChar}Skills";
    public readonly string PreComEffectsPath = $"{PreComPath}{dirChar}Effects";
    public readonly string PreComArrticlesPath = $"{PreComPath}{dirChar}Arrticles";


    public readonly static string PreUIPath = "Prefab/UI";
    public readonly string PreUIDialogPath = $"{PreUIPath}{dirChar}Dialog";
    public readonly string PreUIDialogSystemPath = $"{PreUIPath}{dirChar}Dialog/System";
    public readonly string PreUIElementPath = $"{PreUIPath}{dirChar}UIElement";



    public readonly string ResLocalDataPath = "Assets/Resources/DataAssets/Localize";
    public readonly string ResRolesPrePath = "Prefab/Roles";
    public readonly string ResArt = "Art";

    public readonly string ResArticle = "Prefab/Articles";
}
