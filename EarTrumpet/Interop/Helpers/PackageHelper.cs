﻿using System;
using Windows.ApplicationModel;

namespace EarTrumpet.Interop.Helpers
{
    class PackageHelper
    {
        public static Version GetVersion(bool isPackaged)
        {
            if (isPackaged)
            {
                var packageVer = Package.Current.Id.Version;
                return new Version(packageVer.Major, packageVer.Minor, packageVer.Build, packageVer.Revision);
            }
            else
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            }
        }

        public static bool CheckHasIdentity()
        {
#if VSDEBUG
            if (Debugger.IsAttached)
            {
                return false;
            }
#endif

            try
            {
                return Package.Current.Id != null;
            }
            catch (InvalidOperationException ex)
            {
                System.Diagnostics.Trace.WriteLine($"AppExtensions HasIdentity Failed: {ex.Message}");

                // We do not expect this to occur in production when the app is packaged.
                // Async so that the HasIdentity bit is properly set.
#if !DEBUG
                App.Current.Dispatcher.BeginInvoke((Action)(() => Diagnosis.ErrorReporter.LogWarning(ex)));
#endif
                return false;
            }
        }
    }
}
