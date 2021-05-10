using Godot;
using Vortico.Utils;
using Vortico.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Vortico.Core.Dependency
{
    public sealed class DependencyInjection : Node
    {
        #region Members

        private readonly Dictionary<Type, object> _installers = new Dictionary<Type, object>();
        private readonly DependencyContainer _container = new DependencyContainer();
        private readonly ServiceLocator _locator;

        #endregion


        #region Constructor

        public DependencyInjection()
        {
            _locator = new ServiceLocator(_container);
        }

        #endregion


        public override void _Ready()
        {
            CreateInstallers();
            Install();
            Initialize();
        }


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
            foreach (var kvp in _installers)
            {
                var installer = (IInstaller)kvp.Value;
                installer.Install(_container);
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