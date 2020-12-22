using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Factories;
using ProjectReferencesBuilder.Helpers.Interface.WarningHelpers;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ProjectReferencesBuilder.Helpers.WarningHelpers
{
    public class EndOfLifeWarningHelper : IEndOfLifeWarningHelper
    {
        //Taken from https://docs.microsoft.com/en-us/lifecycle/products/microsoft-net-and-net-core
        //TFM list found from https://docs.microsoft.com/en-us/dotnet/standard/frameworks
        private readonly ImmutableDictionary<string, DateTime?> _targetFrameworksWithEndOfLifeDate = new Dictionary<string, DateTime?>
        {
            //.NET Standard does not have a EOL since it is a specification
            { "netstandard1.0", null },
            { "netstandard1.1", null },
            { "netstandard1.2", null },
            { "netstandard1.3", null },
            { "netstandard1.4", null },
            { "netstandard1.5", null },
            { "netstandard1.6", null },
            { "netstandard2.0", null },
            { "netstandard2.1", null },

            //.NET Framework
            //EOL for .NET Framework versions found on https://docs.microsoft.com/en-us/lifecycle/products/microsoft-net-framework
            { "net11", new DateTime(2011, 07, 12) },
            { "v1.1", new DateTime(2011, 07, 12) },
            { "net20", new DateTime(2011, 07, 12) },
            { "v2.0", new DateTime(2011, 07, 12) },
            { "net30", new DateTime(2011, 07, 12) }, //net30 is not shown on Microsoft's exhaustive list of TFMs, so this TFM is assumed
            { "v3.0", new DateTime(2011, 07, 12) },
            { "net35", new DateTime(2029, 01, 09) },
            { "v3.5", new DateTime(2029, 01, 09) },
            { "net40", new DateTime(2016, 01, 12) },
            { "v4.0", new DateTime(2016, 01, 12) },
            { "net403", new DateTime(2016, 01, 12) }, //assumption to match 4.0 and 4.5
            { "v4.0.3", new DateTime(2016, 01, 12) }, //assumption to match 4.0 and 4.5
            { "net45", new DateTime(2016, 01, 12) },
            { "v4.5", new DateTime(2016, 01, 12) },
            { "net451", new DateTime(2016, 01, 12) },
            { "v4.5.1", new DateTime(2016, 01, 12) },
            { "net452", null },
            { "v4.5.2", null },
            { "net46", null },
            { "v4.6", null },
            { "net461", null },
            { "v4.6.1", null },
            { "net462", null },
            { "v4.6.2", null },
            { "net47", null },
            { "v4.7", null },
            { "net471", null },
            { "v4.7.1", null },
            { "net472", null },
            { "v4.7.2", null },
            { "net48", null },
            { "v4.8", null },

            //.NET Core
            //EOL for .NET Core and .NET 5+ found on https://docs.microsoft.com/en-us/lifecycle/products/microsoft-net-and-net-core
            { "netcoreapp1.0", new DateTime(2019, 06, 27) },
            { "netcoreapp1.1", new DateTime(2019, 06, 27) },
            { "netcoreapp2.0", new DateTime(2018, 10, 01) },
            { "netcoreapp2.1", new DateTime(2021, 08, 21) },
            { "netcoreapp2.2", new DateTime(2019, 12, 23) },
            { "netcoreapp3.0", new DateTime(2020, 03, 03) },
            { "netcoreapp3.1", new DateTime(2022, 12, 03) },

            //.NET 5+
            { "net5.0", null },
            { "net5.0-android", null },
            { "net5.0-ios", null },
            { "net5.0-macos", null },
            { "net5.0-tvos", null },
            { "net5.0-watchos", null },
            { "net5.0-windows", null },
        }.ToImmutableDictionary();

        public bool IsProjectTfmEndOfLife(ProjectInfo project, out string warningMessage)
        {
            warningMessage = null;
            if (!_targetFrameworksWithEndOfLifeDate.TryGetValue(project.TFM, out DateTime? eolDate))
            {
                return false;
            }

            if (eolDate.HasValue && eolDate < DateTime.Now.Date)
            {
                warningMessage = WarningMessageFactory.GetEndOfLifeWarning(project, eolDate.Value);
                return true;
            }

            warningMessage = null;
            return false;
        }
    }
}
