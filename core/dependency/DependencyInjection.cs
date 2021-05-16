using Godot;
using Vortico.Core.Debug;
using Vortico.Utils;
using Vortico.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Vortico.Core.Dependency
{
    sealed class DependencyInjection : Node
    {
        #region Members

        private readonly Dictionary<Type, object> _installers = new Dictionary<Type, object>();

        #endregion


        #region Node

        public override void _Ready()
        {
            CreateInstallers();
            Install();
            Initialize();
        }

        #endregion


        #region Helpers

        private void CreateInstallers()
        {
            var mainAssembly = Assemblies.GetMainAssembly();
            var types = mainAssembly.GetTypes().Where(t => t.IsClass && t.IsAssignableTo(typeof(IInstaller)));
            foreach (var t in types)
            {
                var obj = Activator.CreateInstance(t);
                _installers.Add(t, obj);
            }
        }

        private void Install()
        {
            var container = GetNode<DependencyContainer>(DependencyContainer.Path);
            Assert.IsFalse(container == null, $"Failed to find {nameof(DependencyContainer)} node at {DependencyContainer.Path}");

            foreach (var kvp in _installers)
            {
                var installer = (IInstaller)kvp.Value;
                installer.Install(container);
            }
        }

        private void Initialize()
        {
            foreach (var kvp in _installers)
            {
                var installer = (IInstaller)kvp.Value;
                installer.Initialize();
            }
        }

        #endregion
    }
}