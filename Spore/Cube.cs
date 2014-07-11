using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Runtime.Caching;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Spore
{
    public class Cube
    {

        static Cube()
        {
            //初始化缓存
            Cube.CommonCache = new MemoryCache(Constants.Spore_MemoryCache_ConfigName);
        }


        #region 容器相关

        /// <summary>
        /// Spore:IOC容器
        /// </summary>
        public static IUnityContainer UnityContainer
        {
            get
            {
                if (!Cube.CommonCache.Contains(Constants.Spore_UnityContainer_Cache_Key))
                {
                    //将容器放入缓存
                    Cube.CommonCache.Add(Constants.Spore_UnityContainer_Cache_Key,
                         new UnityContainer(),
                         new CacheItemPolicy() { }, null);
                }
                return Cube.CommonCache[Constants.Spore_UnityContainer_Cache_Key] as IUnityContainer;
            }
        }

        /// <summary>
        /// Spore:解析方法
        /// </summary>
        public static T Resolve<T>(params ResolverOverride[] overrides)
        {
            return Cube.UnityContainer.Resolve<T>(overrides);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="injectionMembers"></param>
        public static void RegisterType<TFrom, TTo>(params InjectionMember[] injectionMembers) where TTo : TFrom
        {
            Cube.UnityContainer.RegisterType<TFrom, TTo>(injectionMembers);
        }


        #endregion 


        #region 常用杂项封装

        /// <summary>
        /// Spore:配置
        /// </summary>
        public static System.Collections.Specialized.NameValueCollection AppSettings
        {
            get { return System.Configuration.ConfigurationManager.AppSettings; }
        }

        /// <summary>
        /// Spore:数据库连接字符串
        /// </summary>
        public static System.Configuration.ConnectionStringSettingsCollection ConnectionStrings
        {
            get { return System.Configuration.ConfigurationManager.ConnectionStrings; }
        }

        /// <summary>
        /// Spore:应用程序基本环境
        /// </summary>
        public static string Enviroment { get; private set; }


        #endregion


        #region 缓存

        /// <summary>
        /// Spore:进程内缓存
        /// </summary>
        public static System.Runtime.Caching.ObjectCache CommonCache { get; private set; }


        #endregion

    }
}
