﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace BossRush.Common
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TaskAttribute : Attribute
    {
        public double Weight { get; }
        public TaskAttribute(double weight = 1d)
        {
            Weight = weight;
        }
    }

    internal interface ITaskCollection
    {
        public List<GenPass> Tasks
        {
            get
            {
                List<GenPass> tasks = new();
                foreach (MethodInfo methodInfo in GetType().GetRuntimeMethods())
                {
                    TaskAttribute taskAttribute = methodInfo.GetCustomAttribute<TaskAttribute>();
                    if (taskAttribute is null)
                    {
                        continue;
                    }

                    tasks.Add(new PassLegacy(
                        methodInfo.Name,
                        (progress, configuration) => {
                            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                            switch (parameterInfos.Length)
                            {
                                case 0:
                                    methodInfo.Invoke(this, null);
                                    break;
                                case 1:
                                    if (parameterInfos[0].ParameterType == progress?.GetType())
                                    {
                                        methodInfo.Invoke(this, new object[] { progress });
                                    }
                                    else if (parameterInfos[0].ParameterType == configuration?.GetType())
                                    {
                                        methodInfo.Invoke(this, new object[] { configuration });
                                    }
                                    else
                                    {
                                        Logging.PublicLogger.Error("TaskAttribute method mismatched parameters.");
                                    }
                                    break;
                                case 2:
                                    if (parameterInfos[0].ParameterType == progress?.GetType() && parameterInfos[1].ParameterType == configuration?.GetType())
                                    {
                                        methodInfo.Invoke(this, new object[] { progress, configuration });
                                    }
                                    else if (parameterInfos[0].ParameterType == configuration?.GetType() && parameterInfos[1].ParameterType == progress?.GetType())
                                    {
                                        methodInfo.Invoke(this, new object[] { configuration, progress });
                                    }
                                    else
                                    {
                                        Logging.PublicLogger.Error("TaskAttribute method mismatched parameters.");
                                    }
                                    break;
                                default:
                                    Logging.PublicLogger.Error("TaskAttribute method too many parameters.");
                                    break;
                            }
                        },
                        taskAttribute.Weight
                     ));
                }

                return tasks;
            }
        }
    }
}
