﻿using System;
using System.Collections.Generic;

namespace DotNet.DependencyInjectionBenchmarks.Containers
{
    public enum RegistrationMode
    {
        Single,
        Multiple,
    }

    public enum RegistrationLifestyle
    {
        Transient,
        Singleton,
        SingletonPerScope,
        SingletonPerObjectGraph,
        SingletonPerNamedScope,
        SingletonPerAncestor
    }

    public enum MemberInjectionType
    {
        Method,
        Field,
        Property
    }

    public class MemberInjectionInfo
    {
        public MemberInjectionType InjectionType { get; set; }

        public string MemberName { get; set; }
    }

    public class RegistrationDefinition
    {
        public Type ActivationType { get; set; }

        public Type ExportType { get; set; }

        public object ExportKey { get; set; }

        public RegistrationMode RegistrationMode { get; set; } = RegistrationMode.Single;

        public RegistrationLifestyle RegistrationLifestyle { get; set; } = RegistrationLifestyle.Transient;

        public object LifestyleInformation { get; set; }

        public Dictionary<object, object> Metadata { get; set; }

        public List<MemberInjectionInfo> MemberInjectionList { get; set; }
    }

    public interface IResolveScope : IDisposable
    {
        IResolveScope CreateScope(string scopeName = "");

        object Resolve(Type type);

        object Resolve(Type type, object data);

        bool TryResolve(Type type, object data, out object value);
    }

    public static class IResolveScopeExtensions
    {
        public static T Resolve<T>(this IResolveScope scope)
        {
            return (T)scope.Resolve(typeof(T));
        }
    }

    public interface IContainer : IResolveScope
    {
        string DisplayName { get; }

        string Version { get; }

        string WebSite { get; }

        void BuildContainer();

        void Registration(IEnumerable<RegistrationDefinition> definitions);

        void RegisterFactory<TResult>(Func<TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle) where TResult : class;

        void RegisterFactory<T1, TResult>(Func<T1, TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle) where TResult : class;

        void RegisterFactory<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle) where TResult : class;
    }

    public static class ContainerScopeExtensions
    {
        public static void Register<TInterface, TImplementation>(this IContainer scope,
            RegistrationLifestyle lifestyle = RegistrationLifestyle.Transient) where TImplementation : TInterface
        {
            scope.Registration(new[] { new RegistrationDefinition { ExportType = typeof(TInterface), ActivationType = typeof(TImplementation) } });
        }
    }
}
