using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

//[CreateAssetMenu(fileName ="C", menuName ="A/New C")]
public class TemplateClass
{
    public enum TemplateFlags
    {
        parameter,
        commonClass,
        asset,
        singleton,
        dictionary,
    }
    public static string parameter =
        "\tpublic {0} {1} = default;\n";
    public static string commonClass =
        "using System.Collections.Generic;\n" +
        "using UnityEngine; \n" +
        "[System.Serializable] \n" +
        "public class {2} \n" +
        "{0} \n" +
        "{3} \n" +
        "{1} \n";
    public static string asset =
        "using System.Collections.Generic;\n" +
        "using UnityEngine; \n" +
        "public class {2} : ScriptableObject \n" +
        "{0} \n" +
            "{3} \n" +
        "{1} \n";
    public static string partialClass =
        "using System.Collections.Generic;\n" +
        "public partial class {2}\n" +
        "{0}\n" +
            "{3}\n" +
        "{1}\n";

    public static string dictionary = "" +
        "\tpublic Dictionary<{0}, {1}> {1} = new Dictionary<{0}, {1}>();";
    public static string CreateAssetScript(string ClassName, string parameters)
    {
        var text = string.Format(commonClass, '{', '}', ClassName, parameters);
        return text;
    }
    public static string CreateAsset(string ClassName, string parameters)
    {
        var text = string.Format(asset, '{', '}', ClassName, parameters);
        return text;
    }

    public static string CreateParameterAndScript(TemplateFlags templateFlags, params string[] parameter)
    {
        var type = Type.GetType("TemplateAssetClass");
        var instance = Activator.CreateInstance(type);
        var templateField = type.GetField(templateFlags.ToString(), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
        var template = (string)templateField.GetValue(instance);
        template = string.Format(template, '{', '}', parameter);
        return template; 
    }
    public static string CreateParameter(TemplateFlags templateFlags, params string[] parameter)
    {
        var type = Type.GetType("TemplateAssetClass");
        var instance = Activator.CreateInstance(type);
        var templateField = type.GetField(templateFlags.ToString(), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
        var template = (string)templateField.GetValue(instance);
        template = string.Format(template, parameter);
        return template;
    }

}