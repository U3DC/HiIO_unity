﻿//****************************************************************************
// Description: 文件常用操作
// Author: hiramtan@qq.com
//****************************************************************************
using System;
using System.IO;
using UnityEngine;
using System.Collections;
namespace HiIO
{
    public class MyIO
    {
        public bool IsFolderExist(string param)
        {
            return Directory.Exists(param);
        }

        public void CreateFolder(string param)
        {
            Directory.CreateDirectory(param);
        }

        public void DeleteFolder(string param)
        {
            Directory.Delete(param, true);//第二个参数：删除子目录
        }

        public bool IsFileExist(string param)
        {
            return File.Exists(param);
        }
        Action<byte[]> finishLoadFromStreamingAssetsPathHandler;
        public void ReadFileFromStreamingAssetsPath(string paramPath, Action<byte[]> paramBytesHandler)
        {
            finishLoadFromStreamingAssetsPathHandler = paramBytesHandler;
            paramPath = GetStreamingAssetsPath() + paramPath;
            WWWLoader.Instance.Startload(paramPath, FinishLoadFromStreamingAssetsPath);
        }
        private void FinishLoadFromStreamingAssetsPath(WWW paramWWW)
        {
            finishLoadFromStreamingAssetsPathHandler(paramWWW.bytes);
        }

        public byte[] ReadFileFromPersistentDataPath(string param)
        {
            param = Application.persistentDataPath + param;
            return ReadFile(param);
        }
        public byte[] ReadFile(string param)
        {
            try
            {
                using (FileStream fs = new FileStream(param, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, (int)fs.Length);
                    return bytes;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
            return null;
        }
        public void WriteFileToPersistentDataPath(string paramPath, byte[] paramBytes)
        {
            paramPath = Application.persistentDataPath + paramPath;
            WriteFile(paramPath, paramBytes);
        }
        public void WriteFile(string paramPath, byte[] paramBytes)
        {
            try
            {
                using (FileStream fs = new FileStream(paramPath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(paramBytes, 0, paramBytes.Length);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
        public void DeleteFile(string param)
        {
            File.Delete(param);
        }
        private string GetStreamingAssetsPath()
        {
            if ((Application.platform == RuntimePlatform.WindowsEditor) || (Application.platform == RuntimePlatform.OSXEditor))
                return Application.streamingAssetsPath + "/";
            else if ((Application.platform == RuntimePlatform.WindowsWebPlayer) || (Application.platform == RuntimePlatform.OSXWebPlayer))
                return "StreamingAssets/";
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
                return Application.dataPath + "/Raw/";
            else if (Application.platform == RuntimePlatform.Android)
                return "jar:file://" + Application.dataPath + "!/assets/" + "/";
            else
                return "Cann't distinguish your platform";
        }
    }
}