﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebStore.Services.Helpers
{
    public static class Mapper
    {
        private static List<ConfigurationMap> ConfigurationMaps = new List<ConfigurationMap>();
       
        public static ConfigurationMap CreateMap<TSource, TDestination>()
        {
            var conf = ConfigurationMaps.FirstOrDefault(cm => cm.SourceType.Equals(typeof(TSource))
                && cm.DestinationType.Equals(typeof(TDestination)));

            if (conf == null)
            {
                conf = new ConfigurationMap()
                {
                    SourceType = typeof(TSource),
                    DestinationType = typeof(TDestination)
                };

                var destProp = conf.DestinationType.GetProperties();
                var propNames = destProp.Select(p => p.Name).ToList();

                conf.SourceType.GetProperties().Where(p => propNames.Contains(p.Name))
                    .ToList()
                    .ForEach(pi => conf.MembersForMap.Add(pi.Name, pi.Name));

               
                List<MemberInfo> sourceMembers = conf.SourceType
                    .GetProperties(BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.IgnoreCase)
                    .ToList<MemberInfo>();
                sourceMembers.AddRange(conf.DestinationType
                    .GetFields(BindingFlags.Instance | BindingFlags.GetField | BindingFlags.Public | BindingFlags.NonPublic));

                //Находим соответствующие наименования полей и строим карту маппинга
                sourceMembers.ForEach(sm =>
                {
                    var dm = conf.DestinationType.GetMember(sm.Name, BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.IgnoreCase).FirstOrDefault();
                    if (dm != null)
                        conf.MembersMap.Add(sm, dm);
                });

                ConfigurationMaps.Add(conf);
            }

            return conf;
        }
       
        public static TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
            where TDestination : class, new()
            where TSource : class, new()
        {
            TDestination res = (destination == null) ? new TDestination() : destination;

            if (source != null)
            {
                var conf = GetConfigurationMap<TSource, TDestination>();
               
                foreach (var kvp in conf.MembersMap)
                {
                    var sourceVal = kvp.Key.MemberType.Equals(MemberTypes.Field) ?
                        ((FieldInfo)kvp.Key).GetValue(source)
                        :
                        kvp.Key.MemberType.Equals(MemberTypes.Property) ?
                        ((PropertyInfo)kvp.Key).GetValue(source)
                        :
                        null;

                    if (sourceVal == null)
                        continue;

                    if (kvp.Value.MemberType.Equals(MemberTypes.Field))
                    {
                        var fi = (FieldInfo)kvp.Value;
                        if(ConvertValue(sourceVal, fi.FieldType, out object result))
                            fi.SetValue(res, result);
                    }
                    else if (kvp.Value.MemberType.Equals(MemberTypes.Property))
                    {
                        var pi = (PropertyInfo)kvp.Value;
                        if (ConvertValue(sourceVal, pi.PropertyType, out object result))
                            pi.SetValue(res, result);
                    }
                }
            }

            return res;
        }

        public static TDestination Map<TSource, TDestination>(TSource source)
            where TDestination : class, new()
            where TSource : class, new()
        {
            return Map<TSource, TDestination>(source, null);
        }
       
        private static ConfigurationMap GetConfigurationMap<TSource, TDestination>()
        {
            return ConfigurationMaps.FirstOrDefault(cm => cm.SourceType.Equals(typeof(TSource))
                && cm.DestinationType.Equals(typeof(TDestination)))
                ??
                CreateMap<TSource, TDestination>();
        }

        public static bool ConvertValue(object source, Type type, out object result)
        {
            var success = true;
            result = null;

            if (source != null) // && !source.GetType().Equals(type))
            {
                if (type == typeof(string))
                {
                    result = source.ToString();
                }
                if (type.Equals(typeof(Guid)) || type.Equals(typeof(Guid?)))
                {
                    if(Guid.TryParse(source.ToString(), out Guid val));
                        result = val;
                }
                if (type.Equals(typeof(DateTime)) || type.Equals(typeof(DateTime?)))
                {
                    if (DateTime.TryParse(source.ToString(), out DateTime val))
                        result = val;
                }
                // int, int?
                if (type.Equals(typeof(int)) || type.Equals(typeof(int?)))
                {
                    if (int.TryParse(source.ToString(), out int val))
                        result = val;
                }
                // decimal, decimal?
                if (type.Equals(typeof(decimal)) || type.Equals(typeof(decimal?)))
                {
                    if (decimal.TryParse(source.ToString(), out decimal val))
                        result = val;
                }
                if (type.Equals(typeof(bool)) || type.Equals(typeof(bool?)))
                {
                    if (bool.TryParse(source.ToString(), out bool val))
                        return val;                   
                }

                success = result != null;
            }

            return success;
        }

        public class ConfigurationMap
        {
            internal Type SourceType;
            internal Type DestinationType;
            internal Dictionary<string, string> MembersForMap;
            internal Dictionary<MemberInfo, MemberInfo> MembersMap;

            internal ConfigurationMap()
            {
                MembersForMap = new Dictionary<string, string>();
                MembersMap = new Dictionary<MemberInfo, MemberInfo>();
            }

            public ConfigurationMap ForMember(string sourceMember, string destinationMember)
            {
                if (MembersForMap.ContainsKey(sourceMember))
                    MembersForMap[sourceMember] = destinationMember;
                else
                    MembersForMap.Add(sourceMember, destinationMember);

                var miSrc = SourceType.GetMember(sourceMember, BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.IgnoreCase)
                    .FirstOrDefault();
                var miDest = DestinationType.GetMember(destinationMember, BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.IgnoreCase)
                    .FirstOrDefault();

                if (miSrc == null)
                    throw new Exception($"Ошибка маппинга! Тип { SourceType.GetType().FullName } не содержит свойство с именем { sourceMember }");
                if (miDest == null)
                    throw new Exception($"Ошибка маппинга! Тип { DestinationType.GetType().FullName } не содержит свойство с именем { destinationMember }");

                if (MembersMap.ContainsKey(miSrc))
                    MembersMap[miSrc] = miDest;
                else
                    MembersMap.Add(miSrc, miDest);

                return this;
            }
        }
    }
}
