using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public enum Type {
        FILEMANAGER = 1,
        SKILLFILEMANAGER = 2,
        SKILLMANAGER = 3,
        GAMEMANAGER = 4,
        Unknown = -1,
    }
    public class Console {
        
        // 나중 로그 체크해서 해야될 작업이 있다면 리스트로 각각 로그 넣어서 처리함.
        // 처리하기 위해서는 타이머가 필요하고 이 리스트 요소에 모두 시간의 변수가 필요함.
        private List<LogData> logs;
        private List<LogData> errors;

        public Console()
        {
            logs = new List<LogData>();
            errors = new List<LogData>();
        }


        public void Print(Type type, string detail, int id)
        {
            
            Debug.Log($"[{GetConsoleType(type)}]: {detail}");
            logs.Add(new LogData(GetConsoleType(type), detail, id));
        }

        public void ErrorPrint(Type type, string detail, int id)
        {
            Debug.LogError($"[{GetConsoleType(type)}]: {detail}");
            errors.Add(new LogData(GetConsoleType(type), detail, id));
        }

        public void Print(Type type, string detail)
        {
            Debug.Log($"[{GetConsoleType(type)}]: {detail}");
            logs.Add(new LogData(GetConsoleType(type), detail, -1));

        }

        public void ErrorPrint(Type type, string detail)
        {
            Debug.LogError($"[{GetConsoleType(type)}]: {detail}");
            errors.Add(new LogData(GetConsoleType(type), detail, -1));

        }
        
        public void Print(string detail) { Debug.Log($"[{GetConsoleType(Type.Unknown)}]: {detail}"); }
        
        public void ErrorPrint(string detail) { Debug.LogError($"[{GetConsoleType(Type.Unknown)}]: {detail}");  }
        
        private string GetConsoleType(Type id) {
            switch (id) {
                case Type.FILEMANAGER: return "FileManager";
                case Type.GAMEMANAGER: return "GameManager";
                case Type.SKILLMANAGER: return "SkillManager";
                case Type.SKILLFILEMANAGER: return "SkillFileManager";
                default: return "Unknown";
            }
        }


        public bool GetLogId(int id)
        {
            return false;
        }
        
        
    }

    /// <summary>
    /// LogData관리 클래스
    ///
    /// log를 발생시킨 클래스
    /// log의 내용
    /// log의 id
    /// 
    /// </summary>
    class LogData {
        public string className, detail;
        public int  id;
        
        public LogData(string className, string detail, int id)
        {
            this.className = className;
            this.detail = detail;
            this.id = id;
        }
        public LogData(string className, string detail) {
            this.className = className;
            this.detail = detail;
        }
    }
}