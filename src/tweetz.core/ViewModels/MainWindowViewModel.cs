﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using tweetz.core.Infrastructure;

namespace tweetz.core.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChanged
    {
        public ISettings Settings { get; }
        public ISystemState SystemState { get; }
        private IWindowInteropService WindowInteropService { get; }
        private IEnumerable<ICommandBinding> CommandBindings { get; }

        public GetPinControlViewModel GetPinControlViewModel { get; }
        public TabBarControlViewModel TabBarControlViewModel { get; }

        public MainWindowViewModel(
            ISettings settings,
            ISystemState systemState,
            IWindowInteropService windowInteropService,
            GetPinControlViewModel getPinControlViewModel,
            TabBarControlViewModel tabBarControlViewModel,
            IEnumerable<ICommandBinding> commandBindings)
        {
            Settings = settings;
            SystemState = systemState;
            WindowInteropService = windowInteropService;
            GetPinControlViewModel = getPinControlViewModel;
            TabBarControlViewModel = tabBarControlViewModel;
            CommandBindings = commandBindings;
        }

        public void Initiate(Window window)
        {
            Settings.Load();
            WindowInteropService.DisableMaximizeButton(window);
            WindowInteropService.PowerManagmentRegistration(window, SystemState);
            WindowInteropService.SetWindowPosition(window, Settings.MainWindowPosition);
            window.CommandBindings.AddRange(CommandBindings.Select(cb => cb.CommandBinding()).ToList());
        }

        public bool Shutdown(Window window)
        {
            Settings.MainWindowPosition = WindowInteropService.GetWindowPosition(window);
            Settings.Save();
            return true;
        }
    }
}