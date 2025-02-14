﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.DotNet.Interactive.Commands;
using Microsoft.DotNet.Interactive.CSharp;
using Microsoft.DotNet.Interactive.FSharp;
using Microsoft.DotNet.Interactive.PowerShell;
using Microsoft.DotNet.Interactive.Server;

using System.Threading.Tasks;

namespace Microsoft.DotNet.Interactive.VSCode
{
    public static class KernelExtensions
    {
        public static TKernel UseVSCodeCommands<TKernel>(this TKernel kernel) where TKernel : Kernel
        {
            kernel.RegisterCommandType<GetInput>();
            KernelEventEnvelope.RegisterEvent<InputProduced>();
            return kernel;
        }
        public static CSharpKernel UseVSCodeHelpers(this CSharpKernel kernel)
        {
            var command = new SubmitCode($@"
#r ""{typeof(VSCodeInteractiveHost).Assembly.Location.Replace("\\", "/")}""
using static {typeof(VSCodeInteractiveHost).FullName};
");
            kernel.DeferCommand(command);
            return kernel;
        }

        public static FSharpKernel UseVSCodeHelpers(this FSharpKernel kernel)
        {
            var command = new SubmitCode($@"
#r ""{typeof(VSCodeInteractiveHost).Assembly.Location.Replace("\\", "/")}""

[<AutoOpen>]
module VSCodeTopLevelModule =
    let GetInputAsync ( prompt : string ) ( isPassword : bool) = 
        {typeof(VSCodeInteractiveHost).FullName}.{nameof(VSCodeInteractiveHost.GetInputAsync)}( prompt, isPassword ) |> Async.AwaitTask
");
            kernel.DeferCommand(command);
            return kernel;
        }

        public static PowerShellKernel UseVSCodeHelpers(this PowerShellKernel kernel)
        {
            kernel.ReadInput = prompt => Task.Run(() =>VSCodeInteractiveHost.GetInputAsync(prompt: prompt)).Result;
            kernel.ReadPassword = prompt =>
            {
                var value = Task.Run( () => VSCodeInteractiveHost.GetInputAsync(prompt: prompt, isPassword: true)).Result;
                return new PasswordString(value);
            };

            return kernel;
        }

    }
}
