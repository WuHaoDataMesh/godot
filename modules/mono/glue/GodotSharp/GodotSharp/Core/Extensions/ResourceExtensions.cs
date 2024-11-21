using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace Godot
{
    public partial class Resource
    {
        internal const int C_Awake = 0;
        internal const int C_OnEnable = 1;
        internal const int C_OnDisable = 2;
        internal const int C_Start = 3;
        internal const int C_Update = 4;
        internal const int C_LateUpdate = 5;
        internal const int C_FixedUpdate = 6;
        internal const int C_OnDestroy = 7;
        internal const int C_NUM = 8;

        public Node gameObject;

        public bool isBehaviour = false;

        public double deltaTime = 0;

        private bool hasAwake = false;
        private bool hasStart = false;

        public bool _enabled;
        public bool enabled
        {
            get { return _enabled; }
            set
            {
                if (_enabled == value) return;

                _enabled = value;

                if (_enabled)
                {
                    CallEnable();
                }
                else
                {
                    CallDisable();
                }
            }
        }

        public virtual void AttachToNode(Node node)
        {
            gameObject = node;
        }

        private class MethodData
        {
            public int pcount;
            public MethodInfo m;
        }

        private MethodData[] md = new MethodData[C_NUM];
        private static string[] names = new string[] { "Awake", "OnEnable", "OnDisable", "Start", "Update", "LateUpdate", "FixedUpdate", "OnDestroy" };

        private static MethodInfo GetMethodRecrusive(Type type, string key)
        {
            while (type != null)
            {
                MethodInfo methodInfo = type.GetMethod(key, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (methodInfo != null)
                {
                    return methodInfo;
                }
                type = type.BaseType;
            }
            return null;
        }

        internal void PerformMessage(int predefined_m, object value = null)
        {
            if (md[predefined_m] == null)
            {
                Type type = GetType();
                MethodInfo methodInfo = GetMethodRecrusive(type, names[predefined_m]);
                var t = new MethodData();
                if (methodInfo == null)
                {
                    t.m = methodInfo;
                    md[predefined_m] = t;
                    return;
                }
                var parameters = methodInfo.GetParameters();
                t.pcount = parameters.Length;
                t.m = methodInfo;
                md[predefined_m] = t;
            }
            if (md[predefined_m].m == null) return;
            if (md[predefined_m].pcount == 1)
            {
                md[predefined_m].m.Invoke(this, new object[] { value });
            }
            else
            {
                md[predefined_m].m.Invoke(this, null);
            }
        }

        internal void PerformMessage(string name, object value = null)
        {
            Type type = GetType();
            MethodInfo methodInfo = GetMethodRecrusive(type, name);
            if (methodInfo == null) return;
            var parameters = methodInfo.GetParameters();
            if (parameters.Length == 1)
            {
                methodInfo.Invoke(this, new object[] { value });
            }
            else
            {
                methodInfo.Invoke(this, null);
            }
        }


        internal void CallAwake()
        {
            if (!hasAwake)
            {
                PerformMessage(C_Awake);
                hasAwake = true;
            }
        }

        internal void CallEnable()
        {
            PerformMessage(C_OnEnable);
        }

        internal void CallDisable()
        {
            PerformMessage(C_OnDisable);
        }

        internal void CallStart()
        {
            if (!hasStart)
            {
                hasStart = true;
                PerformMessage(C_Start);
            }
        }

        internal void CallUpdate(double delta)
        {
            deltaTime = delta;
            PerformMessage(C_Update);
        }

        internal void CallLateUpdate()
        {
            PerformMessage(C_LateUpdate);
        }

        internal void CallFixedLateUpdate()
        {
            PerformMessage(C_FixedUpdate);
        }

        internal void CallOnDestroy()
        {
            PerformMessage(C_OnDestroy);
        }

    }
}
