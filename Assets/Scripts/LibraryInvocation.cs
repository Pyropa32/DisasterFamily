using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class LibraryInvocation : MonoBehaviour
{
    [DllImport("helloworld")]
    private static extern IntPtr hello_world();

    void Start()
    {
        try
        {
            var s = hello_world();
            var result = Marshal.PtrToStringAnsi(s);
            Debug.Log(result);
        }
        catch (DllNotFoundException e)
        {
            Debug.LogError("DLL not found: " + e.Message);
        }
        catch (Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
    }
}
