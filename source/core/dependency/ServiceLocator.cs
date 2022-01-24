using System.Collections.Generic;
using Godot;
using Vortico.Core.Debug;
using Vortico.Utils.Extensions;
using Vortico.Core.Console;

namespace Vortico.Core.Dependency
{
	public sealed class ServiceLocator : Node
	{
		private static ServiceLocator _instance;
		private static DependencyContainer _container;

		internal ServiceLocator()
		{
			_instance = this;
			Log.Debug(nameof(ServiceLocator), "Node built");
		}

		public override void _Ready()
		{
			_container = GetNode<DependencyContainer>("/root/DependencyContainer");
			Assert.IsTrue(_container != null, "Dependency container not found");
		}

		public static T Locate<T>()
		{
			var type = typeof(T);
			var obj = (T)_container.FindOrCreateType(type);
			Assert.IsTrue(obj != null, $"Could not locate type {typeof(T)}");
			return obj;
		}

		public static List<T> LocateAsList<T>()
		{
			var type = typeof(T);
			//
			var objs = _container.FindList(type);
			Assert.IsFalse(objs.Count == 0, $"Could not locate type {typeof(T)}");
			//
			var list = new List<T>(objs.Count);
			objs.ForEach(obj => list.Add((T)obj));
			//
			return list;
		}

		public static void Register<I, C>(C instance) where C : I
		{
			_container.Instance<I, C>(instance);
		}
	}
}
