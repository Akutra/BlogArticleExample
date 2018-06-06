using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// apiControllerRESTful
/// RESTful controller for DOT NET v4 or greater
/// Designed for ASP.NET page classes
/// Database access can be very controlled with 
/// custom classes, yet takes a bit more work
///
/// This is a base class and would be utilized
/// something like this
///
/// public class api_ArticlesController : apiControllerRESTful {}
///
/// --------------------------------------------
/// Created By : Akutra RA Cea
/// --------------------------------------------
/// </summary>
public abstract class apiControllerRESTful : System.Web.UI.Page, RESTService
{
    private MethodInfo[] methodList = null;
    private string _HTTPVerb;
    private Type _ControllerType;

    public abstract Type thisType { get; }
    public abstract Type objectType { get; }

    public apiControllerRESTful()
    {

    }

    public void ExecuteHTTPVerb(string HTTPVerb, object[] parameters)
    {
        _HTTPVerb = HTTPVerb.ToUpper();
        _ControllerType = thisType;
        
        MethodInfo cMethodInfo;

        cMethodInfo = FindMethod(_HTTPVerb, null); //locate the appropriate method by httpverb and submethod
        if(cMethodInfo!=null)
        {
            //invoke the method
            object obj = cMethodInfo.Invoke(this, null);

            if(obj!=null) //if return object is null then response is handled inside the method called.
                Response.Write(GenerateOutput(obj)); //output return in request specified format
        } else
        {

            throw new Exception("500 : Invalid HTTP Verb");
        }
    }

    public string GenerateOutput(object returnObject)
    {
        string rt = "", type = Request.ContentType; Response.ContentType = type;

        switch (type)
        {
            case "application/json":
                rt = JsonConvert.SerializeObject(returnObject);
                break;
            case "text/xml":
            case "application/xml":
                //todo 
                break;
            default:
                rt = returnObject.ToString();
                Response.ContentType = "text/plain";
                break;
        }

        return rt;
    }

    public object GenerateInputObject(Type object_type = null)
    {
        object rt; string type = Request.ContentType; Response.ContentType = type;

        StreamReader _reader = new StreamReader(Request.InputStream);

        //default to specified type
        if (object_type == null)
            object_type = objectType;

        switch (type)
        {
            case "application/json":
                //rt = new JavaScriptSerializer().Deserialize(_reader.ReadToEnd(), object_type);
                rt = JsonConvert.DeserializeObject(_reader.ReadToEnd(), object_type);
                break;
            case "text/xml":
            case "application/xml":
                //todo 
                break;
            default:
                rt = _reader.ReadToEnd();
                Response.ContentType = "text/plain";
                break;
        }

        return rt;
    }

    public MethodInfo FindMethod(string HTTPVerb, object[] parameters)
    {
        MethodInfo rt = null;
        Object[] attrMethods;
        Type attrHTTPVerb = typeof(HttpNullAttribute);
        int x_loop = 0;
        

        switch(HTTPVerb)
        {
            case "GET":
                //submethod of find will invoke find not get
                //if (parameters.Length > 0 && ((string)parameters[0]).ToLower() == "search")
                if (Request.QueryString.Count > 0 && (Request.QueryString["searchfor"]!=null || Request.QueryString["collectionId"]!=null) )
                {
                    attrHTTPVerb = typeof(SearchAttribute); //find user
                }
                else
                {
                    attrHTTPVerb = typeof(HttpGetAttribute); //get user
                }
                break;
            case "POST":
                attrHTTPVerb = typeof(HttpPostAttribute);
                break;
            case "PUT":
                attrHTTPVerb = typeof(HttpPutAttribute);
                break;
            case "HEAD":
                attrHTTPVerb = typeof(HttpHeadAttribute);
                break;
            case "DELETE":
                attrHTTPVerb = typeof(HttpDeleteAttribute);
                break;
            case "TRACE":
                attrHTTPVerb = typeof(HttpTraceAttribute);
                break;
            case "OPTIONS":
                attrHTTPVerb = typeof(HttpOptionsAttribute);
                break;
            case "CONNECT":
                attrHTTPVerb = typeof(HttpConnectAttribute);
                break;
        }

        if(methodList==null)
            methodList = _ControllerType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

        for (x_loop = 0; x_loop<methodList.Length; x_loop++)
        {
            if (attrHTTPVerb != typeof(HttpNullAttribute))
            {
                attrMethods = methodList[x_loop].GetCustomAttributes(attrHTTPVerb, false);
                if(attrMethods.Length>0)
                {
                    rt = methodList[x_loop];
                    break;
                }
            }
        }

        return rt;
    }
}

//Interface for the controller
public interface RESTService
{
    Type thisType { get; }
    Type objectType { get; }
}

public class searchfield
{
    public string searchfor;
}

// Add the TRACE attribute    
[System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = false)]
public class HttpTraceAttribute : Attribute
{
    // Constructor
    public HttpTraceAttribute()
    {
    }

}

// Add the CONNECT attribute    
[System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = false)]
public class HttpConnectAttribute : Attribute
{
    // Constructor
    public HttpConnectAttribute()
    {
    }

}

// Add the NULL attribute    
[System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = false)]
sealed class HttpNullAttribute : Attribute
{
    // Constructor
    public HttpNullAttribute()
    {
    }

}

// Add the FIND user sub-attribute    
[System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = false)]
public class SearchAttribute : Attribute
{
    // Constructor
    public SearchAttribute()
    {
    }

}


