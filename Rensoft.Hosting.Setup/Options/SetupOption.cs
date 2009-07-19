using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using WindowsInstaller;
using System.Windows.Forms;
using Microsoft.Win32;
using Rensoft.Platform;

namespace Rensoft.Hosting.Setup.Options
{
    public abstract class SetupOption
    {
        private bool suspendEnabledChanged;
        private bool enabled;

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                if (!suspendEnabledChanged && (EnabledChanged != null))
                {
                    EnabledChanged(this, EventArgs.Empty);
                }
            }
        }

        public string SetupFilePath
        {
            get { return Path.Combine(SetupWizard.SetupPath, SetupFileName); }
        }

        public SetupStatus Status { get; set; }
        public string Message { get; set; }
        public int StatusIndex { get; set; }
        public CheckBox ComponentCheckBox { get; set; }
        public abstract string ProductName { get; }
        public abstract string SetupFileName { get; }
        public abstract string RegistryKeyName { get; }
        public SetupWizardForm SetupWizard { get; private set; }

        public event EventHandler EnabledChanged;

        public SetupOption(SetupWizardForm setupWizard)
        {
            this.SetupWizard = setupWizard;
            initializeComponentCheckBox();
            EnabledChanged += new EventHandler(SetupOption_EnabledChanged);  
        }

        private void initializeComponentCheckBox()
        {
            ComponentCheckBox = new CheckBox();
            ComponentCheckBox.Text = ProductName;
            ComponentCheckBox.AutoSize = true;
            ComponentCheckBox.CheckedChanged += new EventHandler(ComponentCheckBox_CheckedChanged);
        }

        void SetupOption_EnabledChanged(object sender, EventArgs e)
        {
            suspendEnabledChanged = true;
            SetupWizard.Invoke((MethodInvoker)delegate() { ComponentCheckBox.Checked = Enabled; });
            suspendEnabledChanged = false;
        }

        void ComponentCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!suspendEnabledChanged)
            {
                suspendEnabledChanged = true;
                Enabled = ComponentCheckBox.Checked;
                suspendEnabledChanged = false;
            }
        }

        public virtual bool ProcessOption(UpdateStatusDelegate updateStatus)
        {
            return true;
        }

        public virtual void Load() { }

        protected RegistryKey Get32BitSoftwareRegistryKey()
        {
            string key;
            switch (PlatformInfo.CurrentPlatformType)
            {
                case PlatformType.X86:
                    key = @"Software";
                    break;

                case PlatformType.X64:
                    key = @"Software\Wow6432Node";
                    break;

                default:
                    throw new NotSupportedException(
                        "The current platform is neither 32-bit or 64-bit.");
            }

            return Registry.LocalMachine.OpenSubKey(key, true);
        }

        protected RegistryKey Get32BitUninstallRegistryKey()
        {
            return Get32BitSoftwareRegistryKey().OpenSubKey(
                @"Microsoft\Windows\CurrentVersion\Uninstall", true);
        }

        protected string Get32BitProgramFilesPath()
        {
            switch (PlatformInfo.CurrentPlatformType)
            {
                case PlatformType.X86:
                    return Environment.GetEnvironmentVariable("ProgramFiles");

                case PlatformType.X64:
                    return Environment.GetEnvironmentVariable("ProgramFiles(x86)");

                default:
                    throw new NotSupportedException(
                        "The current platform is neither 32-bit or 64-bit.");
            }
        }

        protected string Get32BitSystemPath()
        {
            string systemRoot = Environment.GetEnvironmentVariable("SystemRoot");
            switch (PlatformInfo.CurrentPlatformType)
            {
                case PlatformType.X86:
                    return Path.Combine(systemRoot, "System32");

                case PlatformType.X64:
                    return Path.Combine(systemRoot, "SysWOW64");

                default:
                    throw new NotSupportedException(
                        "The current platform is neither 32-bit or 64-bit.");
            }
        }

        protected bool IsInstalled()
        {
            return Get32BitUninstallRegistryKey().GetSubKeyNames().Count(n => n == RegistryKeyName) != 0;
        }

        protected string GetUninstallPath(bool stripQuotes)
        {
            string value = (string)Get32BitUninstallRegistryKey().OpenSubKey(RegistryKeyName).GetValue("UninstallString");
            if (stripQuotes)
            {
                value = value.Substring(1, value.Length - 2);
            }
            return value;
        }

        protected string GetInstallPath(string setupPath)
        {
            return Path.Combine(setupPath, SetupFileName);
        }
    }
}
